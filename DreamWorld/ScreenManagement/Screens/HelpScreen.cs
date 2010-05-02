using System;
using DreamWorld.Interface.Help;
using DreamWorld.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement.Screens
{
    public class HelpScreen : Screen
    {        
        private string closeText;
        private Vector2 closePos;

        private Texture2D signTexture;
        private Texture2D glassTexture;
  
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
            Viewport vp = ScreenManager.GraphicsDevice.Viewport;
            closeText = StringUtil.ParsePlatform("{Click the left mouse button|Press B} to return.");
            Vector2 closeSize = helpSystem.HintFont.MeasureString(closeText);
            closePos = new Vector2(vp.Width/2f - closeSize.X/2f, vp.Height * .925f);

            signTexture = Content.Load<Texture2D>(@"Textures\Interface\helpscreen");
            glassTexture = Content.Load<Texture2D>(@"Textures\Interface\helpscreenglass");
            font = Content.Load<SpriteFont>(@"Fonts\helptext");
            
            // Create paragraphs, not to far apart.
            text = text.Replace("\n", "\n\n");
            font.LineSpacing = 24;

            // Cut the lines            
            text = StringUtil.CutLine(vp, font, text, 0.45f);

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

            Viewport vp = GameScreen.Instance.GraphicsDevice.Viewport;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;            
            spriteBatch.Begin();
            
            spriteBatch.Draw(signTexture, new Rectangle(0,0,vp.Width, vp.Height), Color.White);
            
            spriteBatch.DrawString(font, text, fontPos, Color.Black);
            
            spriteBatch.DrawString(helpSystem.HintFont, closeText, closePos + new Vector2(-1, 1), Color.Black);
            spriteBatch.DrawString(helpSystem.HintFont, closeText, closePos, new Color(204, 180, 167));

            spriteBatch.Draw(glassTexture, new Rectangle(0, 0, vp.Width, vp.Height), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
