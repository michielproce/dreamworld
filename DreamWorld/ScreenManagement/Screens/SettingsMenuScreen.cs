using System;
using Microsoft.Xna.Framework;

namespace DreamWorld.ScreenManagement.Screens
{
    public struct Resolution
    {
        public int Width;
        public int Height;
        public bool FullScreen;

        public Resolution(int width, int height, bool fullScreen)
        {
            Width = width;
            Height = height;
            FullScreen = fullScreen;
        }
    }

    class SettingsMenuScreen : MenuScreen
    {
        MenuEntry resolutionEntry;
        MenuEntry exitMenuEntry;

        private Resolution[] Resolutions;
        private int selectedResolution;

        public SettingsMenuScreen()
            : base("Settings")
        {

            resolutionEntry = new MenuEntry("");
            exitMenuEntry = new MenuEntry("Back");

            resolutionEntry.Selected += ResolutionMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(resolutionEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        public override void Initialize()
        {
            Resolutions = new Resolution[]
                              {
                                  new Resolution(800, 600, false), 
                                  new Resolution(800, 600, true), 
                                  new Resolution(1024, 600, true), 
                                  new Resolution(1024, 720, true), 
                                  new Resolution(1280, 720, true), 
                                  new Resolution(1280, 760, true), 
                                  new Resolution(1280, 800, true)
                              };

            GraphicsDeviceManager GraphicsDeviceManager = ((DreamWorldGame)ScreenManager.Game).GraphicsDeviceManager;
            
            for (int i = 0; i < Resolutions.Length; i++ )
            {
                if (Resolutions[i].Width == GraphicsDeviceManager.PreferredBackBufferWidth &&
                    Resolutions[i].Height == GraphicsDeviceManager.PreferredBackBufferHeight &&
                    Resolutions[i].FullScreen == GraphicsDeviceManager.IsFullScreen)
                {
                    selectedResolution = i;
                    break;
                }
            }

            SetMenuEntryText();
        }

        void SetMenuEntryText()
        {
            resolutionEntry.Text = "Resolution: " + (Resolutions[selectedResolution].FullScreen ? "Fullscreen" : "Windowed") +" - "+ Resolutions[selectedResolution].Width + " x " + Resolutions[selectedResolution].Height;
        }

        void ResolutionMenuEntrySelected(object sender, EventArgs e)
        {
            selectedResolution++;
            if (selectedResolution == Resolutions.Length)
            {
                selectedResolution = 0;
            }

            SetMenuEntryText();
        }

        protected override void OnCancel()
        {
             GraphicsDeviceManager graphicsDeviceManager = ((DreamWorldGame)ScreenManager.Game).GraphicsDeviceManager;
             
            if (Resolutions[selectedResolution].Width != graphicsDeviceManager.PreferredBackBufferWidth ||
                Resolutions[selectedResolution].Height != graphicsDeviceManager.PreferredBackBufferHeight ||
                Resolutions[selectedResolution].FullScreen != graphicsDeviceManager.IsFullScreen)
            {
                graphicsDeviceManager.IsFullScreen = Resolutions[selectedResolution].FullScreen;
                graphicsDeviceManager.PreferredBackBufferWidth = Resolutions[selectedResolution].Width;
                graphicsDeviceManager.PreferredBackBufferHeight = Resolutions[selectedResolution].Height;

                graphicsDeviceManager.ApplyChanges();
            }

            ScreenManager.AddScreen(new MainMenuScreen());
            ExitScreen();
        }

    }
}
