using AutoAlignGenerator.ui.controllers;
using Silk.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAlignGenerator.ui.graphics.ui
{
    public class AnalysisPanel : Panel
    {
        private Button recordPositionButton;
        private AnchorLayout recordPositionButtonLayout;

        public AnalysisPanel(Canvas canvas) : base(canvas)
        {
            recordPositionButton = new Button("Record Position", canvas);
            recordPositionButton.Color = Theme.Primary;
            recordPositionButton.FontSize = 15;
            recordPositionButton.AutoSize = Button.AutoSizeMode.TextAndIcon;
            recordPositionButton.Padding = new Insets(20, 17, 35, 17);
            recordPositionButton.Icon = Scene.AddResource(new Texture("assets/textures/add.png"));

            recordPositionButtonLayout = new AnchorLayout(recordPositionButton, this);
            recordPositionButtonLayout.Anchor = Anchor.All;
            recordPositionButtonLayout.AllowResize = false;
            recordPositionButtonLayout.Insets = new Insets(0);
        }

        protected override void UpdateZIndex()
        {
            recordPositionButton.ZIndex = ZIndex + 1;
        }

        public override void Subscribe()
        {
            recordPositionButton.Subscribe();
            recordPositionButtonLayout.Subscribe();

            Canvas.AddComponent(recordPositionButton);
        }

        public override void Unsubscribe()
        {
            recordPositionButton.Unsubscribe();
            recordPositionButtonLayout.Unsubscribe();

            Canvas.RemoveComponent(recordPositionButton);
        }
    }
}
