using System;
using DreamWorld.Interface.Help;
using DreamWorld.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement.Screens
{
    public class HelpScreen : Screen
    {        
        private string _closeText;
        private Vector2 _closePos;

        private Texture2D _signTexture;
        private Texture2D _glassTexture;
  
        private readonly HelpSystem _helpSystem;
        private string _text;
        private Vector2 _fontPos;
        private SpriteFont _font;

        public HelpScreen(HelpSystem helpSystem, string text)
        {
            _helpSystem = helpSystem;
            _text = text;

            helpSystem.ScreenActive = true;
            IsPopup = true;
            TransitionOnTime = TimeSpan.FromMilliseconds(300);
        }

        protected override void LoadContent()
        {
            Viewport vp = ScreenManager.GraphicsDevice.Viewport;
            _closeText = StringUtil.ParsePlatform("{Click the left mouse button|Press B} to return.");
            Vector2 closeSize = _helpSystem.HintFont.MeasureString(_closeText);
            _closePos = new Vector2(vp.Width/2f - closeSize.X/2f, vp.Height * .925f);

            _signTexture = Content.Load<Texture2D>(@"Textures\Interface\helpscreen");
            _glassTexture = Content.Load<Texture2D>(@"Textures\Interface\helpscreenglass");
            _font = Content.Load<SpriteFont>(@"Fonts\helptext");
            
            // Create paragraphs, not to far apart.
            _text = _text.Replace("\n", "\n\n");
            _font.LineSpacing = 24;

            // Cut the lines            
            _text = StringUtil.CutLine(vp, _font, _text, 0.45f);

            // Center the text
            Vector2 size = _font.MeasureString(_text);
            _fontPos = new Vector2(vp.Width / 2f - size.X / 2f, vp.Height / 2f - size.Y /2f);
        }

        public override void Update(GameTime gameTime)
        {
            if (ScreenManager.InputManager.Player.ApplyRotation)
            {               
                ExitScreen();
                _helpSystem.ScreenActive = false;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            Viewport vp = GameScreen.Instance.GraphicsDevice.Viewport;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;            
            spriteBatch.Begin();
            
            spriteBatch.Draw(_signTexture, new Rectangle(0,0,vp.Width, vp.Height), Color.White);
            
            spriteBatch.DrawString(_font, _text, _fontPos, Color.Black);
            
            spriteBatch.DrawString(_helpSystem.HintFont, _closeText, _closePos + new Vector2(-1, 1), Color.Black);
            spriteBatch.DrawString(_helpSystem.HintFont, _closeText, _closePos, new Color(204, 180, 167));

            spriteBatch.Draw(_glassTexture, new Rectangle(0, 0, vp.Width, vp.Height), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
