using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace IA_manoir.modele
{
    class Item
    {

        public Image image { get; private set; }

        public String nom { get; private set; }

        public Item(String image, String nom, int hauteur, int largeur) 
        {
            this.nom = nom;
            this.image = new Image
            {
                Source = new BitmapImage(new Uri(image, UriKind.Relative)),
                Width = hauteur,
                Height = largeur
            };
        }

        

    }
}
