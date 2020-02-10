using System.Collections.Generic;
using System.Windows.Controls;

namespace IA_manoir.modele
{
    class Noeud
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public List<Noeud> Voisins { get; private set; }
        public bool Contientpoussiere { get; set; }
        public bool ContientBijoux { get; set; }
        public bool ContientAspi { get; set; }
        public Item Poussiere { get; set; }
        public Item Bijoux { get; set; }
        public bool Marque { get; set; }
        public int Heuristique { get; set; }

        public Noeud(int y, int x)
        {
            X = x;
            Y = y;
            ContientAspi = false;
            ContientBijoux = false;
            Contientpoussiere = false;
            Voisins = new List<Noeud>();
            Marque = false;
            Heuristique = -1;
        }

        public void AjouterVoisin(Noeud n)
        {
            Voisins.Add(n);
        }

        public void AjoutPoussiere(Item pouss)
        {
            Poussiere = pouss;
        }

        public void AjoutBijoux(Item bij)
        {
            Bijoux = bij;
        }
    }
}
