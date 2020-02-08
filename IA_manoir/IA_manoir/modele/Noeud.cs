using System.Collections.Generic;

namespace IA_manoir.modele
{
    class Noeud
    {
        public int x { get; private set; }

        public int y { get; private set; }

        public List<Noeud> voisins { get; private set; }

        public bool Contientpoussiere { get; set; }

        public bool ContientBijoux { get; set; }

        public bool ContientAspi { get; set; }

        public Noeud(int y, int x)
        {
            this.x = x;
            this.y = y;
            ContientAspi = false;
            ContientBijoux = false;
            Contientpoussiere = false;
            voisins = new List<Noeud>();
        }

        public void AjouterVoisin(Noeud n)
        {
            voisins.Add(n);
        }
    }
}
