using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement.Screens
{
    internal class SettingsMenuScreen : MenuScreen
    {
        private readonly Config _config;

        private readonly SettingsMenuEntry _resolutionEntry;
        private readonly SettingsMenuEntry _fullscreenEntry;
        private readonly SettingsMenuEntry _antiAliasingEntry;
        private readonly SettingsMenuEntry _verticalSyncEntry;
        private readonly SettingsMenuEntry _subtitlesEntry;
        private readonly SettingsMenuEntry _invertCameraEntry;

        private readonly MenuEntry _saveMenuEntry;
        private readonly MenuEntry _exitMenuEntry;

        public SettingsMenuScreen()
        {
            MenuPosition = new Vector2(150, 215);
            EntryOffset = new Vector2(0, 5);

            _config = Config.Load();

            _resolutionEntry = new SettingsMenuEntry("Resolution", "");
            _fullscreenEntry = new SettingsMenuEntry("Screenmode", "");
            _antiAliasingEntry = new SettingsMenuEntry("Anti-aliasing", "");
            _verticalSyncEntry = new SettingsMenuEntry("Vertical sync", "");
            _subtitlesEntry = new SettingsMenuEntry("Subtitles", "");
            _invertCameraEntry = new SettingsMenuEntry("Invert camera", "");
            _saveMenuEntry = new MenuEntry("Save");
            _exitMenuEntry = new MenuEntry("Cancel");

            _resolutionEntry.Selected += ResolutionMenuEntrySelected;
            _fullscreenEntry.Selected += FullscreenMenuEntrySelected;
            _antiAliasingEntry.Selected += AntiAliasingMenuEntrySelected;
            _verticalSyncEntry.Selected += VerticalSyncMenuEntrySelected;
            _subtitlesEntry.Selected += SubtitlesMenuEntrySelected;
            _invertCameraEntry.Selected += InvertCameraMenuEntrySelected;
            _saveMenuEntry.Selected += SaveMenuEntrySelected;
            _exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(_resolutionEntry);
            MenuEntries.Add(_fullscreenEntry);
            MenuEntries.Add(_antiAliasingEntry);
            MenuEntries.Add(_verticalSyncEntry);
            MenuEntries.Add(_subtitlesEntry);
            MenuEntries.Add(_invertCameraEntry);
            MenuEntries.Add(_saveMenuEntry);
            MenuEntries.Add(_exitMenuEntry);
        }

        public override void Initialize()
        {
            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                if (mode.Width == _config.Width && mode.Height == _config.Height)
                {
                    _config.Width = mode.Width;
                    _config.Height = mode.Height;
                    break;
                }
            }

            SetMenuEntryText();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Background = Content.Load<Texture2D>(@"Textures/Menu/settings");
            Font = Content.Load<SpriteFont>(@"Fonts/bigSettingsMenu");
            SmallFont = Content.Load<SpriteFont>(@"Fonts/smallSettingsMenu");
            SmallFont.Spacing *= 0.9f;
            base.LoadContent();
        }

        private void SetMenuEntryText()
        {
            _resolutionEntry.Value =     _config.Width + " x " + _config.Height;
            _fullscreenEntry.Value =     _config.Fullscreen ? "Fullscreen" : "Windowed";
            _antiAliasingEntry.Value =   _config.AntiAliasing ? "On" : "Off";
            _verticalSyncEntry.Value =   _config.VerticalSync ? "On" : "Off";
            _subtitlesEntry.Value =      _config.Subtitles ? "On" : "Off";
            _invertCameraEntry.Value =   _config.InvertCamera ? "On" : "Off";
        }

        private void ResolutionMenuEntrySelected(object sender, EventArgs e)
        {
            bool found = false;

            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                if (found && (_config.Width != mode.Width || _config.Height != mode.Height))
                {
                    _config.Width = mode.Width;
                    _config.Height = mode.Height;
                    SetMenuEntryText();
                    return;
                }

                if (_config.Width == mode.Width && _config.Height == mode.Height)
                    found = true;
            }

            // Not found? Select the first
            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                _config.Width = mode.Width;
                _config.Height = mode.Height;
                return;
            }
        }

        private void FullscreenMenuEntrySelected(object sender, EventArgs e)
        {
            _config.Fullscreen = !_config.Fullscreen;
            SetMenuEntryText();
        }

        private void AntiAliasingMenuEntrySelected(object sender, EventArgs e)
        {
            _config.AntiAliasing = !_config.AntiAliasing;
            SetMenuEntryText();
        }

        private void VerticalSyncMenuEntrySelected(object sender, EventArgs e)
        {
            _config.VerticalSync = !_config.VerticalSync;
            SetMenuEntryText();
        }

        private void SubtitlesMenuEntrySelected(object sender, EventArgs e)
        {
            _config.Subtitles = !_config.Subtitles;
            SetMenuEntryText();
        }

        private void InvertCameraMenuEntrySelected(object sender, EventArgs e)
        {
            _config.InvertCamera = !_config.InvertCamera;
            SetMenuEntryText();
        }

        private void SaveMenuEntrySelected(object sender, EventArgs e)
        {
            Config.Save(_config);

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

            if (_saveMenuEntry == menuEntry)
                position += new Vector2(0, 10);
            else if (_exitMenuEntry == menuEntry)
                position += new Vector2(160, -EntryOffset.Y - menuEntry.GetHeight(this));

            bool isSelected = IsActive && (key == SelectedEntry);

            menuEntry.Draw(gameTime, this, position, isSelected);

            position.Y += menuEntry.GetHeight(this);
            position += EntryOffset;
        }
    }
}