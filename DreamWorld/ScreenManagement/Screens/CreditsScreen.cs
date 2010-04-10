using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement.Screens
{
    public  class CreditsScreen : Screen
    {
        private Rectangle destination;
        private Texture2D texture;

        public CreditsScreen()
        {           
            TransitionOnTime = TimeSpan.FromMilliseconds(500);
            TransitionOffTime = TimeSpan.FromMilliseconds(500);
        }

        protected override void LoadContent()
        {
            texture = ScreenManager.Game.Content.Load<Texture2D>(@"Textures\Menu\credits");
            Viewport vp = ScreenManager.Game.GraphicsDevice.Viewport;
            destination = new Rectangle(0, 0, vp.Width, vp.Height);
        }


        public override void Update(GameTime gameTime)
        {
            if (ScreenManager.InputManager.Menu.Select || ScreenManager.InputManager.Menu.Cancel)
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
