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
        private Environnement Env;
        private static Canvas LeCanvas;
        private static Agent agent;
        private static TextBlock textFin;
        private static Button boutonDeStop;
        private static TextBlock EnergieD;
        private static TextBlock PoussiereA;
        private static TextBlock BijouxA;
        private static TextBlock BijouxR;
        private bool Informe;

        public MainWindow()
        {
            InitializeComponent();
            LeCanvas = Lecanvas;
            textFin = textDeFin;
            boutonDeStop = boutonStop;
            EnergieD = EnergieDepensee;
            PoussiereA = PoussieresAspirees;
            BijouxA = BijouxAspires;
            BijouxR = BijouxRamasses;
            Informe = false;
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            agent.Start();
            ((Button)sender).IsEnabled = false;
        }

        private void Stop(object sender, RoutedEventArgs e)
        {
            agent.Stop();
            textFin.Text = "Fin, arrêt d'urgence";
            textFin.Visibility = Visibility.Visible;
            boutonDeStop.IsEnabled = false;
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

        public static void FinEnvironnement()
        {
            textFin.Text = "Fin, l'environnement est propre !";
            textFin.Visibility = Visibility.Visible;
            boutonDeStop.IsEnabled = false;
        }

        public static void FinAspi()
        {
            textFin.Text = "Fin, L'aspirateur n'a plus d'énergie";
            textFin.Visibility = Visibility.Visible;
            boutonDeStop.IsEnabled = false;
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            if(Debut.Visibility == Visibility.Hidden)
                agent.Stop();
        }

        private void Go(object sender, RoutedEventArgs e)
        {
            Debut.Visibility = Visibility.Hidden;
            Lecanvas.Visibility = Visibility.Visible;
            BoutonsS.Visibility = Visibility.Visible;
            Stats.Visibility = Visibility.Visible;
            Manoir.Visibility = Visibility.Visible;

            Env = new Environnement(5, 5, int.Parse(TpsActualisation.Text), int.Parse(pourcenP.Text) , int.Parse(pourcenB.Text), Lecanvas);
            agent = new Agent( int.Parse(EnergieMax.Text), int.Parse(EnergiePAct.Text), int.Parse(TpsAction.Text), Informe ,Env);
        }

        public static void ActualisationStats()
        {
            EnergieD.Text = agent.EnergieDepensee.ToString();
            PoussiereA.Text = agent.PoussiereAspiree.ToString();
            BijouxA.Text = agent.BijouxAspire.ToString();
            BijouxR.Text = agent.BijouxRamasse.ToString();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Informe = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Informe = false;
        }
    }
}
