using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace IA_manoir.modele
{
    class Effecteur
    {
        public delegate void AddAgent(Image agentImage, int x, int y);
        public AddAgent myDelegateAAgent;
        public delegate void RemoveAgent(Image agentImage);
        public RemoveAgent myDelegateRAgent;

        public Effecteur()
        {
            myDelegateAAgent = new AddAgent(MainWindow.PlacerElement);
            myDelegateRAgent = new RemoveAgent(MainWindow.EnleverElement);

    }
    public void Aspirer(Noeud agent)
        {
            Thread.Sleep(1000);
            agent.Contientpoussiere = false;
            if (agent.ContientBijoux)
            {
                agent.ContientBijoux = false;
                Application.Current.Dispatcher.Invoke(this.myDelegateRAgent, new Object[] { agent.Bijoux.Image });
            }
            Application.Current.Dispatcher.Invoke(this.myDelegateRAgent, new Object[] { agent.Poussiere.Image });
            
        }

        public void Ramasser(Noeud agent)
        {
            Thread.Sleep(1000);
            agent.ContientBijoux = false;
            Application.Current.Dispatcher.Invoke(this.myDelegateRAgent, new Object[] { agent.Bijoux.Image });
        }

        public void SeDeplacer(Noeud arr, Noeud agent, Image agentImage)
        {
            Thread.Sleep(1000);
            agent.ContientAspi = false;
            arr.ContientAspi = true;
            Application.Current.Dispatcher.Invoke(this.myDelegateRAgent, new Object[] { agentImage } );
            Application.Current.Dispatcher.Invoke(this.myDelegateAAgent, new Object[] { agentImage, arr.X * 60 + 5, arr.Y * 60 + 5 });
            
        }

    }
}
