using System.Collections.Generic;
using System.Windows.Controls;

namespace IA_manoir.modele
{
    /// <summary>
    /// Noeud qui represente une case du manoir.
    /// </summary>
    class Noeud
    {
        /// <summary>
        /// Position X du noeud dans le manoir.
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Position Y du noeud dans le manoir.
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Liste des voisins de ce noeud.
        /// </summary>
        public List<Noeud> Voisins { get; private set; }

        /// <summary>
        /// Booleen pour savoir si ce neoud contient de la poussiere ou non.
        /// </summary>
        public bool Contientpoussiere { get; set; }

        /// <summary>
        /// Booleen pour savoir si ce noeud contient un bijoux ou non.
        /// </summary>
        public bool ContientBijoux { get; set; }

        /// <summary>
        /// Booleen pour savoir si ce noeud contient l'agent ou non.
        /// </summary>
        public bool ContientAgent { get; set; }

        /// <summary>
        /// Poussiere, existe seulement quand ce noeud contient une poussiere.
        /// </summary>
        public Item Poussiere { get; set; }

        /// <summary>
        /// Bijoux, existe seulement quand ce noeud contient un bijoux.
        /// </summary>
        public Item Bijoux { get; set; }

        /// <summary>
        /// Booleen qui permet de savoir si ce noeud a deja ete visite dans un parcours notamment.
        /// </summary>
        public bool Marque { get; set; }

        /// <summary>
        /// Heuristique du noeud par rapport a une poussiere proche, existe seulement quand on utilise un parcours informe.
        /// </summary>
        public int Heuristique { get; set; }

        /// <summary>
        /// Constructeur d'un noeud.
        /// </summary>
        /// <param name="y"> Coordonnee Y du noeud dans l'environnement (ici le manoir). </param>
        /// <param name="x"> Coordonnee X du noeud dans l'environnement (ici le manoir). </param>
        public Noeud(int y, int x)
        {
            X = x;
            Y = y;
            ContientAgent = false;
            ContientBijoux = false;
            Contientpoussiere = false;
            Voisins = new List<Noeud>();
            Marque = false;
            Heuristique = -1;
        }

        /// <summary>
        /// Methode permettant d'ajouter un voisin au noeud.
        /// </summary>
        /// <param name="n"> Le noeud voisin (Noeud). </param>
        public void AjouterVoisin(Noeud n)
        {
            Voisins.Add(n);
        }

        /// <summary>
        /// Methode permettant de mettre une poussiere dans ce noeud.
        /// </summary>
        /// <param name="pouss"> La poussiere a ajouter (Item). </param>
        public void AjoutPoussiere(Item pouss)
        {
            Poussiere = pouss;
        }

        /// <summary>
        /// Methode permettant de mettre un bijoux dans ce noeud.
        /// </summary>
        /// <param name="pouss"> Le bijoux a ajouter (Item). </param>
        public void AjoutBijoux(Item bij)
        {
            Bijoux = bij;
        }
    }
}
