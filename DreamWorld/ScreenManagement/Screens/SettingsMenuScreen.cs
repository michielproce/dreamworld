using System;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement.Screens
{
    internal class SettingsMenuScreen : MenuScreen
    {
        private readonly MenuEntry antiAliasingEntry;
        private readonly Config config;
        private readonly MenuEntry exitMenuEntry;

        private readonly MenuEntry fullscreenEntry;
        private readonly MenuEntry resolutionEntry;
        private readonly MenuEntry saveMenuEntry;
        private readonly MenuEntry shadowsEntry;
        private readonly MenuEntry verticalSyncEntry;

        public SettingsMenuScreen()
            : base("Settings")
        {
            config = Config.Load();

            resolutionEntry = new MenuEntry("");
            fullscreenEntry = new MenuEntry("");
            antiAliasingEntry = new MenuEntry("");
            verticalSyncEntry = new MenuEntry("");
            shadowsEntry = new MenuEntry("");
            saveMenuEntry = new MenuEntry("Save");
            exitMenuEntry = new MenuEntry("Cancel");

            resolutionEntry.Selected += ResolutionMenuEntrySelected;
            fullscreenEntry.Selected += FullscreenMenuEntrySelected;
            antiAliasingEntry.Selected += AntiAliasingMenuEntrySelected;
            verticalSyncEntry.Selected += VerticalSyncMenuEntrySelected;
            shadowsEntry.Selected += ShadowMenuEntrySelected;
            saveMenuEntry.Selected += SaveMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(resolutionEntry);
            MenuEntries.Add(fullscreenEntry);
            MenuEntries.Add(antiAliasingEntry);
            MenuEntries.Add(verticalSyncEntry);
            MenuEntries.Add(shadowsEntry);
            MenuEntries.Add(saveMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        public override void Initialize()
        {
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
        }

        private void SetMenuEntryText()
        {
            resolutionEntry.Text = "Resolution: " + config.Width + " x " + config.Height;
            fullscreenEntry.Text = config.Fullscreen ? "Fullscreen" : "Windowed";
            antiAliasingEntry.Text = "Anti-aliasing: " + (config.AntiAliasing ? "On" : "Off");
            verticalSyncEntry.Text = "Vertical synchronization: " + (config.VerticalSync ? "On" : "Off");
            shadowsEntry.Text = "Shadows: " + (config.Shadows ? "On" : "Off");
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

        private void ShadowMenuEntrySelected(object sender, EventArgs e)
        {
            config.Shadows = !config.Shadows;
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
    }
}