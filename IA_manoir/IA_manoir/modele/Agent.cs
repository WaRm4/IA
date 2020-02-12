using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace IA_manoir.modele
{
    /// <summary>
    /// Classe agent = notre intelligence artificielle, dans le cadre de notre projet c'est un aspirateur.
    /// </summary>
    class Agent
    {
        /// <summary>
        /// Energie maximum que l'agent pourra depenser.
        /// </summary>
        private readonly int EnergieMax;

        /// <summary>
        /// Energie depensee pour chaque action faite par l'agent.
        /// </summary>
        private readonly int EnergieParAction;

        /// <summary>
        /// Energie depensee a un moment precis par l'agent.
        /// </summary>
        public int EnergieDepensee { get; set; }

        /// <summary>
        /// Nombre de poussieres aspirees par l'agent a un moment precis.
        /// </summary>
        public int PoussiereAspiree { get; set; }

        /// <summary>
        /// Nombre de bijoux ramasses par l'agent a un moment precis.
        /// </summary>
        public int BijouxRamasse { get; set; }

        /// <summary>
        /// Nombre de bijoux aspires par l'agent a un moment precis.
        /// </summary>
        public int BijouxAspire { get; set; }

        /// <summary>
        /// Le capteur que detient l'agent pour obtenir des informations sur l'environnement.
        /// </summary>
        public Capteur MonCapteur { get; set; }

        /// <summary>
        /// Effecteur de l'agent pour lui permettre d'effectuer ses differentes actions.
        /// </summary>
        public Effecteur MonEffecteur { get; set; }

        /// <summary>
        /// Thread sur lequel fonctionnera l'agent.
        /// </summary>
        public Thread Thd { get; set; }

        /// <summary>
        /// agent (graphique) contient l'image et la taille de l'image.
        /// </summary>
        public Item AgentI { get; set; }

        /// <summary>
        /// Variable pour savoir si l'agent devra faire un parcours informe ou non.
        /// </summary>
        private readonly bool Informe;

        //Delegues pour effectuer des actions sur le thread principal (notamment pour la partie graphique).
        public delegate void SpawnAgent(Image image, int posLeft, int posTop);
        public SpawnAgent DelegueAgent;
        public delegate void FinEnvironnement();
        public FinEnvironnement DelegueFinEnv;
        public delegate void FinAspi();
        public FinAspi DelegueFinAspi;
        public delegate void ActusalisationStats();
        public ActusalisationStats DelegueActualisation;
        //*************************************************************************************************

        /// <summary>
        /// Constructeur d'un agent avec tous ses parametres.
        /// Creer un nouveau thread.
        /// </summary>
        /// <param name="energie"> Energie maximum que pourra consommer l'agent (entier). </param>                      
        /// <param name="energieMouv"> Energie que consommera l'agent pour un mouvement (entier). </param>                       
        /// <param name="TpsAction"> Temps que mettra l'agent a effectuer une action (entier, en milliseconde). </param>                   
        /// <param name="informe"> Booleen pour savoir si le parcours que l'agent fera est informe ou non (Bool). </param>                
        /// <param name="env"> Environnement dans lequel l'agent agira (Environnement). </param>
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
            DelegueAgent = new SpawnAgent(MainWindow.PlacerElement);
            DelegueFinEnv = new FinEnvironnement(MainWindow.FinEnvironnement);
            DelegueFinAspi = new FinAspi(MainWindow.FinAspi);
            DelegueActualisation = new ActusalisationStats(MainWindow.ActualisationStats);
        }

        /// <summary>
        /// Methode qui permet de demarrer les threads de l'agent et de l'environnement.
        /// </summary>
        public void Start()
        {
            Thd.Start();
            MonCapteur.Start();
        }

        /// <summary>
        /// Methode qui permet d'arreter les boucles de l'environnement et de l'agent, et leur threads.
        /// </summary>
        public void Stop()
        {
            MonCapteur.Stop();
            Thd.Abort();
        }

        /// <summary>
        /// Methode qui place l'agent au milieu de l'environnement, definie comme position de base de l'agent.
        /// </summary>
        /// <param name="liste"> L'environnement (sous forme de graphe avec ses differents noeuds) (List<Noeud>) </param>
        public void Spawn(List<Noeud> liste)
        {
            liste[12].ContientAgent = true;
            Application.Current.Dispatcher.Invoke(DelegueAgent, new Object[] { AgentI.Image, liste[12].X * 60 + 5, liste[12].Y * 60 + 5 });
        }

        /// <summary>
        /// Methode qui effectue un parcours en largeur pour chercher la poussiere la plus proche.
        /// </summary>
        /// <param name="liste"> L'environnement (sous forme de graphe avec ses differents noeuds) (List<Noeud>) </param>
        /// <returns> Noeud :  le noeud sur lequel se trouve la poussiere la plus proche. </returns>
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

        /// <summary>
        /// Methode qui definit une liste d'actions que devra faire l'agent.
        /// </summary>
        /// <param name="nArrivee"> Noeud ou doit se rendre l'agent (Noeud). </param>
        /// <param name="liste"> L'environnement (sous forme de graphe avec ses differents noeuds) (List<Noeud>) </param>
        /// <returns> Liste d'actions a suivre par l'agent (List<String>) </returns>
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

        /// <summary>
        /// Methode qui sert au parcours en largeur, elle met la marque de chaque noeud a false.
        /// </summary>
        /// <param name="liste"> L'environnement (sous forme de graphe avec ses differents noeuds) (List<Noeud>) </param>
        private void MettreMarquesFalse(List<Noeud> liste)
        {
            foreach (Noeud n in liste)
            {
                n.Marque = false;
            }
        }

        /// <summary>
        /// Fonction de l'agent, boucle ou l'agent defini ses actions en fonction de son environnement.
        /// </summary>
        private void FonctionAgent()
        {
            Application.Current.Dispatcher.Invoke(DelegueActualisation);
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
                    Application.Current.Dispatcher.Invoke(DelegueFinEnv);
                    this.Stop();
                }
                Queue<String> actions = ListeActions(arrivee, environnement);
                if(MonCapteur.RenvoyerMesurePerformance() <= 50) //Une mesure de performance a 50 sera consideree comme mauvaise.
                {
                    Queue<String> act2 = new Queue<string>();
                    for (int i = 0; i < actions.Count / 2 ; i++)// On reduit donc de moitie le nombre d'actions,  
                    {                                           // pour que l'agent puisse actualiser ses actions plus souvent.
                        act2.Enqueue(actions.Dequeue());
                    }
                    actions = act2;
                }
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
            Application.Current.Dispatcher.Invoke(DelegueFinAspi);
            this.Stop();
        }

        /// <summary>
        /// Methode permettant de traduire la liste d'actions en actions que l'agent effectue.
        /// </summary>
        /// <param name="actions"> Liste d'actions que l'agent doit faire (List<String>) </param>
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
                    Application.Current.Dispatcher.Invoke(DelegueActualisation);
                    return;
                }
                if (st == "aspirer")
                {
                    MonEffecteur.Aspirer(agent);
                    PoussiereAspiree++;
                    if (agent.ContientBijoux)
                        BijouxRamasse++;
                    Application.Current.Dispatcher.Invoke(DelegueActualisation);
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
                arr = MonCapteur.Env.RechercheNoeud(x, y);
                MonEffecteur.SeDeplacer(arr, agent, AgentI.Image);
                Application.Current.Dispatcher.Invoke(DelegueActualisation);
            }
        }

        /// <summary>
        /// Methode qui effectue un parcours Glouton, parcours qui est informe.
        /// </summary>
        /// <param name="liste"> L'environnement (sous forme de graphe avec ses differents noeuds) (List<Noeud>) </param>
        /// <returns> Le noeud le plus interressant en termes d'heuristique. </returns>
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

        /// <summary>
        /// Methode qui met les heuristiques de chaque noeud a null.
        /// </summary>
        /// <param name="liste"> L'environnement (sous forme de graphe avec ses differents noeuds) (List<Noeud>) </param>
        private void MettreHeuristiqueNull(List<Noeud> liste)
        {
            foreach (Noeud n in liste)
            {
                n.Heuristique = -1;
            }
        }

        /// <summary>
        /// Methode qui permet d'appliquer une heuristique pour chaque noeud. 
        /// </summary>
        /// <param name="liste"> L'environnement (sous forme de graphe avec ses differents noeuds) (List<Noeud>) </param>
        public void CalculerHeuristique(List<Noeud> liste)
        {
            this.MettreHeuristiqueNull(liste);
            foreach (Noeud n in liste)
            {
                this.LongueurPoussierPlusProche(n, liste);
            }
        }

        /// <summary>
        /// Methode qui calcule une heuristique en fonction de la longueur a parcourir jusqu'a une poussiere.
        /// </summary>
        /// <param name="n"> Noeud pour lequel on doit definir une heuristique (Noeud). </param>
        /// <param name="liste"> L'environnement (sous forme de graphe avec ses differents noeuds) (List<Noeud>) </param>
        private void LongueurPoussierPlusProche(Noeud n, List<Noeud> liste)
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
