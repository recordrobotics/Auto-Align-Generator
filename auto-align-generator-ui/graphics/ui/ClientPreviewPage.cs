using AutoAlignGenerator.ui.controllers;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;

namespace AutoAlignGenerator.ui.graphics.ui
{
    public class ClientPreviewPage : Page
    {
        private LiveClientPreviewController livePreview;

        private FlowPanel controlPanel;
        private AnchorLayout controlPanelLayout;

        private Button settingsButton;
        private Texture settingsIcon;

        private SettingsPage settingsPage;

        private AnchorLayout mapImageLayout;
        private Image mapImage;

        private FieldLayout robotLayout;
        private Image robotImage;

        private UIClient client;

        public ClientPreviewPage(Canvas canvas, Camera camera, Navigator navigator, SettingsPage settingsPage, UIClient client) : base(canvas, navigator)
        {
            this.settingsPage = settingsPage;
            this.client = client;

            livePreview = new LiveClientPreviewController(client);

            controlPanel = new FlowPanel(canvas);
            controlPanel.Direction = FlowDirection.Horizontal;
            controlPanel.AlignItems = AlignItems.Middle;
            controlPanel.Gap = 27;
            controlPanelLayout = new AnchorLayout(controlPanel, this);
            controlPanelLayout.Anchor = Anchor.Top | Anchor.Right;
            controlPanelLayout.AllowResize = false;
            controlPanelLayout.Insets = new Insets(0, 39, 51, 0);

            settingsIcon = Scene.AddResource(new Texture("assets/textures/settings.png"));

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
            robotImage.Bounds = new RectangleF(100, 100, 200, 200);
            robotImage.ImageType = ImageType.Sliced;
            robotImage.Size = new Size(30, 30);
            robotImage.Color = SolidUIColor.Transparent;
            robotImage.Texture = Texture.RoundedOutline;

            robotLayout = new FieldLayout(robotImage, this);
            robotLayout.ComponentBounds = new RectangleF(robotLayout.FieldBounds.Width / 2, robotLayout.FieldBounds.Height / 2, robotLayout.ComponentBounds.Width, robotLayout.ComponentBounds.Height);

            settingsButton = new Button("Settings", canvas);
            settingsButton.Padding = new Insets(16);
            settingsButton.IsIconButton = true;
            settingsButton.Icon = settingsIcon;
            settingsButton.Click += new PrioritizedAction<GenericPriority>(GenericPriority.Highest, () => Navigator.Push(settingsPage));
            controlPanel.Components.Add(settingsButton);
            settingsButton.SetTooltip("Open settings");

            UpdateZIndex();
        }

        protected override void UpdateZIndex()
        {
            controlPanel.ZIndex = ZIndex + 1;
            settingsButton.ZIndex = ZIndex + 1;
            mapImage.ZIndex = ZIndex;
        }

        public override void Show()
        {
            Scene.Update += new PrioritizedAction<UpdatePriority, double>(UpdatePriority.BeforeGeneral, Scene_Update);

            SubscribeLater(
                mapImage, mapImageLayout,
                robotImage, robotLayout,
                controlPanel, controlPanelLayout,
                livePreview
                );

            Canvas.AddComponent(mapImage);
            Canvas.AddComponent(robotImage);
            Canvas.AddComponent(controlPanel);
            Canvas.AddComponent(settingsButton);
        }

        public override void Hide()
        {
            Scene.Update -= Scene_Update;

            Canvas.RemoveComponent(mapImage);
            Canvas.RemoveComponent(robotImage);
            Canvas.RemoveComponent(controlPanel);
            UnsubscribeLater(
                mapImage, mapImageLayout,
                robotImage, robotLayout,
                controlPanel, controlPanelLayout,
                livePreview
                );
        }

        private void Scene_Update(double deltaTime)
        {
            if(client.Connected)
            {
                robotImage.Color = Theme.Robot;
            } else
            {
                robotImage.Color = SolidUIColor.Transparent;
            }

            if (robotImage.Texture != null)
            {
                float robotFactor = robotImage.Bounds.Width / robotImage.Texture.Width;
                robotImage.Size = new Size((int)(11 * robotFactor), (int)(11 * robotFactor));
            }

            robotLayout.ComponentBounds = new RectangleF(livePreview.Translation.X, livePreview.Translation.Y, robotLayout.ComponentBounds.Width, robotLayout.ComponentBounds.Height);

            robotImage.SetOrigin(new Vector2(0.5f, 0.5f));
            robotImage.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, livePreview.Rotation);
        }
    }
}
