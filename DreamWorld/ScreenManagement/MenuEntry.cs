using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement
{
    class MenuEntry
    {
        public string Text;

        float selectionFade;
        public event EventHandler Selected;

        protected internal virtual void OnSelectEntry()
        {
            if (Selected != null)
                Selected(this, null);
        }


        public MenuEntry(string text)
        {
            Text = text;
        }


        public virtual void Update(bool isSelected, GameTime gameTime)
        {
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

            selectionFade = isSelected ? Math.Min(selectionFade + fadeSpeed, 1) : Math.Max(selectionFade - fadeSpeed, 0);
        }

        public virtual void Draw(GameTime gameTime, MenuScreen screen, Vector2 position, bool isSelected)
        {
            Color color = isSelected ? new Color(29, 22, 18, screen.TransitionAlpha) : new Color(83, 71, 65, screen.TransitionAlpha);
            SpriteFont font = screen.Font ?? screen.ScreenManager.Font;
            Vector2 origin = new Vector2(0, 0.5f * font.LineSpacing);
            
            screen.ScreenManager.SpriteBatch.DrawString(font, Text, position + new Vector2(1), Color.Black, 0, origin, 1, SpriteEffects.None, 0);
            screen.ScreenManager.SpriteBatch.DrawString(font, Text, position, color, 0, origin, 1, SpriteEffects.None, 0);            
        }

        public virtual int GetHeight(MenuScreen screen)
        {
            var font = screen.Font ?? screen.ScreenManager.Font;
            return font.LineSpacing;
        }

    }
}
