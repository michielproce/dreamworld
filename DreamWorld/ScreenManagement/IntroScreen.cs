using System;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement
{
    public abstract class IntroScreen : Screen
    {
        private Rectangle destination;
        private Texture2D texture;

        protected TimeSpan duration;
        
        
        protected IntroScreen()
        {
            duration = TimeSpan.FromSeconds(3);
            TransitionOnTime = TimeSpan.FromMilliseconds(500);
            TransitionOffTime = TimeSpan.FromMilliseconds(500);            
        }

        protected override void LoadContent()
        {
            texture = ScreenManager.Game.Content.Load<Texture2D>(@"Textures\Intro\" + TextureName);
            Viewport vp = ScreenManager.Game.GraphicsDevice.Viewport;
            destination = new Rectangle(0, 0, vp.Width, vp.Height);     
        }

        protected abstract string TextureName { get; }


        public override void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime > duration)
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
            spriteBatch.Draw(texture, destination, new Color(255, 255, 255, TransitionAlpha));
            spriteBatch.End();
        }    
    }
}
