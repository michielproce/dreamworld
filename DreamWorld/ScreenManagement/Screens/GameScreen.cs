using System;
using DreamWorld.Cameras;
using DreamWorld.InputManagement;
using DreamWorld.InputManagement.Types;
using DreamWorld.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement.Screens
{
    public class GameScreen : Screen
    {
        public static GameScreen Instance { get; private set; }
        public ContentManager Content { get; private set; }
        public Camera Camera { get; private set; }
        public Level Level { get; private set; }
        public InputManager InputManager { get; private set; }
        public GraphicsDevice GraphicsDevice { get; private set; }

        public GameScreen(Level level)
        {
            Instance = this;            
            LoadingScreen = new LoadingScreen(@"Textures/Test/gradient");
            ScreenState = ScreenState.Hidden;

            Level = level;
            Level.GameScreen = this;
        }

        public override void Initialize()
        {
            Content = new ContentManager(ScreenManager.Game.Services) {RootDirectory = "Content"};
            InputManager = ((DreamWorldGame) ScreenManager.Game).InputManager;
            GraphicsDevice = ScreenManager.Game.GraphicsDevice;

            base.Initialize();

            Camera = new ThirdPersonCamera();
            
            Level.Initialize();
            Camera.Initialize();

            LoadingScreen.Loaded = true;
        }

        public override void UnloadContent()
        {
            Content.Dispose();
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (!OtherScreenHasFocus)
            {
                #if (DEBUG)
                    if (((DreamWorldGame)ScreenManager.Game).InputManager.Debug.ToggleDebugCamera)
                    {
                        if (Camera is DebugCamera)
                            Camera = new ThirdPersonCamera();
                        else
                            Camera = new DebugCamera { Position = Camera.Position};
                        Camera.Initialize();
                    }
                #endif
                
                Level.Update(gameTime);
                Camera.Update(gameTime);
                if (Level.Skybox != null)
                    Level.Skybox.Update(gameTime); // TODO: This updates the skybox second time around, but we don't want the camera delay.
            }
            base.Update(gameTime);
        }

        public override void HandleInput()
        {
            PlayerInput playerInput = ScreenManager.InputManager.Player;
            if(playerInput.ShowPauseMenu)
            {
                const string message = "Are you sure you want to return to the main menu?";

                MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);
                confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

                ScreenManager.AddScreen(confirmExitMessageBox);
            }
        }

        private void ConfirmExitMessageBoxAccepted(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new MainMenuScreen());
            ExitScreen();
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);
            Level.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
