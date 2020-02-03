using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace IA_manoir
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int[,] tab = new int[5, 5]
            { { 1, 0, 0, 0, 0 },
              { 0, 0, 0, 0, 0 },
              { 0, 0, 2, 0, 0 },
              { 0, 0, 0, 0, 0 },
              { 0, 0, 0, 0, 0 } };

        public MainWindow()
        {
            InitializeComponent();

            Image poussiere = new Image
            {
                Source = new BitmapImage(new Uri("images/poussiere.png", UriKind.Relative)),
                Width = 30,
                Height = 30
            };
            Canvas.SetLeft(poussiere, 15);
            Canvas.SetTop(poussiere, 15);
            lecanvas.Children.Add(poussiere);

            Image img = new Image
            {
                Source = new BitmapImage(new Uri("images/aspi.png", UriKind.Relative)),
                Width = 50,
                Height = 50
            };
            Canvas.SetLeft(img, 125);
            Canvas.SetTop(img, 125);
            lecanvas.Children.Add(img);

        }

        private void Poussiere(object sender, RoutedEventArgs e)
        {
            RandomPoussiere();
        }

        private void Bijoux(object sender, RoutedEventArgs e)
        {

        }

        private void Robot(object sender, RoutedEventArgs e)
        {

        }

        private void RandomPoussiere()
        {
            Random r = new Random();
            int i = r.Next(5);
            int j = r.Next(5);

            if ( tab[i, j] != 1 && tab[i, j] != 2)
            {
                tab[i, j] = 1;
                Image poussiere = new Image
                {
                    Source = new BitmapImage(new Uri("images/poussiere.png", UriKind.Relative)),
                    Width = 30,
                    Height = 30
                };
                Canvas.SetLeft(poussiere, 15 + i * 60);
                Canvas.SetTop(poussiere, 15 + j * 60);
                lecanvas.Children.Add(poussiere);
            }
        }
    }
}
