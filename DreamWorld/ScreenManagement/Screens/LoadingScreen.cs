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
            get { return _loaded; }
            internal set { _loaded = value; }
        }

        private bool _loaded;

        private Texture2D _loadingTexture;
        private Vector2 _loadingTexturePosition;
        private Color _color;

        public LoadingScreen(Color color)
        {
            _color = color;
            TransitionOffTime = TimeSpan.FromSeconds(0.5f);
            TransitionOnTime = TimeSpan.FromSeconds(0.5f);
        }

        protected override void LoadContent()
        {
            _loadingTexture = ScreenManager.Game.Content.Load<Texture2D>(@"Textures\Menu\loadingMorwir");
            _loadingTexturePosition = new Vector2(ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Right - _loadingTexture.Width, 
                ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Bottom - _loadingTexture.Height) - 
                new Vector2(100, 80);
        }

        public override void HandleInput()
        {
            if (_loaded)
                ExitScreen();
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            string text = (_loaded ? "Loaded!" : "Loading...");

            Vector2 textPosition = new Vector2(80, 60);            

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
           
            spriteBatch.Draw(ScreenManager.BlankTexture, ScreenManager.FullscreenDestination, _color);
            spriteBatch.Draw(_loadingTexture, _loadingTexturePosition, new Color(255, 255, 255, TransitionAlpha));
            spriteBatch.DrawString(font, text, textPosition, new Color(255 - _color.R, 255 - _color.G, 255 - _color.B, TransitionAlpha));

            spriteBatch.End();
        }
    }
}
