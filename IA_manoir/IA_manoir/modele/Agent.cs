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
        private readonly int EnergieParMouvement;
        public int EnergieDepensee { get; set; }
        public Capteur MonCapteur { get; set; }
        public Effecteur MonEffecteur { get; set; }
        public Thread Thd { get; set; }
        public Item AgentI { get; set; }

        public delegate void SpawnAgent(Image image, int posLeft, int posTop);
        public SpawnAgent myDelegate;

        public Agent(int energie, int energieMouv, Environnement env)
        {
            EnergieMax = energie;
            EnergieParMouvement = energieMouv;
            EnergieDepensee = 0;
            MonCapteur = new Capteur(env);
            MonEffecteur = new Effecteur();
            AgentI = new Item("images/aspi.png", "agent", 50, 50);

            Thd = new Thread(this.FonctionAgent)
            {
                Name = "agent"
            };
            myDelegate = new SpawnAgent(MainWindow.PlacerElement);
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
            Application.Current.Dispatcher.Invoke(myDelegate, new Object[]{AgentI.Image, liste[12].X * 60 + 5, liste[12].Y * 60 + 5});
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
            if(depart == null)
            {
                this.Stop();
            }
            x = depart.X;
            y = depart.Y;
            String action = "";
            while (x != nArrivee.X)
            {
                if (x < nArrivee.X )
                {
                    x = x + 1;
                    action = "droite";
                    file.Enqueue(action);
                }
                if (x > nArrivee.X )
                {
                    x = x - 1;
                    action = "gauche";
                    file.Enqueue(action);
                }
            }
            while (y != nArrivee.Y)
            { 
                if (y < nArrivee.Y )
                {
                    y = y + 1;
                    action = "bas";
                    file.Enqueue(action);
                }
                if (y > nArrivee.Y )
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
            List<Noeud> environnement = MonCapteur.RenvoyerEnvironnement();
            Thread.Sleep(500);
            Spawn(environnement);
            while (EnergieDepensee <= EnergieMax)
            {
                environnement = MonCapteur.RenvoyerEnvironnement();
                Noeud arrivee = ParcoursLargeur(environnement);
                if (arrivee == null)
                {
                    Console.WriteLine("propre !!!!!");
                    this.Stop();
                }
                Queue<String> actions = ListeActions(arrivee, environnement);
                EffectuerActions(actions);
                
            }
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
            int y= agent.Y;
            foreach (String st in actions)
            {
                agent = MonCapteur.Env.RechercherAgent();
                EnergieDepensee += EnergieParMouvement;
                if (st == "ramasser")
                {
                    MonEffecteur.Ramasser(agent);
                    return;
                }
                if (st == "aspirer")
                {
                    MonEffecteur.Aspirer(agent);
                    return;
                }
                if (st== "haut")
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
                MonEffecteur.SeDeplacer(arr, agent,AgentI.Image);
            }
        }
    }
}
