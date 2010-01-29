using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement.Screens
{
    internal class SettingsMenuScreen : MenuScreen
    {
        private readonly Config config;

        private readonly SettingsMenuEntry resolutionEntry;
        private readonly SettingsMenuEntry fullscreenEntry;
        private readonly SettingsMenuEntry antiAliasingEntry;
        private readonly SettingsMenuEntry verticalSyncEntry;
        private readonly SettingsMenuEntry subtitlesEntry;

        private readonly MenuEntry saveMenuEntry;
        private readonly MenuEntry exitMenuEntry;

        public SettingsMenuScreen()
        {
            config = Config.Load();

            resolutionEntry = new SettingsMenuEntry("Resolution", "");
            fullscreenEntry = new SettingsMenuEntry("Screenmode", "");
            antiAliasingEntry = new SettingsMenuEntry("Anti-aliasing", "");
            verticalSyncEntry = new SettingsMenuEntry("Vertical synchronization", "");
            subtitlesEntry = new SettingsMenuEntry("Subtitles", "");
            saveMenuEntry = new MenuEntry("Save");
            exitMenuEntry = new MenuEntry("Cancel");

            resolutionEntry.Selected += ResolutionMenuEntrySelected;
            fullscreenEntry.Selected += FullscreenMenuEntrySelected;
            antiAliasingEntry.Selected += AntiAliasingMenuEntrySelected;
            verticalSyncEntry.Selected += VerticalSyncMenuEntrySelected;
            subtitlesEntry.Selected += SubtitlesMenuEntrySelected;
            saveMenuEntry.Selected += SaveMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(resolutionEntry);
            MenuEntries.Add(fullscreenEntry);
            MenuEntries.Add(antiAliasingEntry);
            MenuEntries.Add(verticalSyncEntry);
            MenuEntries.Add(subtitlesEntry);
            MenuEntries.Add(saveMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        public override void Initialize()
        {
            menuPosition = new Vector2(100, 240);

            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                if (mode.Width == config.Width && mode.Height == config.Height)
                {
                    config.Width = mode.Width;
                    config.Height = mode.Height;
                    break;
                }
            }

            SetMenuEntryText();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Background = Content.Load<Texture2D>(@"Textures/Menu/settings");
            SmallFont = Content.Load<SpriteFont>(@"Fonts/smallSettingsMenu");
            SmallFont.Spacing *= 0.9f;
            base.LoadContent();
        }

        private void SetMenuEntryText()
        {
            resolutionEntry.Value =     config.Width + " x " + config.Height;
            fullscreenEntry.Value =     config.Fullscreen ? "Fullscreen" : "Windowed";
            antiAliasingEntry.Value =   config.AntiAliasing ? "On" : "Off";
            verticalSyncEntry.Value =   config.VerticalSync ? "On" : "Off";
            subtitlesEntry.Value =      config.Subtitles ? "On" : "Off";
        }

        private void ResolutionMenuEntrySelected(object sender, EventArgs e)
        {
            bool found = false;

            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                if (found && (config.Width != mode.Width || config.Height != mode.Height))
                {
                    config.Width = mode.Width;
                    config.Height = mode.Height;
                    SetMenuEntryText();
                    return;
                }

                if (config.Width == mode.Width && config.Height == mode.Height)
                    found = true;
            }

            // Not found? Select the first
            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                config.Width = mode.Width;
                config.Height = mode.Height;
                return;
            }
        }

        private void FullscreenMenuEntrySelected(object sender, EventArgs e)
        {
            config.Fullscreen = !config.Fullscreen;
            SetMenuEntryText();
        }

        private void AntiAliasingMenuEntrySelected(object sender, EventArgs e)
        {
            config.AntiAliasing = !config.AntiAliasing;
            SetMenuEntryText();
        }

        private void VerticalSyncMenuEntrySelected(object sender, EventArgs e)
        {
            config.VerticalSync = !config.VerticalSync;
            SetMenuEntryText();
        }

        private void SubtitlesMenuEntrySelected(object sender, EventArgs e)
        {
            config.Subtitles = !config.Subtitles;
            SetMenuEntryText();
        }

        private void SaveMenuEntrySelected(object sender, EventArgs e)
        {
            Config.Save(config);

            ((DreamWorldGame) ScreenManager.Game).ApplyConfig();

            Exit();
        }

        protected override void OnCancel()
        {
            Exit();
        }

        private void Exit()
        {
            ScreenManager.AddScreen(new MainMenuScreen());
            ExitScreen();
        }

        protected override void DrawMenuEntry(GameTime gameTime, int key, ref Vector2 position)
        {
            MenuEntry menuEntry = MenuEntries[key];

            if (saveMenuEntry == menuEntry)
                position += new Vector2(0, 50);

            bool isSelected = IsActive && (key == selectedEntry);

            menuEntry.Draw(gameTime, this, position, isSelected);

            position.Y += menuEntry.GetHeight(this);
        }
    }
}