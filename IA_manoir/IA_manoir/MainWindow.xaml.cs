using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IA_manoir
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Rectangle rect;
            rect = new Rectangle
            {
                Stroke = new SolidColorBrush(Colors.Black),
                Fill = new SolidColorBrush(Colors.Black),
                Width = 50,
                Height = 50
            };
            Canvas.SetLeft(rect, 35);
            Canvas.SetTop(rect, 65);
            lecanvas.Children.Add(rect);

            Image img = new Image
            {
                Source = new BitmapImage(new Uri("aspi.png", UriKind.Relative)),
                Width = 50,
                Height = 50
            };
            Canvas.SetLeft(img, 155);
            Canvas.SetTop(img, 125);
            lecanvas.Children.Add(img);
        }

        private void Poussiere(object sender, RoutedEventArgs e)
        {

        }

        private void Bijoux(object sender, RoutedEventArgs e)
        {

        }

        private void Robot(object sender, RoutedEventArgs e)
        {

        }
    }
}
