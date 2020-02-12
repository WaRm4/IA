using System.Collections.Generic;

namespace IA_manoir.modele
{
    /// <summary>
    /// Capteur de l'agent.
    /// </summary>
    class Capteur
    {
        /// <summary>
        /// L'environnement dans lequel agit l'agent.
        /// </summary>
        public  Environnement Env;

        /// <summary>
        /// Constructeur d'un capteur.
        /// </summary>
        /// <param name="e"> L'environnement dans lequel agit l'agent (Environnement). </param>
        public Capteur(Environnement e)
        {
            Env = e;
        }

        /// <summary>
        /// Methode qui permet de capter l'environnement (renvoyer le graphe de l'environnemnt.
        /// </summary>
        /// <returns> L'environnement (List<Noeud>). </returns>
        public List<Noeud> RenvoyerEnvironnement()
        {
            return Env.Carte; 
        }

        /// <summary>
        /// Methode qui permet de recuperer la mesure de performance.
        /// </summary>
        /// <returns> La mesure de performance (Entier) </returns>
        public int RenvoyerMesurePerformance()
        {
            return Env.MesurePerformance;
        }

        /// <summary>
        /// Methode qui permet de demarrer l'environnement. (Presente ici car nous demarrons notre application avec l'agent et 
        /// l'environnement en meme temps bien qu'ils soit sur 2 fils d'action differents).
        /// </summary>
        public void Start()
        {
            Env.Start();
        }

        /// <summary>
        /// Methode qui permet d'arreter l'environnement. (Presente ici car nous demarrons notre application avec l'agent et 
        /// l'environnement en meme temps bien qu'ils soit sur 2 fils d'action differents).
        /// </summary>
        public void Stop()
        {
            Env.ArreterBoucle();
        }
    }
}
