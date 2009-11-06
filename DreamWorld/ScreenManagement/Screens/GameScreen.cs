using System;
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
        }

        public override void Initialize()
        {
            base.Initialize();

            CurrentCamera = new DebugCamera
                                {
                                    GraphicsDevice = ScreenManager.Game.GraphicsDevice,
                                    InputManager = ((DreamWorldGame) ScreenManager.Game).InputManager
                                };
            CurrentCamera.Initialize();

            CurrentLevel = new TestLevel {GameScreen = this};
            CurrentLevel.Initialize();

            
        }

        protected override void LoadContent()
        {
            DefaultEffect = ScreenManager.Game.Content.Load<Effect>(@"Effects\Default");
        }

        public override void Update(GameTime gameTime)
        {
            CurrentCamera.Update(gameTime);
            CurrentLevel.Update(gameTime);
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
