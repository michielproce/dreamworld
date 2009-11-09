using System;
using DreamWorld.InputManagement;
using DreamWorld.InputManagement.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement.Screens
{
    public class LoadingScreen : Screen
    {
        public float progress;
        private string background;
        private Texture2D backgroundTexture;
        private Texture2D progressTexture;

        public LoadingScreen(string background)
        {
            TransitionOffTime = TimeSpan.FromSeconds(0.5f);
            TransitionOnTime = TimeSpan.FromSeconds(0.5f);
            this.background = background;
        }

        public override void LoadContent()
        {
            backgroundTexture = ScreenManager.Game.Content.Load<Texture2D>(background);
            progressTexture = ScreenManager.Game.Content.Load<Texture2D>(@"Textures/Test/gradient");
        }

        public override void HandleInput(Input input)
        {
            if (progress == 1f)
            {
                MenuInput menuInput = (MenuInput)input;

                if (menuInput.Select)
                {
                    ExitScreen();
                }
            }
        }
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            string text = (progress == 1f ? "Loaded!" : "Loading...");

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            Rectangle progressPosition = new Rectangle(0, (int)(viewport.Height * 0.9f), (int)(viewport.Width * progress), (int)(viewport.Height * 0.1f));
            Vector2 textPosition = new Vector2(10, 10);

            spriteBatch.Begin();

            spriteBatch.Draw(backgroundTexture, fullscreen, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha, TransitionAlpha));
            spriteBatch.Draw(progressTexture, progressPosition, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha, TransitionAlpha));
            spriteBatch.DrawString(font, text, textPosition, new Color(0f, 0f, 0f, TransitionAlpha));

            spriteBatch.End();
        }
    }
}
