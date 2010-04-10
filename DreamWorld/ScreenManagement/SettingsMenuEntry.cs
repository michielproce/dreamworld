using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement
{
    class SettingsMenuEntry : MenuEntry
    {
        internal string Value;

        public SettingsMenuEntry(string label, string value) : base(label)
        {
            Value = value;
        }

        public override void Draw(GameTime gameTime, MenuScreen screen, Vector2 position, bool isSelected)
        {
            base.Draw(gameTime, screen, position, isSelected);
            position += new Vector2(0, base.GetHeight(screen) * 0.75f);

            Color color = isSelected ? new Color(153, 102, 204, screen.TransitionAlpha) : new Color(255, 255, 255, screen.TransitionAlpha);
            SpriteFont font = screen.SmallFont ?? screen.Font ?? screen.ScreenManager.Font;
            Vector2 origin = new Vector2(0, 0.5f * font.LineSpacing);

            screen.ScreenManager.SpriteBatch.DrawString(font, Value, position, color, 0, origin, 1, SpriteEffects.None, 0);
        }

        public override int GetHeight(MenuScreen screen)
        {
            SpriteFont font = screen.SmallFont ?? screen.Font ?? screen.ScreenManager.Font;
            return base.GetHeight(screen) + font.LineSpacing;
        }
    }
}
