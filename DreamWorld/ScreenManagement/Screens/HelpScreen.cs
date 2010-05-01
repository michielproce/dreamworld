using System;
using DreamWorld.Interface.Help;
using DreamWorld.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement.Screens
{
    public class HelpScreen : Screen
    {
        

        private HelpSystem helpSystem;
        private string text;
        private Vector2 fontPos;
        private SpriteFont font;

        public HelpScreen(HelpSystem helpSystem, string text)
        {
            this.helpSystem = helpSystem;
            this.text = text;

            helpSystem.ScreenActive = true;
            IsPopup = true;
            TransitionOnTime = TimeSpan.FromMilliseconds(300);
        }

        protected override void LoadContent()
        {
            font = Content.Load<SpriteFont>(@"Fonts\helptext");
            
            // Create paragraphs, not to far apart.
            text = text.Replace("\n", "\n\n");
            font.LineSpacing = 24;

            // Cut the lines
            Viewport vp = ScreenManager.GraphicsDevice.Viewport;
            text = StringUtil.CutLine(vp, font, text, 0.8f);

            // Add the quit text info
            text += StringUtil.ParsePlatform("\n\n\n{Click the left mouse button|Press B} to return.");

            // Center the text
            Vector2 size = font.MeasureString(text);
            fontPos = new Vector2(vp.Width / 2f - size.X / 2f, vp.Height / 2f - size.Y /2f);
        }

        public override void Update(GameTime gameTime)
        {
            if (ScreenManager.InputManager.Player.ApplyRotation)
            {               
                ExitScreen();
                helpSystem.ScreenActive = false;
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
