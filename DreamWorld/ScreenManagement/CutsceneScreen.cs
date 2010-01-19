using System;
using System.Collections.Generic;
using DreamWorld.Levels.VillageLevel;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement
{
    public abstract class CutsceneScreen : Screen
    {
        private TimeSpan? startTime;
        private Rectangle destination;

        private int currentTexture;
        private int currentLine;

        private List<CutsceneTexture> textures;
        private List<CutsceneLine> lines;

        private Vector2 textPosition;
        private SpriteFont font;
        private string text;

        protected CutsceneScreen()
        {
            text = "";
        }

        public override void Initialize()
        {
            Viewport vp = ScreenManager.Game.GraphicsDevice.Viewport;
            destination = new Rectangle(0, 0, vp.Width, vp.Height);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            font = Content.Load<SpriteFont>(@"Fonts\default");
            textures = LoadTextures();
            lines = LoadLines();
        }

        public override void Update(GameTime gameTime)
        {
            if (!startTime.HasValue)
            {
                startTime = gameTime.TotalGameTime;
                PlayCurrentLine(); 
            }
            TimeSpan total = gameTime.TotalGameTime - (TimeSpan) startTime;

            TimeSpan totalTextureDuration = TimeSpan.Zero;
            for(int i=0; i<=currentTexture; i++)            
                totalTextureDuration += textures[i].Duration;
            if (totalTextureDuration < total)
                currentTexture++;
            if(currentTexture >= textures.Count)
            {
                ScreenManager.AddScreen(LoadNextScreen());
                ExitScreen();
            }

            if (currentLine < lines.Count)
            {
                TimeSpan totalLineDuration = TimeSpan.Zero;
                for (int i = 0; i <= currentLine; i++)
                    totalLineDuration += lines[i].Duration;

                if (totalLineDuration - lines[currentLine].Delay < total)
                    text = "";

                if (totalLineDuration < total)
                {
                    currentLine++;
                    if (currentLine < lines.Count)
                    {                        
                        PlayCurrentLine();   
                    }
                }
            }
        }

        private void PlayCurrentLine()
        {
            text = lines[currentLine].Text;
            Vector2 textSize = font.MeasureString(text);
            Viewport vp = ScreenManager.Game.GraphicsDevice.Viewport;
            textPosition = new Vector2(vp.Width / 2f - textSize.X / 2f, vp.Height - textSize.Y - 50f);
            lines[currentLine].Audio.Play();
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(textures[currentTexture].Texture, destination, Color.White);
            if(((DreamWorldGame)ScreenManager.Game).Config.Subtitles)
                spriteBatch.DrawString(font, text, textPosition, Color.White);
            spriteBatch.End();

        }

        protected abstract List<CutsceneTexture> LoadTextures();
        protected abstract List<CutsceneLine> LoadLines();
        protected abstract Screen LoadNextScreen();
        
    }
}
