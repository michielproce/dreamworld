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
            MenuEntry settingsMenuEntry = new MenuEntry("Settings");
            MenuEntry creditsMenuEntry = new MenuEntry("Credits");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            newGameGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            loadGameGameMenuEntry.Selected += LoadGameMenuEntrySelected;
            settingsMenuEntry.Selected += SettingsEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(newGameGameMenuEntry);
            MenuEntries.Add(loadGameGameMenuEntry);
            MenuEntries.Add(settingsMenuEntry);
            MenuEntries.Add(creditsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        void PlayGameMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new GameScreen());
            ExitScreen();
        }

        void LoadGameMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new GameScreen());
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