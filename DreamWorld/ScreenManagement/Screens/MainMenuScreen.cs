using System;
using DreamWorld.Levels.PuzzleLevel1;
using DreamWorld.ScreenManagement.Screens.Cutscenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace DreamWorld.ScreenManagement.Screens
{
    class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen()
        {
            menuPosition = new Vector2(150, 255);
            entryOffset = new Vector2(0, 26);

            MenuEntry playGameMenuEntry = new MenuEntry("Play game");
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            MenuEntries.Add(playGameMenuEntry);

            #if (!XBOX)
            MenuEntry settingsMenuEntry = new MenuEntry("Settings");
            settingsMenuEntry.Selected += SettingsEntrySelected;
            MenuEntries.Add(settingsMenuEntry);
            #endif

            MenuEntry creditsMenuEntry = new MenuEntry("Credits");
            MenuEntries.Add(creditsMenuEntry);
            creditsMenuEntry.Selected += CreditsEntrySelected;

            MenuEntry quitMenuEntry = new MenuEntry("Quit");
            quitMenuEntry.Selected += OnCancel;
            MenuEntries.Add(quitMenuEntry);
        }

        protected override void LoadContent()
        {
            if(MediaPlayer.State != MediaState.Playing)
                MediaPlayer.Play(Content.Load<Song>(@"Audio\Music\Menu"));

            Background = Content.Load<Texture2D>(@"Textures/Menu/mainMenu");
            Font = Content.Load<SpriteFont>(@"Fonts/mainMenu");
            Font.Spacing *= 0.9f;
            base.LoadContent();
        }

        void PlayGameMenuEntrySelected(object sender, EventArgs e)
        {
            MediaPlayer.Stop();
            ScreenManager.AddScreen(new VillageIntroCutscene());
            ExitScreen();
        }

        void SettingsEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new SettingsMenuScreen());
            ExitScreen();
        }

        void CreditsEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new CreditsScreen());
            ExitScreen();
        }

        protected override void OnCancel()
        {
            const string message = "Are you sure you want to exit dreamworld";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);
            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox);
        }

        void ConfirmExitMessageBoxAccepted(object sender, EventArgs e)
        {
            MediaPlayer.Stop();
            ScreenManager.Game.Exit();
        }

    }
}
