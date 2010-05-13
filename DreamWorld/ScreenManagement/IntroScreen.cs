using System;
using DreamWorld.ScreenManagement.Screens.Cutscenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement
{
    public abstract class IntroScreen : Screen
    {
        private Texture2D _texture;

        private readonly TimeSpan _duration;
        
        
        protected IntroScreen()
        {
            _duration = TimeSpan.FromSeconds(3);
            TransitionOnTime = TimeSpan.FromMilliseconds(500);
            TransitionOffTime = TimeSpan.FromMilliseconds(500);            
        }

        protected override void LoadContent()
        {
            _texture = ScreenManager.Game.Content.Load<Texture2D>(@"Textures\Intro\" + TextureName);
        }

        protected abstract string TextureName { get; }


        public override void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime > _duration && !IsExiting)
            {
                ScreenManager.AddScreen(new GlobalIntroCutscene());
                ExitScreen();
            }
            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(_texture, ScreenManager.Game.GraphicsDevice.Viewport.TitleSafeArea, new Color(255, 255, 255, TransitionAlpha));
            spriteBatch.End();
        }    
    }
}
