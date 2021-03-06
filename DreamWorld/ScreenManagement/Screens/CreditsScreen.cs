﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement.Screens
{
    public  class CreditsScreen : Screen
    {
        private Rectangle _destination;
        private Texture2D _texture;

        public CreditsScreen()
        {           
            TransitionOnTime = TimeSpan.FromMilliseconds(500);
            TransitionOffTime = TimeSpan.FromMilliseconds(500);
        }

        protected override void LoadContent()
        {
            _texture = ScreenManager.Game.Content.Load<Texture2D>(@"Textures\Menu\credits");
            Viewport vp = ScreenManager.Game.GraphicsDevice.Viewport;
            _destination = new Rectangle(0, 0, vp.Width, vp.Height);
        }


        public override void Update(GameTime gameTime)
        {
            if (ScreenManager.InputManager.Menu.Select || ScreenManager.InputManager.Menu.Cancel)
            {
                ScreenManager.AddScreen(new MainMenuScreen());
                ExitScreen();
            }
            
            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            Color color = IsExiting ? Color.White : new Color(255, 255, 255, TransitionAlpha);
            spriteBatch.Draw(_texture, _destination, color);
            spriteBatch.End();
        }
    }
}
