using System;
using System.Threading;
using DreamWorld.Cameras;
using DreamWorld.Levels;
using DreamWorld.Levels.TestLevel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement.Screens
{
    public class GameScreen : Screen
    {
        public Camera CurrentCamera { get; private set; }
        public Level CurrentLevel { get; private set; }
        public Effect DefaultEffect { get; private set; }

        public GameScreen()
        {
            ScreenState = ScreenState.Hidden;
            LoadingScreen = new LoadingScreen(@"Textures/Test/gradient");
        }

        public override void Initialize()
        {
            base.Initialize();

            LoadingScreen.progress = 0.75f;
            CurrentCamera = new DebugCamera
                                {
                                    GraphicsDevice = ScreenManager.Game.GraphicsDevice,
                                    InputManager = ((DreamWorldGame) ScreenManager.Game).InputManager
                                };
            LoadingScreen.progress = 0.8f;
            CurrentCamera.Initialize();

            CurrentLevel = new TestLevel {GameScreen = this};
            LoadingScreen.progress = 0.85f;
            CurrentLevel = new TestLevel { GameScreen = this };
            LoadingScreen.progress = 0.9f;
            CurrentLevel.Initialize();

            
            LoadingScreen.progress = 0.95f;   
        }

        public override void LoadContent()
        {

            DefaultEffect = ScreenManager.Game.Content.Load<Effect>(@"Effects\Default");

            // Placeholder for the actual loading of the content
            for (int i = 0; i <= 7; i++)
            {
                LoadingScreen.progress = (float)i / 10;
                Thread.Sleep(500);
            }

            Initialize();

            LoadingScreen.progress = 1f;
        }

        public override void Update(GameTime gameTime)
        {
            if(ScreenState != ScreenState.Hidden)
            {
                CurrentCamera.Update(gameTime);
                CurrentLevel.Update(gameTime);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);
            CurrentLevel.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
