using System;
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

        private Texture2D loadingTexture;
        private Vector2 loadingTexturePosition;

        public LoadingScreen()
        {
            TransitionOffTime = TimeSpan.FromSeconds(0.5f);
            TransitionOnTime = TimeSpan.FromSeconds(0.5f);
        }

        protected override void LoadContent()
        {
            loadingTexture = ScreenManager.Game.Content.Load<Texture2D>(@"Textures\Menu\loadingMorwir");
            loadingTexturePosition = new Vector2(ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Right - loadingTexture.Width, 
                ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Bottom - loadingTexture.Height) - 
                new Vector2(100, 80);
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

            Vector2 textPosition = new Vector2(80, 60);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
            
            spriteBatch.Draw(ScreenManager.BlankTexture, ScreenManager.FullscreenDestination, Color.Black);
            spriteBatch.Draw(loadingTexture, loadingTexturePosition, new Color(255, 255, 255, TransitionAlpha));
            spriteBatch.DrawString(font, text, textPosition, new Color(255, 255, 255, TransitionAlpha));

            spriteBatch.End();
        }
    }
}
