using System;
using DreamWorld.InputManagement;
using DreamWorld.InputManagement.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement.Screens
{
    public class LoadingScreen : Screen
    {
        public bool Loaded
        {
            get { return loaded; }
            internal set { loaded = value; }
        }
        public bool loaded;

        private string background;
        private Texture2D backgroundTexture;

        public LoadingScreen(string background)
        {
            TransitionOffTime = TimeSpan.FromSeconds(0.5f);
            TransitionOnTime = TimeSpan.FromSeconds(0.5f);
            this.background = background;
        }

        protected override void LoadContent()
        {
            backgroundTexture = ScreenManager.Game.Content.Load<Texture2D>(background);
        }

        public override void HandleInput()
        {
            MenuInput menuInput = ScreenManager.InputManager.Menu;

            if (loaded 
//                && (menuInput.Select || menuInput.Cancel)
                )
            {
                ExitScreen();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            string text = (loaded ? "Loaded!" : "Loading...");

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            Vector2 textPosition = new Vector2(10, 10);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);

            spriteBatch.Draw(backgroundTexture, fullscreen, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha, TransitionAlpha));
            spriteBatch.DrawString(font, text, textPosition, new Color(0f, 0f, 0f, TransitionAlpha));

            spriteBatch.End();
        }
    }
}
