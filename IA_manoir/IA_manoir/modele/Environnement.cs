using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace IA_manoir.modele
{
    class Environnement
    {
        public List<Noeud> Carte { get; set; }
        public Canvas LeCanvas { get; private set; }
        public Thread thd1 { get; set; }
        public bool bcl { get; set; }
        public int MesurePerformance { get; set; }
        private readonly int TpsActualisation;
        private readonly int PourcentagePoussiere;
        private readonly int PourcentageBijoux;

        public delegate void AddDirty();
        public AddDirty myDelegateDirty;
        public delegate void AddJewels();
        public AddJewels myDelegateJewels;

        public Environnement(int nbCasesH, int nbCasesL, int TpsA, int pourcentP, int pourcentB ,Canvas ca)
        {
            Carte c = new Carte(nbCasesH, nbCasesL);
            Carte = c.Manoir;
            LeCanvas = ca;
            TpsActualisation = TpsA;
            PourcentageBijoux = pourcentB;
            PourcentagePoussiere = pourcentP;
            bcl = true;
            thd1 = new Thread(this.Boucle)
            {
                Name = "Environnement"
            };
            myDelegateDirty = new AddDirty(AjouterPoussière);
            myDelegateJewels = new AddJewels(AjouterBijoux);
        }

        private bool NouvelleCaseSale()
        {
            Random r = new Random();
            if (r.NextDouble() * 100 < PourcentagePoussiere)
                return true;
            return false;
        }

        private void AjouterPoussière()
        {
            Random r = new Random();
            int i = r.Next(Carte.Count);
            if (Carte[i].Contientpoussiere || Carte[i].ContientAspi)
            {
                i = r.Next(Carte.Count);
                int sauv = i;
                while (Carte[i].Contientpoussiere || Carte[i].ContientAspi)
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
            Console.WriteLine("ajout poussiere 111");
        }

        private bool NouveauBijoux()
        {
            Random r = new Random();
            if (r.NextDouble() * 100 < PourcentageBijoux)
                return true;
            return false;
        }

        private void AjouterBijoux()
        {
            Random r = new Random();
            int i = r.Next(Carte.Count);
            if (Carte[i].ContientBijoux || Carte[i].ContientAspi)
            {
                i = r.Next(Carte.Count);
                int sauv = i;
                while (Carte[i].ContientBijoux || Carte[i].ContientAspi)
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
            Console.WriteLine("ajout bijoux 000");
        }

        public void Start()
        {
            thd1.Start();
        }


        public void Boucle()
        {
            Application.Current.Dispatcher.Invoke(this.myDelegateDirty);
            while (bcl)
            {
                if (NouvelleCaseSale())
                {
                    Application.Current.Dispatcher.Invoke(this.myDelegateDirty);
                }
                if (NouveauBijoux())
                {
                    Application.Current.Dispatcher.Invoke(this.myDelegateJewels);
                }
                Console.WriteLine("boucle");
                Thread.Sleep(TpsActualisation);
            }
        }
        public void ArreterBoucle()
        {
            Console.WriteLine("fin boucle env");
            bcl = false;
        }

        public Noeud rechercheNoeud(int x, int y)
        {
            foreach (Noeud n in Carte)
            {
                if(n.X==x && n.Y==y)
                {
                    return n;
                }
            }
            return null;
        }

        public Noeud RechercherAgent()
        {
            foreach (Noeud n in Carte)
            {
                if (n.ContientAspi)
                {
                    return n;
                }
            }
            return null;
        }
    }
}
