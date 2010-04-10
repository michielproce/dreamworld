using System;
using DreamWorld.Levels.PuzzleLevel1;
using DreamWorld.ScreenManagement.Screens.Cutscenes;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement.Screens
{
    class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen()
        {
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

            #if (DEBUG)
//            MenuEntry puzzleLevelMenuEntry = new MenuEntry("PuzzleLevel");
//            puzzleLevelMenuEntry.Selected += LoadGameMenuEntrySelected;
//            MenuEntries.Add(puzzleLevelMenuEntry);
            #endif
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
            ScreenManager.AddScreen(new VillageIntroCutscene());
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
            ScreenManager.Game.Exit();
        }

    }
}
