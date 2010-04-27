using System;
using DreamWorld.Interface.Help;
using DreamWorld.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement.Screens
{
    public class HelpScreen : Screen
    {
        private const string RETURN_TEXT = "\n\nClick the left mouse button or press B to return.";

        private HelpSystem helpSystem;
        private string text;
        private Vector2 fontPos;
        private SpriteFont font;

        public HelpScreen(HelpSystem helpSystem, string text)
        {
            this.helpSystem = helpSystem;
            this.text = text;

            helpSystem.HintVisible = false;
            IsPopup = true;
            TransitionOnTime = TimeSpan.FromMilliseconds(300);
        }

        protected override void LoadContent()
        {
            font = Content.Load<SpriteFont>(@"Fonts\helptext");
            
            // Cut the lines
            Viewport vp = ScreenManager.GraphicsDevice.Viewport;
            float charWidth = font.MeasureString(text).X / text.Length;;
            float maxWidth = vp.Width - 200;
            int maxChars = (int) Math.Floor(maxWidth / charWidth);
            text = StringUtil.CutLine(text, maxChars);
            
            // Center the text
            Vector2 size = font.MeasureString(text);
            fontPos = new Vector2(vp.Width / 2f - size.X / 2f, vp.Height / 2f - size.Y /2f);
            
            // Add the quit text info
            text += RETURN_TEXT;
        }

        public override void Update(GameTime gameTime)
        {
            if (ScreenManager.InputManager.Player.ApplyRotation)
            {               
                ExitScreen();
                helpSystem.HintVisible = true;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);
            
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;            
            spriteBatch.Begin();
            spriteBatch.DrawString(font, text, fontPos + new Vector2(2), Color.Black);
            spriteBatch.DrawString(font, text, fontPos, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
