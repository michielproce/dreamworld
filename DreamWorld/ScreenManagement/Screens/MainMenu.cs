using System;

namespace DreamWorld.ScreenManagement.Screens
{
    class MainMenu : MenuScreen
    {
        public MainMenu()
            : base("Mainmenu")
        {
            MenuEntry newGameGameMenuEntry = new MenuEntry("New game");
            MenuEntry loadGameGameMenuEntry = new MenuEntry("Load game");
            MenuEntry optionsMenuEntry = new MenuEntry("Settings");
            MenuEntry creditsMenuEntry = new MenuEntry("Credits");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            newGameGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            loadGameGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(newGameGameMenuEntry);
            MenuEntries.Add(loadGameGameMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(creditsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        #region Handle Input

        void PlayGameMenuEntrySelected(object sender, EventArgs e)
        {
            Console.WriteLine("play game");

            Screen gameScreen = new GameScreen();
            
            ScreenManager.AddScreen(gameScreen);
            
            ExitScreen();
        }

        void OptionsMenuEntrySelected(object sender, EventArgs e)
        {
            Console.WriteLine("Options");
            //ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
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


        #endregion
    }
}
