using System;
using DreamWorld.InputManagement.Types;
using DreamWorld.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement.Screens
{
    class MessageBoxScreen : Screen
    {
        private readonly string _message;

        private Texture2D _backgroundTexture;
        private Texture2D _foregroundTexture;

        public event EventHandler Accepted;
        public event EventHandler Cancelled;


        public MessageBoxScreen(string message)
            : this(message, true)
        {
        }

        public MessageBoxScreen(string message, bool includeUsageText)
        {
            _message = message;

            string usageText = StringUtil.ParsePlatform("\n{Enter | A button} to accept.\n" +
                                                        "{Escape|B button} to cancel.");

            if (includeUsageText)
                _message += usageText;

            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }

        protected override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            _backgroundTexture = content.Load<Texture2D>(@"Textures/Menu/confirmBackground");
            _foregroundTexture = content.Load<Texture2D>(@"Textures/Menu/confirmForeground");
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
            Vector2 vpSize = new Vector2(viewport.Width, viewport.Height);

            Vector2 textSize = font.MeasureString(_message);
            Vector2 textPosition = (vpSize - textSize) / 2;

            Rectangle position = new Rectangle((viewport.Width - _backgroundTexture.Width) / 2, (viewport.Height - _backgroundTexture.Height) / 2, _backgroundTexture.Width, _backgroundTexture.Height);
            position.Y += 5;
            Color color = new Color(255, 255, 255, TransitionAlpha);

            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);

            spriteBatch.Draw(_backgroundTexture, position, color);

            spriteBatch.DrawString(font, _message, textPosition, new Color(0,0,0, TransitionAlpha));

            spriteBatch.Draw(_foregroundTexture, position, color);                       

            spriteBatch.End();
        }

    }
}
