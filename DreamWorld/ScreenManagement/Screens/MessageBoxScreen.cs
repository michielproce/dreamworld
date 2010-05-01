using System;
using DreamWorld.InputManagement.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement.Screens
{

    class MessageBoxScreen : Screen
    {
        public string Message;

        private Texture2D backgroundTexture;
        private Texture2D foregroundTexture;

        private float headerAspectRatio;
        private const float bodyToHeaderRatio = 1.1f;
        private const int hPadding = 32;
        private const int vPadding = 16;

        public event EventHandler Accepted;
        public event EventHandler Cancelled;


        public MessageBoxScreen(string message)
            : this(message, true)
        {
        }

        public MessageBoxScreen(string message, bool includeUsageText)
        {
            Message = message;

            const string usageText = "\nA button or Enter to accept" +
                                     "\nB button or Esc to cancel";

            if (includeUsageText)
                Message += usageText;

            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }

        protected override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            backgroundTexture = content.Load<Texture2D>(@"Textures/Menu/confirmBackground");
            foregroundTexture = content.Load<Texture2D>(@"Textures/Menu/confirmForeground");

            headerAspectRatio = (float)backgroundTexture.Width / backgroundTexture.Height;
        }

        public override void HandleInput()
        {
            MenuInput menuInput = ScreenManager.InputManager.Menu;

            if (menuInput.Select)
            {
                if (Accepted != null)
                    Accepted(this, null);

                ExitScreen();
            }
            else if (menuInput.Cancel)
            {
                if (Cancelled != null)
                    Cancelled(this, null);

                ExitScreen();
            }
        }


        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 defaultSize = new Vector2(1280, 720);

            Vector2 textSize = font.MeasureString(Message);
            Vector2 textPosition = (defaultSize - textSize) / 2;

            Rectangle position = new Rectangle((viewport.Width - backgroundTexture.Width) / 2, (viewport.Height - backgroundTexture.Height) / 2, backgroundTexture.Width, backgroundTexture.Height);
            position.Y += 5;
            Color color = new Color(255, 255, 255, TransitionAlpha);

            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState, Matrix.CreateScale((float)viewport.Width / 1280, (float)viewport.Height / 720, 1));

            spriteBatch.Draw(backgroundTexture, position, color);

            spriteBatch.DrawString(font, Message, textPosition, Color.Black);

            spriteBatch.Draw(foregroundTexture, position, color);

            spriteBatch.End();
        }

    }
}
