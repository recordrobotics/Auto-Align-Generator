using AutoAlignGenerator.ui.graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAlignGenerator.ui.controllers
{
    public class LiveClientPreviewController : Controller
    {
        private PointF translation;
        public PointF Translation => translation;

        private float rotation;
        public float Rotation => rotation;

        private UIClient client;

        public LiveClientPreviewController(UIClient client)
        {
            this.client = client;
        }

        public override void Subscribe()
        {
            client.Start();
            Scene.Update += new PrioritizedAction<UpdatePriority, double>(UpdatePriority.BeforeGeneral, Scene_Update);
        }

        public override void Unsubscribe()
        {
            client.Stop();
            Scene.Update -= Scene_Update;
        }

        private void Scene_Update(double deltaTime)
        {
            if (client == null)
                return;
            translation = client.Translation;
            rotation = client.Rotation;
        }
    }
}
