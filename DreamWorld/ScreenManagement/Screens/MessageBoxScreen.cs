using System;
using DreamWorld.InputManagement.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement.Screens
{

    class MessageBoxScreen : Screen
    {
        string message;
        Texture2D background;

        public event EventHandler Accepted;
        public event EventHandler Cancelled;


        public MessageBoxScreen(string message)
            : this(message, true)
        {
        }

        public MessageBoxScreen(string message, bool includeUsageText)
        {
            const string usageText = "\nA button or Enter to accept" +
                                     "\nB button or Esc to cancel";

            if (includeUsageText)
                this.message = message + usageText;
            else
                this.message = message;

            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }

        protected override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            background = content.Load<Texture2D>(@"Textures/Test/gradient");
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

            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 defaultSize = new Vector2(1280, 800);
            Vector2 textSize = font.MeasureString(message);
            Vector2 textPosition = (defaultSize - textSize) / 2;

            const int hPad = 32;
            const int vPad = 16;

            Rectangle backgroundRectangle = new Rectangle((int)textPosition.X - hPad,
                                                          (int)textPosition.Y - vPad,
                                                          (int)textSize.X + hPad * 2,
                                                          (int)textSize.Y + vPad * 2);

            Color color = new Color(255, 255, 255, TransitionAlpha);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState, Matrix.CreateScale((float)viewport.Width / 1280, (float)viewport.Height / 800, 1));

            spriteBatch.Draw(background, backgroundRectangle, color);

            spriteBatch.DrawString(font, message, textPosition, color);

            spriteBatch.End();
        }

    }
}
