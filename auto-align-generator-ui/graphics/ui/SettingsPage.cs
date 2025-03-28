﻿using AutoAlignGenerator.ui.controllers;
using System.Drawing;

namespace AutoAlignGenerator.ui.graphics.ui
{
    public class SettingsPage : Page
    {
        private Image background;
        private AnchorLayout backgroundAnchor;

        private Panel headerPanel;
        private AnchorLayout headerPanelLayout;

        private Image headerBackground;
        private AnchorLayout headerBackgroundLayout;

        private Label header;
        private AnchorLayout headerLayout;

        private FlowPanel controlPanel;
        private AnchorLayout controlPanelLayout;

        private Button saveButton;
        private Button cancelButton;

        private ScrollPanel settingsListContainer;
        private AnchorLayout settingsListContainerLayout;

        private FlowPanel settingsList;
        private AnchorLayout settingsListLayout;

        private Badge? readOnlyBadge;
        private Tooltip? readOnlyBadgeTooltip;

        public SettingsPage(Canvas canvas, Navigator navigator, UIClient client) : base(canvas, navigator)
        {
            background = new Image(canvas);
            background.Color = Theme.Background;
            backgroundAnchor = new AnchorLayout(background, this);
            backgroundAnchor.Anchor = Anchor.All;
            backgroundAnchor.Insets = new Insets(0);

            headerPanel = new Panel(canvas);
            headerPanel.Bounds = new RectangleF(0, 0, 0, 140);
            headerPanelLayout = new AnchorLayout(headerPanel, this);
            headerPanelLayout.Anchor = Anchor.TopLeft | Anchor.Right;
            headerPanelLayout.Insets = new Insets(0);

            headerBackground = new Image(canvas);
            headerBackground.Color = Theme.Background;
            headerBackgroundLayout = new AnchorLayout(headerBackground, headerPanel);
            headerBackgroundLayout.Anchor = Anchor.All;
            headerBackgroundLayout.Insets = new Insets(0);

            header = new Label("Server Options", canvas);
            header.FontSize = 24.5f;
            header.Font = FontFace.InterSemiBold;
            header.Color = Theme.Header;
            headerLayout = new AnchorLayout(header, headerPanel);
            headerLayout.Anchor = Anchor.TopLeft | Anchor.Bottom;
            headerLayout.AllowResize = false;
            headerLayout.Insets = new Insets(53, 0, 0, 0);

            controlPanel = new FlowPanel(canvas);
            controlPanel.Direction = FlowDirection.Horizontal;
            controlPanel.AlignItems = AlignItems.Middle;
            controlPanel.Gap = 27;
            controlPanelLayout = new AnchorLayout(controlPanel, headerPanel);
            controlPanelLayout.Anchor = Anchor.Top | Anchor.Right | Anchor.Bottom;
            controlPanelLayout.AllowResize = false;
            controlPanelLayout.Insets = new Insets(0, 0, 51, 0);

            if (SavedResource.ReadOnly)
            {
                readOnlyBadge = new Badge("Currently in read-only mode", canvas);
                readOnlyBadge.Color = Theme.Warning;
                readOnlyBadge.Padding = new Insets(30, 14, 30, 14);
                readOnlyBadge.FontSize = 14;
                controlPanel.Components.Add(readOnlyBadge);
                readOnlyBadgeTooltip = new Tooltip("These settings are loaded in read-only mode.", "Read-only mode is enabled when there is\nno write access to the settings directory.", readOnlyBadge, canvas);
                readOnlyBadgeTooltip.Anchor = PopupAnchor.Left;
            }

            saveButton = new Button("Save", canvas);
            saveButton.Color = Theme.Primary;
            saveButton.FontSize = 15;
            saveButton.RenderOffset = new System.Numerics.Vector2(0, 2);
            saveButton.AutoSize = Button.AutoSizeMode.TextAndIcon;
            saveButton.Padding = new Insets(35, 17, 35, 17);
            controlPanel.Components.Add(saveButton);
            saveButton.Click += new PrioritizedAction<GenericPriority>(GenericPriority.Highest, () =>
            {
                var newSettings = new SavedResource.Settings();

                if (uint.TryParse(serverPort?.Text.ToString() ?? "6731", out var p))
                {
                    newSettings.Team = p;
                } else
                {
                    newSettings.Team = SavedResource.Settings.Current.Team;
                }


                newSettings.Theme = theme?.Text.ToString();

                if (SavedResource.WriteSettings(newSettings))
                {
                    bool serverChanged = !SavedResource.Settings.Current.Team.Equals(newSettings.Team);

                    SavedResource.Settings.SetSettings(newSettings);

                    Theme.UpdateTheme();

                    Alert.CreateOneShot("Settings successfully saved!", "File written to '" + Path.Join(SavedResource.SavePath, "settings.json") + "'.", canvas).Color = Theme.Success;

                    if (serverChanged)
                    {
                        client.Team = newSettings.Team;
                    }
                } else if(!SavedResource.ReadOnly)
                {
                    Alert.CreateOneShot("Could not save settings!", "An error occurred while trying to write to\n'" + Path.Join(SavedResource.SavePath, "settings.json") + "'.", canvas).Color = Theme.Error;
                }
                else
                {
                    Alert.CreateOneShot("Settings loaded in read-only mode!", "An error occurred while trying to write to\n'" + Path.Join(SavedResource.SavePath, "settings.json") + "'.", canvas).Color = Theme.Warning;
                }
            });
            saveButton.SetTooltip("Save changes");

            cancelButton = new Button("Close", canvas);
            cancelButton.FontSize = 15;
            cancelButton.RenderOffset = new System.Numerics.Vector2(0, 2);
            cancelButton.AutoSize = Button.AutoSizeMode.TextAndIcon;
            cancelButton.Padding = new Insets(35, 17, 35, 17);
            cancelButton.Click += new PrioritizedAction<GenericPriority>(GenericPriority.Highest, Navigator.Back);
            controlPanel.Components.Add(cancelButton);
            cancelButton.SetTooltip("Close Settings");

            settingsList = new FlowPanel(canvas);
            settingsList.AutoSize = FlowLayout.AutoSize.Y;
            settingsList.Direction = FlowDirection.Vertical;
            settingsList.Padding = new Insets(0, 20, 0, 40);
            settingsList.Gap = 60;

            settingsListContainer = new ScrollPanel(canvas, settingsList);
            settingsListContainerLayout = new AnchorLayout(settingsListContainer, this);
            settingsListContainerLayout.Anchor = Anchor.All;
            settingsListContainerLayout.Insets = new Insets(52.5f, 140, 52.5f, 0);

            settingsListLayout = new AnchorLayout(settingsList, settingsListContainer.VirtualWorkingRectangle);
            settingsListLayout.Anchor = Anchor.Left | Anchor.Right;
            settingsListLayout.Insets = new Insets(0);

            UpdateZIndex();
        }

        protected override void UpdateZIndex()
        {
            background.ZIndex = ZIndex;
            headerBackground.ZIndex = ZIndex + 8;
            header.ZIndex = ZIndex + 9;
            controlPanel.ZIndex = ZIndex + 9;
            if(readOnlyBadge != null)
                readOnlyBadge.ZIndex = ZIndex + 9;
            saveButton.ZIndex = ZIndex + 9;
            cancelButton.ZIndex = ZIndex + 9;
            settingsListContainer.ZIndex = ZIndex + 2;

            foreach (var component in settingsList.Components)
            {
                component.ZIndex = ZIndex + 3;
            }
        }

        class InputEntry : FlowPanel
        {
            private readonly Label header;
            private readonly Label text;
            private readonly InputField input;
            private readonly FlowPanel namePanel;

            public InputField InputField => input;

            public InputEntry(string header, string text, string value, Canvas canvas) : base(canvas)
            {
                Direction = FlowDirection.Horizontal;
                AlignItems = AlignItems.Middle;
                Gap = 40;

                namePanel = new FlowPanel(canvas);
                namePanel.Direction = FlowDirection.Vertical;
                namePanel.Gap = 20;

                this.header = new Label(header, canvas);
                this.header.FontSize = 18;
                this.header.Color = Theme.Header;

                this.text = new Label(text, canvas);
                this.text.FontSize = 13;
                this.text.Color = Theme.Text;

                namePanel.Components.Add(this.header);
                namePanel.Components.Add(this.text);

                input = new InputField(value, canvas);
                input.FontSize = 20;

                Components.Add(namePanel);
                Components.Add(input);
            }

            protected override void UpdateZIndex()
            {
                base.UpdateZIndex();
                header.ZIndex = ZIndex;
                text.ZIndex = ZIndex;
                namePanel.ZIndex = ZIndex;
                input.ZIndex = ZIndex;
            }

            public override void OnAdd()
            {
                base.OnAdd();
                Canvas.AddComponent(header);
                Canvas.AddComponent(text);
                Canvas.AddComponent(namePanel);
                Canvas.AddComponent(input);
            }
        }

        private InputField AddInput(string header, string text, string value)
        {
            var entry = new InputEntry(header, text, value, Canvas);
            settingsList.Components.Add(entry);
            Canvas.AddComponent(entry);
            return entry.InputField;
        }

        private InputField? serverPort;
        private InputField? theme;

        public override void Show()
        {
            SubscribeLater(
                background, backgroundAnchor,
                headerPanel, headerPanelLayout,
                header, headerLayout,
                headerBackground, headerBackgroundLayout,
                controlPanel, controlPanelLayout,
                settingsListContainer, settingsListContainerLayout,
                settingsList, settingsListLayout,
                readOnlyBadgeTooltip
                );

            Canvas.AddComponent(background);
            Canvas.AddComponent(headerPanel);
            Canvas.AddComponent(headerBackground);
            Canvas.AddComponent(header);
            Canvas.AddComponent(controlPanel);
            if(readOnlyBadge != null)
                Canvas.AddComponent(readOnlyBadge);
            Canvas.AddComponent(saveButton);
            Canvas.AddComponent(cancelButton);
            Canvas.AddComponent(settingsListContainer);
            Canvas.AddComponent(settingsList);

            settingsList.Components.Clear();

            serverPort = AddInput("Team Number",
                "This is the team number used to get the IP address of the server.\n" +
                "Make sure to set this to the correct team number.", SavedResource.Settings.Current.Team.ToString());
            serverPort.AutoSize = false;
            serverPort.MaxLength = 7;
            serverPort.Filter = char.IsAsciiDigit;
            serverPort.Bounds = serverPort.GetAutoSizeBounds('0', 7, out _);

            theme = AddInput("Theme file",
                "Name of the theme file (json).\n\n" +
                "The built-in themes are\n" +
                "    - dark.json\n" +
                "    - light.json\n" +
                "\n" +
                "To use custom themes, put the .json file in\n'"
                +(Path.Join(SavedResource.SavePath, "themes")??"<NOT SUPPORTED>")+"'\n" +
                "and specify just the filename here.\n" +
                "Note: custom themes overwrite built-in ones.", SavedResource.Settings.Current.Theme ?? "<UNKNOWN>");

            UpdateZIndex();
        }

        public override void Hide()
        {
            Canvas.RemoveComponent(background);
            Canvas.RemoveComponent(headerPanel);
            Canvas.RemoveComponent(headerBackground);
            Canvas.RemoveComponent(header);
            Canvas.RemoveComponent(controlPanel);
            Canvas.RemoveComponent(settingsList);
            Canvas.RemoveComponent(settingsListContainer);

            UnsubscribeLater(
                background, backgroundAnchor,
                headerPanel, headerPanelLayout,
                headerBackground, headerBackgroundLayout,
                header, headerLayout,
                controlPanel, controlPanelLayout,
                settingsListContainer, settingsListContainerLayout,
                settingsList, settingsListLayout,
                readOnlyBadgeTooltip
                );
        }
    }
}
