using System;
using System.Collections.Generic;
using DreamWorld.Levels.VillageLevel;
using DreamWorld.ScreenManagement.Screens;
using DreamWorld.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace DreamWorld.ScreenManagement
{
    public abstract class CutsceneScreen : Screen
    {
        private TimeSpan? startTime;

        private int currentLine;
        private bool started;
        private SoundEffectInstance currentSoundEffectInstance;

        protected Texture2D texture;
        protected List<CutsceneLine> lines;

        protected TimeSpan delay = TimeSpan.Zero;
        protected Song song;
        protected float volume = 1;

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
            
            lines = new List<CutsceneLine>();
            LoadCutscene();
            if (song != null)
            {
                MediaPlayer.Play(song);
                MediaPlayer.Volume = volume;
            }

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if(ScreenManager.InputManager.Menu.Select || ScreenManager.InputManager.Menu.Cancel)
                Stop();
            if (!startTime.HasValue)
            {
                startTime = gameTime.TotalGameTime;                
            }            
            TimeSpan total = gameTime.TotalGameTime - (TimeSpan) startTime;
            if (total < delay)
            {
                base.Update(gameTime);
                return;
            }
            if(!started)
            {
                PlayCurrentLine();
                startTime = gameTime.TotalGameTime;
                started = true;
            }
            if (currentLine == lines.Count - 1)
                MediaPlayer.Volume *= .99f;
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
            } else
            {
                Stop();
            }

            base.Update(gameTime);
        }
        private void Stop()
        {
            if(currentSoundEffectInstance != null)
                currentSoundEffectInstance.Stop(true);
            MediaPlayer.Stop();            
            ExitScreen();            
            ScreenManager.AddScreen(LoadNextScreen());
        }


        private void PlayCurrentLine()
        {
            if (lines[currentLine].Texture != null)
                texture = lines[currentLine].Texture;
            text = StringUtil.CutLine(lines[currentLine].Text, 50);
            Vector2 textSize = font.MeasureString(text);
            Viewport vp = ScreenManager.GraphicsDevice.Viewport;           
            textPosition = new Vector2(vp.Width / 2f - textSize.X / 2f, vp.Height - textSize.Y - 100f);
            currentSoundEffectInstance = lines[currentLine].Audio.CreateInstance();
            currentSoundEffectInstance.Play();
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(ScreenManager.BlankTexture, ScreenManager.FullscreenDestination, Color.Black);
            spriteBatch.Draw(texture, ScreenManager.GraphicsDevice.Viewport.TitleSafeArea, new Color(255, 255, 255, TransitionAlpha));

            if (((DreamWorldGame)ScreenManager.Game).Config.Subtitles)
            {
                spriteBatch.DrawString(font, text, textPosition + new Vector2(2), Color.Black);
                spriteBatch.DrawString(font, text, textPosition, Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }


        protected abstract Screen LoadNextScreen();
        protected abstract void LoadCutscene();        
    }
}
