using IA_manoir.modele;
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
        private Environnement environnement;

        public MainWindow()
        {
            InitializeComponent();
            environnement = new Environnement(5,5,lecanvas);

        }

        private void Poussiere(object sender, RoutedEventArgs e)
        {
            environnement.boucle();
        }

        private void Bijoux(object sender, RoutedEventArgs e)
        {
            
        }

        private void Robot(object sender, RoutedEventArgs e)
        {

        }

    }
}
