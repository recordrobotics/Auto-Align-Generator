using AutoAlignGenerator.ui.graphics;
using AutoAlignGenerator.ui.graphics.ui;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace AutoAlignGenerator.ui.controllers
{
    public class UIClient : Controller
    {
        private readonly Client client;

        public bool Connected => client.IsConnected();
        public uint Team {get => client.Team; set=>client.Team = value; }

        public UIClient()
        {
            client = new Client(SavedResource.Settings.Current.Team);
        }

        public bool Start()
        {
            return Start(null);
        }

        public bool Start(Canvas? alertCanvas)
        {
            client.Start();
            return true;
        }

        public void Stop()
        {
            client.Stop();
        }

        public void Shutdown()
        {
            client.Stop();
        }

        public override void Subscribe()
        {
            Scene.AppExit += new PrioritizedAction<GenericPriority>(GenericPriority.Medium, Scene_AppExit);
            Scene.Update += new PrioritizedAction<UpdatePriority, double>(UpdatePriority.BeforeGeneral, Scene_Update);
        }

        public override void Unsubscribe()
        {
            Scene.AppExit -= Scene_AppExit;
            Scene.Update -= Scene_Update;
        }
        private void Scene_AppExit()
        {
            Shutdown();
        }

        private void Scene_Update(double deltaTime)
        {

        }
    }
}
