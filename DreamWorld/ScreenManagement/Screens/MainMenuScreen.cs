using System;

namespace DreamWorld.ScreenManagement.Screens
{
    class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen()
            : base("Mainmenu")
        {
            MenuEntry newGameGameMenuEntry = new MenuEntry("New game");
            MenuEntry loadGameGameMenuEntry = new MenuEntry("Load game");
            MenuEntry optionsMenuEntry = new MenuEntry("Settings");
            MenuEntry creditsMenuEntry = new MenuEntry("Credits");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            newGameGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            loadGameGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(newGameGameMenuEntry);
            MenuEntries.Add(loadGameGameMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(creditsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        void PlayGameMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new GameScreen());
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
