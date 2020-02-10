using System.Collections.Generic;

namespace IA_manoir.modele
{
    class Carte
    {
        public List<Noeud> Manoir { get; private set; }
        private readonly Noeud[,] Voisin;

        public Carte(int hauteur, int largeur)
        {
            Manoir = new List<Noeud>();
            Voisin = new Noeud[hauteur, largeur];
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    Noeud n = new Noeud(i, j);
                    Manoir.Add(n);
                    Voisin[i, j] = n;
                }
            }
            AjouterLesVoisinsAuxNoeuds();
        }

        private void AjouterLesVoisinsAuxNoeuds()
        {
            int i;
            int j;
            foreach (Noeud n in Manoir)
            {
                i = n.X;
                j = n.Y;
                if (i != 0)
                {
                    n.AjouterVoisin(Voisin[i - 1, j]);
                }
                if (i != 4)
                {
                    n.AjouterVoisin(Voisin[i + 1, j]);
                }
                if (j != 0)
                {
                    n.AjouterVoisin(Voisin[i, j - 1]);
                }
                if (j != 4)
                {
                    n.AjouterVoisin(Voisin[i, j + 1]);
                }
            }
        }
    }
}
