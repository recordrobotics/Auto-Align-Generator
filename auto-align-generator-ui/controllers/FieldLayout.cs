using AutoAlignGenerator.ui.graphics.ui;
using AutoAlignGenerator.ui.graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace AutoAlignGenerator.ui.controllers
{
    public class FieldLayout : Controller
    {
        public UIController Component { get; }

        /// <summary>
        /// The container to which to align to.
        /// Can be set to null to align to the frame buffer
        /// </summary>
        public UIController? Container { get; set; }

        public RectangleF FieldBounds { get; set; } = new RectangleF(0, 0, 17.548f, 8.052f);
        public RectangleF ComponentBounds { get; set; } = new RectangleF(0, 0, 0.94f, 0.94f);

        public FieldLayout(UIController component) : this(component, null)
        { }

        public FieldLayout(UIController component, UIController? container)
        {
            Component = component;
            Container = container;
        }

        public override void Subscribe()
        {
            Scene.Update += new PrioritizedAction<UpdatePriority, double>(UpdatePriority.BeforeGeneral, Scene_Update);
        }

        public override void Unsubscribe()
        {
            Scene.Update -= Scene_Update;
        }

        private void Scene_Update(double deltaTime)
        {
            RectangleF bounds = Component.Bounds;
            RectangleF container = Container?.Bounds ??
                new RectangleF(0, 0, Window.Current.FramebufferSize.X, Window.Current.FramebufferSize.Y);
            RectangleF window = container;

            // Update container bounds to have aspect ratio same as field bounds and centered
            float containerAspect = container.Width / container.Height;
            float fieldAspect = FieldBounds.Width / FieldBounds.Height;

            if (containerAspect < fieldAspect)
            {
                container.Height = container.Width / fieldAspect;
                container.Y = container.Top + (window.Height - container.Height) / 2;
            }
            else
            {
                container.Width = container.Height * fieldAspect;
                container.X = container.Left + (window.Width - container.Width) / 2;
            }

            bounds.X = container.Left + (ComponentBounds.X - FieldBounds.Left) / FieldBounds.Width * container.Width - bounds.Width / 2;
            bounds.Y = container.Top + (1 - (ComponentBounds.Y - FieldBounds.Top) / FieldBounds.Height) * container.Height - bounds.Height / 2;
            bounds.Width = ComponentBounds.Width / FieldBounds.Width * container.Width;
            bounds.Height = ComponentBounds.Height / FieldBounds.Height * container.Height;

            Component.Bounds = bounds;
        }
    }
}
