using IA_manoir.modele;
using System.Windows;
using System.Windows.Controls;

namespace IA_manoir
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Environnement Env;

        private static Canvas LeCanvas;

        private Agent agent;

        public MainWindow()
        {
            InitializeComponent();
            LeCanvas = Lecanvas;
            Env = new Environnement(5, 5, Lecanvas);
            agent = new Agent(20, 1, Env);

        }

        private void Poussiere(object sender, RoutedEventArgs e)
        {
            agent.Start();
            ((Button)sender).IsEnabled = false;
        }

        private void Bijoux(object sender, RoutedEventArgs e)
        {
            agent.Stop();
        }

        private void Robot(object sender, RoutedEventArgs e)
        {
        }

        public static void PlacerElement(Image image, int posLeft, int posTop)
        {
            Canvas.SetLeft(image, posLeft);
            Canvas.SetTop(image, posTop);
            LeCanvas.Children.Add(image);
        }

        public static void EnleverElement(Image image)
        {
            LeCanvas.Children.Remove(image);
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            agent.Stop();
        }
    }
}
