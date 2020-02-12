using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace IA_manoir.modele
{
    /// <summary>
    /// Classe qui represente l'environnement dans lequel l'agent evoluera.
    /// </summary>
    class Environnement
    {
        /// <summary>
        /// Graphe de notre carte (represente le manoir dans notre projet).
        /// </summary>
        public List<Noeud> Carte { get; set; }

        /// <summary>
        /// Thread sur lequel fonctionnera l'environnement.
        /// </summary>
        public Thread Thd { get; set; }

        /// <summary>
        /// Booleen permettant d'arreter la boucle si mit a false.
        /// </summary>
        public bool Boucler { get; set; }

        /// <summary>
        /// Mesure de performance que l'agent pourra consulter.
        /// De base est à 100, qui est une bonne performance et est decrementee a chaque erreur.
        /// </summary>
        public int MesurePerformance { get; set; }

        /// <summary>
        /// Entier qui defini le temps entre chaque spawn de pussiere/bijoux, en millisecondes.
        /// </summary>
        private readonly int TpsActualisation;

        /// <summary>
        /// Entier qui definit a quel pourcentage une poussiere pourra spwan.
        /// </summary>
        private readonly int PourcentagePoussiere;

        /// <summary>
        /// Entier qui definit a quel pourcentage un bijoux pourra spwan.
        /// </summary>
        private readonly int PourcentageBijoux;

        // Delegues pour effectuer des actions sur le thread principal (notamment pour la partie graphique).
        public delegate void AjouterPoussiereGraphique();
        public AjouterPoussiereGraphique DeleguePoussiere;
        public delegate void AjouterBijouxGraphique();
        public AjouterBijouxGraphique DelegueBijoux;
        //**************************************************************************************************

        /// <summary>
        /// Constructeur de l'environnement avec ses differents parametres.
        /// </summary>
        /// <param name="nbCasesH"> Nombre de cases du manoir en hateur (Entier). </param>
        /// <param name="nbCasesL"> Nombre de cases du manoir en largeur (Entier). </param>
        /// <param name="TpsA"> Temps qui defini le temps entre chaque spawn de pussiere/bijoux, en millisecondes (Entier).  </param>
        /// <param name="pourcentP"> Pourcentage pour qu'une poussiere spawn (Entier). </param>
        /// <param name="pourcentB"> Pourcentage pour qu'un bijoux spawn (Entier). </param>
        public Environnement(int nbCasesH, int nbCasesL, int TpsA, int pourcentP, int pourcentB)
        {
            Carte c = new Carte(nbCasesH, nbCasesL);
            Carte = c.Manoir;
            TpsActualisation = TpsA;
            PourcentageBijoux = pourcentB;
            PourcentagePoussiere = pourcentP;
            MesurePerformance = 100;
            Boucler = true;
            Thd = new Thread(this.Boucle)
            {
                Name = "Environnement"
            };
            DeleguePoussiere = new AjouterPoussiereGraphique(AjouterPoussière);
            DelegueBijoux = new AjouterBijouxGraphique(AjouterBijoux);
        }

        /// <summary>
        /// Methode qui definit si une nouvelle poussiere doit apparaitre ou non en fonction du pourcentage.
        /// </summary>
        /// <returns> Booleen true ou false. </returns>
        private bool NouvellePoussiere()
        {
            Random r = new Random();
            if (r.NextDouble() * 100 < PourcentagePoussiere)
                return true;
            return false;
        }

        /// <summary>
        /// Methode qui ajoute une poussiere aleatoirement dans le manoir.
        /// </summary>
        private void AjouterPoussière()
        {
            Random r = new Random();
            int i = r.Next(Carte.Count);
            if (Carte[i].Contientpoussiere || Carte[i].ContientAgent)
            {
                i = r.Next(Carte.Count);
                int sauv = i;
                while (Carte[i].Contientpoussiere || Carte[i].ContientAgent)
                {
                    i++;
                    if (i >= Carte.Count)
                    {
                        i = 0;
                    }
                    if (i == sauv)
                    {
                        return;
                    }
                }
            }
            Carte[i].Contientpoussiere = true;
            Item poussiere = new Item("images/poussiere.png", "poussiere", 20, 20);
            MainWindow.PlacerElement(poussiere.Image, 5 + Carte[i].X * 60, 5 + Carte[i].Y * 60);
            Carte[i].AjoutPoussiere(poussiere);
        }

        /// <summary>
        /// Methode qui definit si un nouveau bijoux doit apparaitre ou non en fonction du pourcentage.
        /// </summary>
        /// <returns> Booleen true ou false. </returns>
        private bool NouveauBijoux()
        {
            Random r = new Random();
            if (r.NextDouble() * 100 < PourcentageBijoux)
                return true;
            return false;
        }

        /// <summary>
        /// Methode qui ajoute un bijoux aleatoirement dans le manoir.
        /// </summary>
        private void AjouterBijoux()
        {
            Random r = new Random();
            int i = r.Next(Carte.Count);
            if (Carte[i].ContientBijoux || Carte[i].ContientAgent)
            {
                i = r.Next(Carte.Count);
                int sauv = i;
                while (Carte[i].ContientBijoux || Carte[i].ContientAgent)
                {
                    i++;
                    if (i >= Carte.Count)
                    {
                        i = 0;
                    }
                    if (i == sauv)
                    {
                        return;
                    }
                }
            }
            Carte[i].ContientBijoux = true;
            Item bijoux = new Item("images/bijoux.png", "bijoux", 20, 20);
            MainWindow.PlacerElement(bijoux.Image, 40 + Carte[i].X * 60, 40 + Carte[i].Y * 60);
            Carte[i].AjoutBijoux(bijoux);
        }

        /// <summary>
        /// Methode qui demarre le thread de l'environnement.
        /// </summary>
        public void Start()
        {
            Thd.Start();
        }

        /// <summary>
        /// Boucle de l'environnement, qui genere un environnement aleatoire.
        /// </summary>
        public void Boucle()
        {
            Application.Current.Dispatcher.Invoke(this.DeleguePoussiere);
            while (Boucler)
            {
                if (NouvellePoussiere())
                {
                    Application.Current.Dispatcher.Invoke(this.DeleguePoussiere);
                }
                if (NouveauBijoux())
                {
                    Application.Current.Dispatcher.Invoke(this.DelegueBijoux);
                }
                Thread.Sleep(TpsActualisation);
            }
        }

        /// <summary>
        /// Methode qui arrete la boucle de l'environnement.
        /// </summary>
        public void ArreterBoucle()
        {
            Boucler = false;
        }

        /// <summary>
        /// Methode qui permet de rechercher un noeud en fonction de ses coordonnees.
        /// </summary>
        /// <param name="x"> Coordonnee X du Noeud (Entier). </param>
        /// <param name="y"> Coordonnee Y du Noeud (Entier). </param>
        /// <returns> Le Noeud qui correspond aux coordonnees (Noeud). </returns>
        public Noeud RechercheNoeud(int x, int y)
        {
            foreach (Noeud n in Carte)
            {
                if (n.X == x && n.Y == y)
                {
                    return n;
                }
            }
            return null;
        }

        /// <summary>
        /// Methode qui permet de trouver ou se trouve l'agent parmis tous les noeuds.
        /// </summary>
        /// <returns> Le noeud ou se trouve notre agent (Noeud). </returns>
        public Noeud RechercherAgent()
        {
            foreach (Noeud n in Carte)
            {
                if (n.ContientAgent)
                {
                    return n;
                }
            }
            return null;
        }
    }
}
