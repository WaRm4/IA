using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace IA_manoir.modele
{
    class Item
    {

        public Image Image { get; private set; }

        public String Nom { get; private set; }

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
