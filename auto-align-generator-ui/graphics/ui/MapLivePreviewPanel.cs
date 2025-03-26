using AutoAlignGenerator.ui.controllers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AutoAlignGenerator.ui.graphics.ui
{
    public class MapLivePreviewPanel : Panel
    {
        private LiveClientPreviewController livePreview;

        private AnchorLayout mapImageLayout;
        private Image mapImage;

        private FieldLayout robotLayout;
        private Image robotImage;
        private AnchorLayout robotHeadingLayout;
        private Image robotHeadingImage;

        private UIClient client;

        public MapLivePreviewPanel(Canvas canvas, UIClient client) : base(canvas)
        {
            this.client = client;

            livePreview = new LiveClientPreviewController(client);

            var mapTexture = Scene.AddResource(new Texture("assets/textures/field25-annotated.png")
            {
                WrapMode = TextureWrapMode.Border
            });
            mapTexture.UpdateSettings();

            mapImage = new Image(canvas);
            mapImage.Bounds = new RectangleF(0, 0, 1920, 1080);
            mapImage.Color = SolidUIColor.White;
            mapImage.PreserveAspect = Image.PreserveAspectMode.Fit;
            mapImage.Texture = mapTexture;

            mapImageLayout = new AnchorLayout(mapImage, this);
            mapImageLayout.Anchor = Anchor.All;
            mapImageLayout.Insets = new Insets(0);

            robotImage = new Image(canvas);
            robotImage.Bounds = new RectangleF(0, 0, 200, 200);
            robotImage.ImageType = ImageType.Sliced;
            robotImage.Size = new Size(30, 30);
            robotImage.Color = SolidUIColor.Transparent;
            robotImage.Texture = Texture.RoundedOutline;

            robotHeadingImage = new Image(canvas);
            robotHeadingImage.Bounds = new RectangleF(0, 0, 25, 25);
            robotHeadingImage.Color = SolidUIColor.Transparent;
            robotHeadingImage.Texture = Texture.Pill;
            robotHeadingImage.Transform = robotImage.Transform;

            robotLayout = new FieldLayout(robotImage, this);
            robotLayout.ComponentBounds = new RectangleF(robotLayout.FieldBounds.Width / 2, robotLayout.FieldBounds.Height / 2, robotLayout.ComponentBounds.Width, robotLayout.ComponentBounds.Height);

            robotHeadingLayout = new AnchorLayout(robotHeadingImage, robotImage);
            robotHeadingLayout.Anchor = Anchor.Right | Anchor.Top | Anchor.Bottom;
            robotHeadingLayout.AllowResize = false;
            robotHeadingLayout.Insets = new Insets(0, 0, -robotHeadingImage.Bounds.Width / 2, 0);
        }

        protected override void UpdateZIndex()
        {
            mapImage.ZIndex = ZIndex;
        }

        public override void Subscribe()
        {
            Scene.Update += new PrioritizedAction<UpdatePriority, double>(UpdatePriority.BeforeGeneral, Scene_Update);

            mapImage.Subscribe(); mapImageLayout.Subscribe();
            robotImage.Subscribe(); robotLayout.Subscribe();
            robotHeadingImage.Subscribe(); robotHeadingLayout.Subscribe();
            livePreview.Subscribe();

            Canvas.AddComponent(mapImage);
            Canvas.AddComponent(robotImage);
            Canvas.AddComponent(robotHeadingImage);
        }

        public override void Unsubscribe()
        {
            Scene.Update -= Scene_Update;
            mapImage.Unsubscribe(); mapImageLayout.Unsubscribe();
            robotImage.Unsubscribe(); robotLayout.Unsubscribe();
            robotHeadingImage.Unsubscribe(); robotHeadingLayout.Unsubscribe();
            livePreview.Unsubscribe();
            Canvas.RemoveComponent(mapImage);
            Canvas.RemoveComponent(robotImage);
            Canvas.RemoveComponent(robotHeadingImage);
        }

        private void Scene_Update(double deltaTime)
        {
            if (client.Connected)
            {
                robotImage.Color = Theme.Robot;
                robotHeadingImage.Color = Theme.RobotHeading;
            }
            else
            {
                robotImage.Color = SolidUIColor.Transparent;
                robotHeadingImage.Color = SolidUIColor.Transparent;
            }

            if (robotImage.Texture != null)
            {
                float robotFactor = robotImage.Bounds.Width / robotImage.Texture.Width;
                robotImage.Size = new Size((int)(11 * robotFactor), (int)(11 * robotFactor));
                robotHeadingImage.Bounds = new RectangleF(0, 0, 15 * robotFactor, 15 * robotFactor);
                robotHeadingLayout.Insets = new Insets(0, 0, -robotHeadingImage.Bounds.Width / 2 + 2 * robotFactor, 0);
            }

            robotLayout.ComponentBounds = new RectangleF(livePreview.Translation.X, livePreview.Translation.Y, robotLayout.ComponentBounds.Width, robotLayout.ComponentBounds.Height);

            robotImage.SetOrigin(new Vector2(0.5f, 0.5f));
            robotImage.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, -livePreview.Rotation);
        }
    }
}
