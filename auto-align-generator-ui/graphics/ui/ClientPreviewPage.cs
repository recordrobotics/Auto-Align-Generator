using AutoAlignGenerator.ui.controllers;
using System.Drawing;
using System.Numerics;

namespace AutoAlignGenerator.ui.graphics.ui
{
    public class ClientPreviewPage : Page
    {
        //private LiveClientPreviewController livePreview;
        private MapCameraController cameraController;

        private FlowPanel controlPanel;
        private AnchorLayout controlPanelLayout;

        private Button settingsButton;
        private Texture settingsIcon;

        private AnchorLayout dataPanelLayout;

        private SettingsPage settingsPage;

        private Texture mapTexture;
        private Image mapImage;

        private UIClient client;

        public ClientPreviewPage(Canvas canvas, Camera camera, Navigator navigator, SettingsPage settingsPage, UIClient client) : base(canvas, navigator)
        {
            this.settingsPage = settingsPage;
            this.client = client;

            //livePreview = new LiveClientPreviewController(sensor.Transform);

            cameraController = new MapCameraController(camera);
            cameraController.Distance = 6;
            cameraController.Yaw = 45;
            cameraController.Pitch = 25;

            controlPanel = new FlowPanel(canvas);
            controlPanel.Direction = FlowDirection.Horizontal;
            controlPanel.AlignItems = AlignItems.Middle;
            controlPanel.Gap = 27;
            controlPanelLayout = new AnchorLayout(controlPanel, this);
            controlPanelLayout.Anchor = Anchor.Top | Anchor.Right;
            controlPanelLayout.AllowResize = false;
            controlPanelLayout.Insets = new Insets(0, 39, 51, 0);

            settingsIcon = Scene.AddResource(new Texture("assets/textures/settings.png"));

            mapTexture = Scene.AddResource(new Texture("assets/textures/field25-annotated.png")
            {
                WrapMode = TextureWrapMode.Border
            });
            mapTexture.UpdateSettings();

            mapImage = new Image(canvas);
            mapImage.Bounds = new RectangleF(0, 0, 1920, 1080);
            mapImage.Color = SolidUIColor.White;
            mapImage.PreserveAspect = true;
            mapImage.Texture = mapTexture;

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
                cameraController,
                mapImage,
                controlPanel, controlPanelLayout,
                dataPanelLayout
                );

            Canvas.AddComponent(mapImage);
            Canvas.AddComponent(controlPanel);
            Canvas.AddComponent(settingsButton);
        }

        public override void Hide()
        {
            Canvas.RemoveComponent(mapImage);
            Canvas.RemoveComponent(controlPanel);
            UnsubscribeLater(
                cameraController,
                mapImage,
                controlPanel, controlPanelLayout,
                dataPanelLayout
                );
            Scene.Update -= Scene_Update;
        }

        private void Scene_Update(double deltaTime)
        {
            mapImage.Bounds = Bounds;
        }
    }
}
