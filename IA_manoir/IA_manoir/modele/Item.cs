using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace IA_manoir.modele
{
    /// <summary>
    /// Classe representant un item ( une poussiere, un bijoux, l'agent) graphiquement, contient son image et sa taille.
    /// </summary>
    class Item
    {
        /// <summary>
        /// Image qui contient l'item.
        /// </summary>
        public Image Image { get; private set; }

        /// <summary>
        /// Le nom de l'item.
        /// </summary>
        public String Nom { get; private set; }

        /// <summary>
        /// Constrcuteur d'un Item.
        /// </summary>
        /// <param name="image"> Le chemin vers l'image de l'item (String). </param>
        /// <param name="nom"> Le nom que l'on veut donner a l'itme (String). </param>
        /// <param name="hauteur"> La hauteur de l'item (la hauteur que l'image prendra) (Int). </param>
        /// <param name="largeur"> La largeur de l'item (la largeur que l'image prendra) (Int). </param>
        public Item(String image, String nom, int hauteur, int largeur)
        {
            this.Nom = nom;
            this.Image = new Image
            {
                Source = new BitmapImage(new Uri(image, UriKind.Relative)),
                Width = hauteur,
                Height = largeur
            };
        }
    }
}
