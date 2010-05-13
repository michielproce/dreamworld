using System;
using System.Collections.Generic;
using DreamWorld.InputManagement.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement
{
    abstract class MenuScreen : Screen
    {
        protected Texture2D Background;
        public SpriteFont Font;
        public SpriteFont SmallFont;

        protected List<MenuEntry> MenuEntries { get; private set; }
        internal int SelectedEntry;
        protected Vector2 MenuPosition = new Vector2(100, 250);
        protected Vector2 EntryOffset;

        protected MenuScreen()
        {
            MenuEntries = new List<MenuEntry>();
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        private void OnSelectEntry()
        {
            MenuEntries[SelectedEntry].OnSelectEntry();
        }

        protected virtual void OnCancel()
        {
            ExitScreen();
        }

        protected void OnCancel(object sender, EventArgs e)
        {
            OnCancel();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            for (int i = 0; i < MenuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == SelectedEntry);

                MenuEntries[i].Update(isSelected, gameTime);
            }
        }

        public override void HandleInput()
        {
            MenuInput menuInput = ScreenManager.InputManager.Menu;

            if (menuInput.Up)
            {
                SelectedEntry--;

                if (SelectedEntry < 0)
                    SelectedEntry = MenuEntries.Count - 1;
            }

            if (menuInput.Down)
            {
                SelectedEntry++;

                if (SelectedEntry >= MenuEntries.Count)
                    SelectedEntry = 0;
            }

            if (menuInput.Select)
            {
                OnSelectEntry();
            }
            else if (menuInput.Cancel)
            {
                OnCancel();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 position = MenuPosition;

            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            if (State == ScreenState.TransitionOn)
                position.X -= transitionOffset * 256;
            else
                position.X += transitionOffset * 512;

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState, Matrix.CreateScale((float)viewport.Width / 1280, (float)viewport.Height / 720, 1));

            
            if (Background != null)
            {
                Color color = IsExiting ? Color.White : new Color(255, 255, 255, TransitionAlpha);
                    
                spriteBatch.Draw(Background, Vector2.Zero, color);
            }

            for (int i = 0; i < MenuEntries.Count; i++)
                DrawMenuEntry(gameTime, i, ref position);

            spriteBatch.End();
        }

        protected virtual void DrawMenuEntry(GameTime gameTime, int key, ref Vector2 position)
        {
            MenuEntry menuEntry = MenuEntries[key];

            bool isSelected = IsActive && (key == SelectedEntry);

            menuEntry.Draw(gameTime, this, position, isSelected);

            position.Y += menuEntry.GetHeight(this);
            position += EntryOffset;
        }

    }
}
