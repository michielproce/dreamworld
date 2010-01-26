using System;
using System.Net.Mime;
using DreamWorld.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement.Screens
{
    public class TutorialText
    {       
        private const int MaxChars = 30;
        private const int Padding = 10;
        private Color textureColor = new Color(128, 128, 128, 128);
                
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private Texture2D texture;
        private Vector2 position;        

        private bool visible;
        private string text;
        private TimeSpan end;
        private Rectangle dest;
       
        public TutorialText(SpriteBatch spriteBatch, SpriteFont font, Texture2D texture, Vector2 position)
        {
            this.spriteBatch = spriteBatch;
            this.font = font;
            this.texture = texture;
            this.position = position;
            text = "";
            end = TimeSpan.Zero;
        }

        public void SetText(string text)
        {
            SetText(text, TimeSpan.Zero);
        }
        public void SetText(string text, TimeSpan end)
        {
            this.text = StringUtil.CutLine(text, MaxChars);
            this.end = end;
            visible = true;
            Vector2 size = font.MeasureString(this.text);      
            dest = new Rectangle((int)position.X - Padding, (int)position.Y - Padding, Padding + Padding + (int)size.X, Padding + Padding + (int)size.Y);
        }

        public void Hide()
        {
            visible = false;
        }

        public void Update(GameTime gameTime)
        {
            if(end != TimeSpan.Zero && gameTime.TotalGameTime > end)
                visible = false;
        }

        public void Draw(GameTime gameTime)
        {
            if(!visible)
                return;

            spriteBatch.Begin();
            spriteBatch.Draw(texture, dest, textureColor);
            spriteBatch.DrawString(font, text, position + new Vector2(1), Color.Black); 
            spriteBatch.DrawString(font, text, position, Color.White); 
            spriteBatch.End();
        }
    }
}
