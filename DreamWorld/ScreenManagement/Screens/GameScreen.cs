using System;
using DreamWorld.Cameras;
using DreamWorld.InputManagement.Types;
using DreamWorld.Levels;
using DreamWorld.Levels.VillageLevel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement.Screens
{
    public class GameScreen : Screen
    {
        public ContentManager Content { get; private set; }
        public Camera CurrentCamera { get; private set; }
        public Level CurrentLevel { get; private set; }

        public GameScreen()
        {
            LoadingScreen = new LoadingScreen(@"Textures/Test/gradient");
            ScreenState = ScreenState.Hidden;
        }

        public override void Initialize()
        {
            Content = new ContentManager(ScreenManager.Game.Services) {RootDirectory = "Content"};

            base.Initialize();

            CurrentLevel = new VillageLevel { GameScreen = this };

            CurrentCamera = new ThirdPersonCamera
            {
                Level = CurrentLevel,
                GraphicsDevice = ScreenManager.Game.GraphicsDevice,
                InputManager = ((DreamWorldGame)ScreenManager.Game).InputManager
            };
            
            CurrentLevel.Initialize();
            CurrentCamera.Initialize();

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
                        if (CurrentCamera is DebugCamera)
                        {
                            CurrentCamera = new ThirdPersonCamera
                            {
                                Level = CurrentLevel,
                                GraphicsDevice = ScreenManager.Game.GraphicsDevice,
                                InputManager = ((DreamWorldGame)ScreenManager.Game).InputManager
                            };
                        }
                        else
                        {
                            CurrentCamera = new DebugCamera
                            {
                                Level = CurrentLevel,
                                GraphicsDevice = ScreenManager.Game.GraphicsDevice,
                                InputManager = ((DreamWorldGame)ScreenManager.Game).InputManager,
                                Position = CurrentCamera.Position
                            };
                        }
                        CurrentCamera.Initialize();
                    }
                #endif
                
                CurrentLevel.Update(gameTime);
                CurrentCamera.Update(gameTime);
                if (CurrentLevel.Skybox != null)
                    CurrentLevel.Skybox.Update(gameTime); // TODO: This updates the skybox second time around, but we don't want the camera delay.
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
            CurrentLevel.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
