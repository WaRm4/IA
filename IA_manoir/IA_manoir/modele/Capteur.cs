using System.Collections.Generic;

namespace IA_manoir.modele
{
    class Capteur
    {
        public  Environnement Env;

        public Capteur(Environnement e)
        {
            Env = e;
        }

        public List<Noeud> RenvoyerEnvironnement()
        {
            return Env.Carte; 
        }

        public void Start()
        {
            Env.Start();
        }

        public void Stop()
        {
            Env.ArreterBoucle();
        }
    }
}
