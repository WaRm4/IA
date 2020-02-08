using System.Collections.Generic;

namespace IA_manoir.modele
{
    class Carte
    {
        public List<Noeud> carte { get; private set; }
        private Noeud[,] voisin;

        public Carte(int hauteur, int largeur)
        {
            carte = new List<Noeud>();
            voisin = new Noeud[hauteur, largeur];
            for(int i=0; i<hauteur; i++)
            {
                for ( int j=0; j<largeur; j++)
                {
                    Noeud n = new Noeud(i, j);
                    carte.Add( n);
                    voisin[i, j] = n;
                }
            }
            AjouterLesVoisinsAuxNoeuds();
        }

        private void AjouterLesVoisinsAuxNoeuds()
        {
            int i;
            int j;
            foreach(Noeud n in carte)
            {
                i = n.x;
                j = n.y;
                if (i!=0)
                {
                    n.AjouterVoisin(voisin[i - 1,j]);
                }
                if (i != 4)
                {
                    n.AjouterVoisin(voisin[i + 1, j]);
                }
                if (j != 0)
                {
                    n.AjouterVoisin(voisin[i, j - 1]);
                }
                if (j != 4)
                {
                    n.AjouterVoisin(voisin[i, j + 1]);
                }
            }
        }
    }
}
