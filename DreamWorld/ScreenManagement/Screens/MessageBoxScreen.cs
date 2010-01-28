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

        private Texture2D headerTexture;
        private Texture2D bodyTexture;

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

            headerTexture = content.Load<Texture2D>(@"Textures/Menu/confirmHeader");
            bodyTexture = content.Load<Texture2D>(@"Textures/Menu/confirmBody");

            headerAspectRatio = (float) headerTexture.Width / headerTexture.Height;
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
            Vector2 defaultSize = new Vector2(1280, 800);

            Vector2 textSize = font.MeasureString(Message);
            Vector2 textPosition = (defaultSize - textSize) / 2;

            Rectangle bodyRectangle = new Rectangle(    (int)textPosition.X - hPadding,
                                                        (int)textPosition.Y - vPadding,
                                                        (int)textSize.X + hPadding * 2,
                                                        (int)textSize.Y + vPadding * 2);

            int headerWidth =  (int) (bodyRectangle.Width * bodyToHeaderRatio);
            int headerHeight = (int) (headerWidth / headerAspectRatio);

            Rectangle headerRectangle = new Rectangle(bodyRectangle.X - (headerWidth - bodyRectangle.Width) / 2, bodyRectangle.Y - headerHeight/3, headerWidth, headerHeight);

            Color color = new Color(255, 255, 255, TransitionAlpha);

            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState, Matrix.CreateScale((float)viewport.Width / 1280, (float)viewport.Height / 800, 1));

            spriteBatch.Draw(headerTexture, headerRectangle, color);
            spriteBatch.Draw(bodyTexture, bodyRectangle, color);

            spriteBatch.DrawString(font, Message, textPosition, color);

            spriteBatch.End();
        }

    }
}
