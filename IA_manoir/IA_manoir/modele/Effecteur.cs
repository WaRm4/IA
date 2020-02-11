using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace IA_manoir.modele
{
    /// <summary>
    /// Effecteur de l'agent qui controle les differentes actions de l'agent.
    /// </summary>
    class Effecteur
    {
        //Delegues pour effectuer des actions sur le thread principal (notamment pour la partie graphique).
        public delegate void AjouterAgent(Image agentImage, int x, int y);
        public AjouterAgent DelegueAjoutAgent;
        public delegate void SupprimerAgent(Image agentImage);
        public SupprimerAgent DelegueSuppressionAgent;
        //*************************************************************************************************

        /// <summary>
        /// Le temps que l'agent mettra pour effectuer une action en millisecondes.
        /// </summary>
        private readonly int TpsAction;

        /// <summary>
        /// Constructeur de l'effecteur.
        /// </summary>
        /// <param name="TpsA"> temps que l'agent mettra pour effectuer une action en millisecondes (Entier). </param>
        public Effecteur(int TpsA)
        {
            DelegueAjoutAgent = new AjouterAgent(MainWindow.PlacerElement);
            DelegueSuppressionAgent = new SupprimerAgent(MainWindow.EnleverElement);
            TpsAction = TpsA;
        }

        /// <summary>
        /// Methode qui permet a l'agent d'aspirer (d'enlever une poussiere).
        /// </summary>
        /// <param name="agent"> Noeud ou se trouve l'agent (Noeud). </param>
        public void Aspirer(Noeud agent)
        {
            Thread.Sleep(TpsAction);
            agent.Contientpoussiere = false;
            if (agent.ContientBijoux)
            {
                agent.ContientBijoux = false;
                Application.Current.Dispatcher.Invoke(this.DelegueSuppressionAgent, new Object[] { agent.Bijoux.Image });
            }
            Application.Current.Dispatcher.Invoke(this.DelegueSuppressionAgent, new Object[] { agent.Poussiere.Image });

        }


        /// <summary>
        /// Methode qui permet a l'agent de ramasser (d'enlever un bijoux sans l'aspirer).
        /// </summary>
        /// <param name="agent"> Noeud ou se trouve l'agent (Noeud). </param>
        public void Ramasser(Noeud agent)
        {
            Thread.Sleep(TpsAction);
            agent.ContientBijoux = false;
            Application.Current.Dispatcher.Invoke(this.DelegueSuppressionAgent, new Object[] { agent.Bijoux.Image });
        }

        /// <summary>
        /// Methode qui permet a l'agent de se deplacer.
        /// </summary>
        /// <param name="arr"> Noeud ou doit se rendre l'agent (Noeud). </param>
        /// <param name="agent"> Noeud ou se trouve l'agent (Noeud). </param>
        /// <param name="agentImage"> Image de l'agent (sert pour la partie graphique) (Image). </param>
        public void SeDeplacer(Noeud arr, Noeud agent, Image agentImage)
        {
            Thread.Sleep(TpsAction);
            agent.ContientAgent = false;
            arr.ContientAgent = true;
            Application.Current.Dispatcher.Invoke(this.DelegueSuppressionAgent, new Object[] { agentImage });
            Application.Current.Dispatcher.Invoke(this.DelegueAjoutAgent, new Object[] { agentImage, arr.X * 60 + 5, arr.Y * 60 + 5 });

        }

    }
}
