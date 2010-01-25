using System;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement
{
    public abstract class IntroScreen : Screen
    {
        private Texture2D texture;

        private TimeSpan duration;
        
        
        protected IntroScreen()
        {
            duration = TimeSpan.FromSeconds(3);
            TransitionOnTime = TimeSpan.FromMilliseconds(500);
            TransitionOffTime = TimeSpan.FromMilliseconds(500);            
        }

        protected override void LoadContent()
        {
            texture = ScreenManager.Game.Content.Load<Texture2D>(@"Textures\Intro\" + TextureName);
        }

        protected abstract string TextureName { get; }


        public override void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime > duration && !IsExiting)
            {
                ScreenManager.AddScreen(new MainMenuScreen());
                ExitScreen();
            }
            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(texture, ScreenManager.Game.GraphicsDevice.Viewport.TitleSafeArea, new Color(255, 255, 255, TransitionAlpha));
            spriteBatch.End();
        }    
    }
}
