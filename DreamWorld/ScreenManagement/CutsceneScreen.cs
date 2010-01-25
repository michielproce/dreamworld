using System;
using System.Collections.Generic;
using DreamWorld.Levels.VillageLevel;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement
{
    public abstract class CutsceneScreen : Screen
    {
        private TimeSpan? startTime;

        private int currentTexture;
        private int currentLine;

        protected List<CutsceneTexture> textures;
        protected List<CutsceneLine> lines;

        private Vector2 textPosition;
        private SpriteFont font;
        private string text;

        protected CutsceneScreen()
        {
            text = "";
        }

        protected override void LoadContent()
        {
            font = Content.Load<SpriteFont>(@"Fonts\subtitle");
            textures = new List<CutsceneTexture>();
            lines = new List<CutsceneLine>();
            LoadCutscene();
            
        }

        public override void Update(GameTime gameTime)
        {
            if(ScreenManager.InputManager.Menu.Select || ScreenManager.InputManager.Menu.Cancel)
                Stop();
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
                Stop();

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
        private void Stop()
        {
            ScreenManager.AddScreen(LoadNextScreen());
            ExitScreen();
        }
        private void PlayCurrentLine()
        {
            text = CutLine(lines[currentLine].Text, 50);
            Vector2 textSize = font.MeasureString(text);
            Viewport vp = ScreenManager.GraphicsDevice.Viewport;
            
            textPosition = new Vector2(vp.Width / 2f - textSize.X / 2f, vp.Height - textSize.Y - 100f);
            lines[currentLine].Audio.Play();
        }

        private static string CutLine(string line, int max)
        {
            for (int i = 0; i < line.Length; i += max)
            {
                for (int j = i; j > 0; j--)
                {
                    if (line[j] == ' ')
                    {
                        line = line.Remove(j, 1).Insert(j, "\n");
                        break;
                    }
                }
            }
            return line;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(ScreenManager.BlankTexture, ScreenManager.FullscreenDestination, Color.Black);
            spriteBatch.Draw(textures[currentTexture].Texture, ScreenManager.GraphicsDevice.Viewport.TitleSafeArea, Color.White);
            if (((DreamWorldGame)ScreenManager.Game).Config.Subtitles)
            {
                spriteBatch.DrawString(font, text, textPosition + new Vector2(2), Color.Black);
                spriteBatch.DrawString(font, text, textPosition, Color.White);
            }
            spriteBatch.End();
        }


        protected abstract Screen LoadNextScreen();
        protected abstract void LoadCutscene();        
    }
}
