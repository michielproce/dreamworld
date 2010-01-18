using System;
using DreamWorld.Levels.PuzzleLevel1;
using DreamWorld.Levels.VillageLevel;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement.Screens
{
    class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen()
        {
            MenuEntry playGameMenuEntry = new MenuEntry("Play game");
            MenuEntry settingsMenuEntry = new MenuEntry("Settings");
            MenuEntry creditsMenuEntry = new MenuEntry("Credits");
            MenuEntry quitMenuEntry = new MenuEntry("Quit");
            MenuEntry puzzleLevelMenuEntry = new MenuEntry("PuzzleLevel");

            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            settingsMenuEntry.Selected += SettingsEntrySelected;
            quitMenuEntry.Selected += OnCancel;
            puzzleLevelMenuEntry.Selected += LoadGameMenuEntrySelected;

            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(settingsMenuEntry);
            MenuEntries.Add(creditsMenuEntry);
            MenuEntries.Add(quitMenuEntry);
            MenuEntries.Add(puzzleLevelMenuEntry);
        }

        protected override void LoadContent()
        {
            Background = Content.Load<Texture2D>(@"Textures/Menu/mainMenu");
            Font = Content.Load<SpriteFont>(@"Fonts/mainMenu");
            Font.Spacing *= 0.9f;
            base.LoadContent();
        }

        void PlayGameMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new GameScreen(new VillageLevel()));
            ExitScreen();
        }

        void LoadGameMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new GameScreen(new PuzzleLevel1()));
            ExitScreen();
        }

        void SettingsEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new SettingsMenuScreen());
            ExitScreen();
        }

        protected override void OnCancel()
        {
            const string message = "Are you sure you want to exit dreamworld?";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);
            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox);
        }

        void ConfirmExitMessageBoxAccepted(object sender, EventArgs e)
        {
            ScreenManager.Game.Exit();
        }

    }
}
