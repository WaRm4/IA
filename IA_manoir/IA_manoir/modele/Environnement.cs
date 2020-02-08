using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IA_manoir.modele
{
    class Environnement
    {
        private List<Noeud> carte { get; set; }

        public Canvas leCanvas { get; private set; }
        public Environnement(int nbCasesH, int nbCasesL, Canvas ca)
        {
            Carte c = new Carte(nbCasesH, nbCasesL);
            carte = c.carte;
            leCanvas = ca;
        }

        private bool NouvelleCaseSale()
        {
            Random r = new Random();
            if (r.NextDouble() > 0.75)
                return true;
            return false;
        }

        private void AjouterPoussière()
        {
            Random r = new Random();
            int i = r.Next(25);

            if ( !carte[i].Contientpoussiere)
            {
                carte[i].Contientpoussiere = true;
                Item poussiere = new Item("images/poussiere.png", "poussiere",20,20);
                Canvas.SetLeft(poussiere.image, 5 + carte[i].x * 60);
                Canvas.SetTop(poussiere.image, 5 + carte[i].y * 60);
                leCanvas.Children.Add(poussiere.image);
                Console.WriteLine("ajout poussiere avec random");
            }
            else 
            {
                foreach(Noeud n in carte)
                {
                    if(!n.Contientpoussiere)
                    {
                        n.Contientpoussiere = true;
                        Item poussiere = new Item("images/poussiere.png", "poussiere", 20, 20);
                        Canvas.SetLeft(poussiere.image, 5 + n.x * 60);
                        Canvas.SetTop(poussiere.image, 5 + n.y * 60);
                        leCanvas.Children.Add(poussiere.image);
                        Console.WriteLine("ajout poussiere avec non random");
                        return;
                    }

                }
            }
        }

        private bool NouveauBijoux()
        {
            Random r = new Random();
            if (r.NextDouble() > 0.9)
                return true;
            return false;
        }

        private void AjouterBijoux()
        {
            Random r = new Random();
            int i = r.Next(25);

            if (!carte[i].ContientBijoux)
            {
                carte[i].ContientBijoux = true;
                Item poussiere = new Item("images/bijoux.png", "bijoux", 20, 20);
                Canvas.SetLeft(poussiere.image, 40 + carte[i].x * 60);
                Canvas.SetTop(poussiere.image, 40 + carte[i].y * 60);
                leCanvas.Children.Add(poussiere.image);
                Console.WriteLine("ajout poussiere avec random");
            }
            else
            {
                foreach (Noeud n in carte)
                {
                    if (!n.ContientBijoux)
                    {
                        n.ContientBijoux = true;
                        Item poussiere = new Item("images/bijoux.png", "bijoux", 20, 20);
                        Canvas.SetLeft(poussiere.image, 40 + n.x * 60);
                        Canvas.SetTop(poussiere.image, 40 + n.y * 60);
                        leCanvas.Children.Add(poussiere.image);
                        Console.WriteLine("ajout poussiere avec non random");
                        return;
                    }

                }
            }
        }

        public void start ()
        {
            Parallel.Invoke(() => boucle());
        }

        
        public void boucle ()
        {
            int i = 0;
            while(i<5)
            {
                if(NouvelleCaseSale())
                {
                    AjouterPoussière();
                }
                if(NouveauBijoux())
                {
                    AjouterBijoux();
                }
                Thread.Sleep(1000);
            }
        }

    }
}
