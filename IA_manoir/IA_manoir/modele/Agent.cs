using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace IA_manoir.modele
{
    class Agent
    {
        private readonly int EnergieMax;
        private readonly int EnergieParAction;
        public int EnergieDepensee { get; set; }
        public int PoussiereAspiree { get; set; }
        public int BijouxRamasse { get; set; }
        public int BijouxAspire { get; set; }
        public Capteur MonCapteur { get; set; }
        public Effecteur MonEffecteur { get; set; }
        public Thread Thd { get; set; }
        public Item AgentI { get; set; }
        private readonly bool Informe;

        public delegate void SpawnAgent(Image image, int posLeft, int posTop);
        public SpawnAgent myDelegate;
        public delegate void FinEnvironnement();
        public FinEnvironnement delegueFinEnv;
        public delegate void FinAspi();
        public FinAspi delegueFinAspi;
        public delegate void ActusalisationStats();
        public ActusalisationStats delegueActualisation;

        public Agent(int energie, int energieMouv, int TpsAction, bool informe, Environnement env)
        {
            EnergieMax = energie;
            EnergieParAction = energieMouv;
            EnergieDepensee = 0;
            PoussiereAspiree = 0;
            BijouxRamasse = 0;
            BijouxAspire = 0;
            Informe = informe;
            MonCapteur = new Capteur(env);
            MonEffecteur = new Effecteur(TpsAction);
            AgentI = new Item("images/aspi.png", "agent", 50, 50);

            Thd = new Thread(this.FonctionAgent)
            {
                Name = "agent"
            };
            myDelegate = new SpawnAgent(MainWindow.PlacerElement);
            delegueFinEnv = new FinEnvironnement(MainWindow.FinEnvironnement);
            delegueFinAspi = new FinAspi(MainWindow.FinAspi);
            delegueActualisation = new ActusalisationStats(MainWindow.ActualisationStats);
        }

        public void Start()
        {
            Thd.Start();
            MonCapteur.Start();
        }

        public void Stop()
        {
            Console.WriteLine("arret agent");
            MonCapteur.Stop();
            Thd.Abort();
        }

        public void Spawn(List<Noeud> liste)
        {
            liste[12].ContientAspi = true;
            Application.Current.Dispatcher.Invoke(myDelegate, new Object[] { AgentI.Image, liste[12].X * 60 + 5, liste[12].Y * 60 + 5 });
        }

        public Noeud ParcoursLargeur(List<Noeud> liste)
        {
            Noeud depart = MonCapteur.Env.RechercherAgent();
            if (depart == null)
            {
                this.Stop();
            }
            MettreMarquesFalse(liste);
            Queue<Noeud> file = new Queue<Noeud>();
            file.Enqueue(depart);
            depart.Marque = true;
            while (file.Count != 0)
            {
                depart = file.Dequeue();
                if (depart.Contientpoussiere)
                    return depart;
                foreach (Noeud n in depart.Voisins)
                {
                    if (!n.Marque)
                    {
                        file.Enqueue(n);
                        n.Marque = true;
                    }
                }
            }
            return null;
        }

        public Queue<String> ListeActions(Noeud nArrivee, List<Noeud> liste)
        {

            int x, y;
            Queue<String> file = new Queue<String>();
            Noeud depart = MonCapteur.Env.RechercherAgent();
            if (depart == null)
            {
                this.Stop();
            }
            x = depart.X;
            y = depart.Y;
            String action = "";
            while (x != nArrivee.X)
            {
                if (x < nArrivee.X)
                {
                    x = x + 1;
                    action = "droite";
                    file.Enqueue(action);
                }
                if (x > nArrivee.X)
                {
                    x = x - 1;
                    action = "gauche";
                    file.Enqueue(action);
                }
            }
            while (y != nArrivee.Y)
            {
                if (y < nArrivee.Y)
                {
                    y = y + 1;
                    action = "bas";
                    file.Enqueue(action);
                }
                if (y > nArrivee.Y)
                {
                    y = y - 1;
                    action = "haut";
                    file.Enqueue(action);
                }
            }
            if (nArrivee.ContientBijoux)
            {
                action = "ramasser";
                file.Enqueue(action);
            }
            action = "aspirer";
            file.Enqueue(action);
            return file;
        }

        private void MettreMarquesFalse(List<Noeud> liste)
        {
            foreach (Noeud n in liste)
            {
                n.Marque = false;
            }
        }

        private void FonctionAgent()
        {
            Application.Current.Dispatcher.Invoke(delegueActualisation);
            List<Noeud> environnement = MonCapteur.RenvoyerEnvironnement();
            Thread.Sleep(200);
            Spawn(environnement);
            while (EnergieDepensee < EnergieMax)
            {
                environnement = MonCapteur.RenvoyerEnvironnement();
                Noeud arrivee;
                if (Informe)
                    arrivee = ParcoursGloutonne(environnement);
                else
                    arrivee = ParcoursLargeur(environnement);
                if (arrivee == null)
                {
                    Application.Current.Dispatcher.Invoke(delegueFinEnv);
                    this.Stop();
                }
                Queue<String> actions = ListeActions(arrivee, environnement);
                if (actions.Count > EnergieMax - EnergieDepensee)
                {
                    Queue<String> act2 = new Queue<string>();
                    for (int i = 0; i < EnergieMax - EnergieDepensee; i++)
                    {
                        act2.Enqueue(actions.Dequeue());
                    }
                    actions = act2;
                }
                EffectuerActions(actions);

            }
            Application.Current.Dispatcher.Invoke(delegueFinAspi);
            this.Stop();
        }

        private void EffectuerActions(Queue<String> actions)
        {
            Noeud agent = MonCapteur.Env.RechercherAgent();
            if (agent == null)
            {
                this.Stop();
            }
            Noeud arr;
            int x = agent.X;
            int y = agent.Y;
            foreach (String st in actions)
            {
                agent = MonCapteur.Env.RechercherAgent();
                EnergieDepensee += EnergieParAction;
                if (st == "ramasser")
                {
                    MonEffecteur.Ramasser(agent);
                    BijouxRamasse++;
                    Application.Current.Dispatcher.Invoke(delegueActualisation);
                    return;
                }
                if (st == "aspirer")
                {
                    MonEffecteur.Aspirer(agent);
                    PoussiereAspiree++;
                    if (agent.ContientBijoux)
                        BijouxRamasse++;
                    Application.Current.Dispatcher.Invoke(delegueActualisation);
                    return;
                }
                if (st == "haut")
                {
                    y -= 1;
                }
                if (st == "bas")
                {
                    y += 1;
                }
                if (st == "gauche")
                {
                    x -= 1;
                }
                if (st == "droite")
                {
                    x += 1;
                }
                arr = MonCapteur.Env.rechercheNoeud(x, y);
                MonEffecteur.SeDeplacer(arr, agent, AgentI.Image);
                Application.Current.Dispatcher.Invoke(delegueActualisation);
            }
        }
        public Noeud ParcoursGloutonne(List<Noeud> liste)
        {
            Noeud depart = MonCapteur.Env.RechercherAgent();
            Noeud noeud;
            if (depart == null)
            {
                this.Stop();
            }
            MettreMarquesFalse(liste);
            CalculerHeuristique(liste);
            Queue<Noeud> file = new Queue<Noeud>();
            file.Enqueue(depart);
            depart.Marque = true;
            while (file.Count != 0)
            {
                depart = file.Dequeue();
                if (depart.Heuristique == 0)
                {
                    return depart;
                }
                noeud = depart.Voisins[0];
                foreach (Noeud n in depart.Voisins)
                {
                    if (n.Heuristique <= noeud.Heuristique && !n.Marque)
                    {
                        noeud = n;
                    }
                }
                file.Enqueue(noeud);
            }
            return null;
        }
        private void MettreHeuristiqueNull(List<Noeud> liste)
        {
            foreach (Noeud n in liste)
            {
                n.Heuristique = -1;
            }
        }
        public void CalculerHeuristique(List<Noeud> liste)
        {
            this.MettreHeuristiqueNull(liste);
            foreach (Noeud n in liste)
            {
                this.longueurPoussierPlusProche(n, liste);
            }
        }
        private void longueurPoussierPlusProche(Noeud n, List<Noeud> liste)
        {
            int longueur = 0;
            int intermediaire;
            if (n.Contientpoussiere)
            {
                n.Heuristique = 0;
                return;
            }
            foreach (Noeud noeud in liste)
            {
                if (noeud.Contientpoussiere)
                {
                    intermediaire = Math.Abs(noeud.X - n.X) + Math.Abs(noeud.Y - n.Y);
                    if (n.Heuristique == -1 || n.Heuristique > intermediaire)
                    {
                        longueur = intermediaire;
                    }
                }
            }
            n.Heuristique = longueur;
            return;
        }

    }
}
