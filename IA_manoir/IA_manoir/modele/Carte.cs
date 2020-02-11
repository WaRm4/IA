using System.Collections.Generic;

namespace IA_manoir.modele
{
    /// <summary>
    /// Classe qui represente le graphe de l'environnement.
    /// </summary>
    class Carte
    {
        /// <summary>
        /// Graphe de Noeud sous forme de liste.
        /// </summary>
        public List<Noeud> Manoir { get; private set; }

        /// <summary>
        /// Tableau 2D servant pour definir les voisins d'un noeud.
        /// </summary>
        private readonly Noeud[,] Voisin;

        /// <summary>
        /// Constructeur de la carte.
        /// </summary>
        /// <param name="hauteur"> Le nombre de case en hauteur de l'environnement (Entier). </param>
        /// <param name="largeur"> Le nombre de case en largeur de l'environnement (Entier). </param>
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

        /// <summary>
        /// Methode qui met pour chaque noeud ses voisins ( les noeuds qui lui sont adjacents).
        /// </summary>
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
