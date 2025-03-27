using AutoAlignGenerator.ui.controllers;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;

namespace AutoAlignGenerator.ui.graphics.ui
{
    public class ClientPreviewPage : Page
    {
        private FlowPanel controlPanel;
        private AnchorLayout controlPanelLayout;

        private Button settingsButton;
        private Texture settingsIcon;

        private SettingsPage settingsPage;

        private MapLivePreviewPanel map;
        private AnchorLayout mapLayout;

        private AnalysisPanel analysis;
        private AnchorLayout analysisLayout;

        private UIClient client;

        public ClientPreviewPage(Canvas canvas, Camera camera, Navigator navigator, SettingsPage settingsPage, UIClient client) : base(canvas, navigator)
        {
            this.settingsPage = settingsPage;
            this.client = client;

            controlPanel = new FlowPanel(canvas);
            controlPanel.Direction = FlowDirection.Horizontal;
            controlPanel.AlignItems = AlignItems.Middle;
            controlPanel.Gap = 27;
            controlPanelLayout = new AnchorLayout(controlPanel, this);
            controlPanelLayout.Anchor = Anchor.Top | Anchor.Right;
            controlPanelLayout.AllowResize = false;
            controlPanelLayout.Insets = new Insets(0, 39, 51, 0);

            map = new MapLivePreviewPanel(canvas, client);

            mapLayout = new AnchorLayout(map, this);
            mapLayout.Anchor = Anchor.All;
            mapLayout.Insets = new Insets(0, 0, 0, 300);

            analysis = new AnalysisPanel(canvas);
            analysis.Bounds = new RectangleF(0, 0, 0, 300);

            analysisLayout = new AnchorLayout(analysis, this);
            analysisLayout.Anchor = Anchor.Bottom | Anchor.Left | Anchor.Right;
            analysisLayout.Insets = new Insets(0);

            settingsIcon = Scene.AddResource(new Texture("assets/textures/settings.png"));

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
            map.ZIndex = ZIndex;
            analysis.ZIndex = ZIndex + 1;
        }

        public override void Show()
        {
            Scene.Update += new PrioritizedAction<UpdatePriority, double>(UpdatePriority.BeforeGeneral, Scene_Update);

            SubscribeLater(
                controlPanel, controlPanelLayout,
                map, mapLayout,
                analysis, analysisLayout
                );

            Canvas.AddComponent(controlPanel);
            Canvas.AddComponent(settingsButton);
            Canvas.AddComponent(map);
            Canvas.AddComponent(analysis);
        }

        public override void Hide()
        {
            Scene.Update -= Scene_Update;

            Canvas.RemoveComponent(controlPanel);
            Canvas.RemoveComponent(map);
            Canvas.RemoveComponent(analysis);

            UnsubscribeLater(
                controlPanel, controlPanelLayout,
                map, mapLayout,
                analysis, analysisLayout
                );
        }

        private void Scene_Update(double deltaTime)
        {
            
        }
    }
}
