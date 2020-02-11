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
        private static Agent Aspirateur;
        private static TextBlock TextFin;
        private static Button BoutonDeStop;
        private static TextBlock EnergieD;
        private static TextBlock PoussiereA;
        private static TextBlock BijouxA;
        private static TextBlock BijouxR;
        private bool Informe;

        public MainWindow()
        {
            InitializeComponent();
            LeCanvas = Lecanvas;
            TextFin = textDeFin;
            BoutonDeStop = boutonStop;
            EnergieD = EnergieDepensee;
            PoussiereA = PoussieresAspirees;
            BijouxA = BijouxAspires;
            BijouxR = BijouxRamasses;
            Informe = false;
        }

        /// <summary>
        /// Methode qui lance les boucles de l'environnement et de l'aspirateur.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Start(object sender, RoutedEventArgs e)
        {
            Aspirateur.Start();
            ((Button)sender).IsEnabled = false;
        }

        /// <summary>
        /// Methode qui arrete les boucles de l'environnement et de l'aspirateur.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Stop(object sender, RoutedEventArgs e)
        {
            Aspirateur.Stop();
            TextFin.Text = "Fin, arrêt d'urgence";
            TextFin.Visibility = Visibility.Visible;
            BoutonDeStop.IsEnabled = false;
        }

        /// <summary>
        /// Methode qui permet de placer un element sur le canvas (sert pour les graphismes).
        /// Methode statique car appelee depuis des threads differents.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="posLeft"></param>
        /// <param name="posTop"></param>
        public static void PlacerElement(Image image, int posLeft, int posTop)
        {
            Canvas.SetLeft(image, posLeft);
            Canvas.SetTop(image, posTop);
            LeCanvas.Children.Add(image);
        }

        /// <summary>
        /// Methode qui permet d'enlever un element sur le canvas (sert pour les graphismes).
        /// Methode statique car appelee depuis des threads differents.
        /// </summary>
        /// <param name="image"></param>
        public static void EnleverElement(Image image)
        {
            LeCanvas.Children.Remove(image);
        }

        /// <summary>
        /// Methode appelee lorsqu'il n'y a plus de poussieres dans l'environnement.
        /// Methode statique car appelee depuis des threads differents.
        /// </summary>
        public static void FinEnvironnement()
        {
            TextFin.Text = "Fin, l'environnement est propre !";
            TextFin.Visibility = Visibility.Visible;
            BoutonDeStop.IsEnabled = false;
        }

        /// <summary>
        /// Methode appelee quand l'aspirateur a depense toute son energie.
        /// Methode statique car appelee depuis des threads differents.
        /// </summary>
        public static void FinAspi()
        {
            TextFin.Text = "Fin, L'aspirateur n'a plus d'énergie";
            TextFin.Visibility = Visibility.Visible;
            BoutonDeStop.IsEnabled = false;
        }

        /// <summary>
        /// Methode qui ferme les threads a la fermeture de la fenetre.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, System.EventArgs e)
        {
            if (Debut.Visibility == Visibility.Hidden)
                Aspirateur.Stop();
        }

        /// <summary>
        /// Methode qui cree l'agent et l'environnement en fonction des parametres passes sur le premier ecran.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Go(object sender, RoutedEventArgs e)
        {
            Debut.Visibility = Visibility.Hidden;
            Lecanvas.Visibility = Visibility.Visible;
            BoutonsS.Visibility = Visibility.Visible;
            Stats.Visibility = Visibility.Visible;
            Manoir.Visibility = Visibility.Visible;

            Env = new Environnement(5, 5, int.Parse(TpsActualisation.Text), int.Parse(pourcenP.Text), int.Parse(pourcenB.Text));
            Aspirateur = new Agent(int.Parse(EnergieMax.Text), int.Parse(EnergiePAct.Text), int.Parse(TpsAction.Text), Informe, Env);
        }

        /// <summary>
        /// Methode qui permet de set les variables liees a l'agent, comme le nombre de poussiere qu'il a ramasse, ou le nombre d'energie depensee.
        /// Methode statique car appelee depuis des threads differents.
        /// </summary>
        public static void ActualisationStats()
        {
            EnergieD.Text = Aspirateur.EnergieDepensee.ToString();
            PoussiereA.Text = Aspirateur.PoussiereAspiree.ToString();
            BijouxA.Text = Aspirateur.BijouxAspire.ToString();
            BijouxR.Text = Aspirateur.BijouxRamasse.ToString();
        }

        /// <summary>
        /// Methode qui permet de choisir un parcours informe.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Informe = true;
        }

        /// <summary>
        /// Methode qui permet de choisir un parcours non-informe.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Informe = false;
        }
    }
}
