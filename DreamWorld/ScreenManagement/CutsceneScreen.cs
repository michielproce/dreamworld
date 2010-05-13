using System;
using System.Collections.Generic;
using DreamWorld.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace DreamWorld.ScreenManagement
{
    public abstract class CutsceneScreen : Screen
    {
        private TimeSpan? _startTime;

        private TimeSpan _totalLineDuration = TimeSpan.Zero;
        private int _currentLine;
        private bool _started;
        private SoundEffectInstance _currentSoundEffectInstance;

        protected Texture2D Texture;
        protected List<CutsceneLine> Lines;

        protected TimeSpan Delay = TimeSpan.Zero;
        protected Song Song;
        protected float Volume = 1;

        private Vector2 _textPosition;
        private SpriteFont _font;
        private string _text;

        private SpriteFont _hintFont;
        private Vector2 _hintPosition;
        private string _hintText;

        protected CutsceneScreen()
        {
            _text = "";
        }

        protected override void LoadContent()
        {
            _font = Content.Load<SpriteFont>(@"Fonts\subtitle");
            _hintFont = Content.Load<SpriteFont>(@"Fonts\helphint");
            _hintText = StringUtil.ParsePlatform("Press {enter|a} to skip.");
            Vector2 hintSize = _hintFont.MeasureString(_hintText);
            _hintPosition = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - hintSize.X - 10, 5);

            Lines = new List<CutsceneLine>();
            LoadCutscene();
            if (Song != null)
            {
                MediaPlayer.Play(Song);
                MediaPlayer.Volume = Volume;
            }

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if(ScreenManager.InputManager.Menu.Select || ScreenManager.InputManager.Menu.Cancel)
                Stop();
            if (!_startTime.HasValue)
            {
                _startTime = gameTime.TotalGameTime;                
            }            
            TimeSpan total = gameTime.TotalGameTime - (TimeSpan) _startTime;
            if (total < Delay)
            {
                base.Update(gameTime);
                return;
            }
            if(!_started)
            {
                PlayCurrentLine();
                _startTime = gameTime.TotalGameTime;
                _started = true;
            }
            if (_currentLine == Lines.Count - 1)
                MediaPlayer.Volume *= .99f;
            if (_currentLine < Lines.Count)
            {                                
                if (_totalLineDuration - Lines[_currentLine].Delay < total)
                    _text = "";

                if (_totalLineDuration < total)
                {
                    _currentLine++;
                    if (_currentLine < Lines.Count)
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
            if(_currentSoundEffectInstance != null)
                _currentSoundEffectInstance.Stop(true);
            MediaPlayer.Stop();            
            ExitScreen();            
            ScreenManager.AddScreen(LoadNextScreen());
        }


        private void PlayCurrentLine()
        {
            if (Lines[_currentLine].Texture != null)
                Texture = Lines[_currentLine].Texture;
            _text = StringUtil.CutLine(ScreenManager.GraphicsDevice.Viewport, _font, Lines[_currentLine].Text, .9f);
            Vector2 textSize = _font.MeasureString(_text);
            Viewport vp = ScreenManager.GraphicsDevice.Viewport;           
            _textPosition = new Vector2(vp.Width / 2f - textSize.X / 2f, vp.Height - textSize.Y - 30f);
            SoundEffect sound = Content.Load<SoundEffect>(Lines[_currentLine].Audio);

            _currentSoundEffectInstance = sound.CreateInstance();            
            _currentSoundEffectInstance.Play();
            _totalLineDuration += sound.Duration + Lines[_currentLine].Delay;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(ScreenManager.BlankTexture, ScreenManager.FullscreenDestination, Color.Black);
            spriteBatch.Draw(Texture, ScreenManager.GraphicsDevice.Viewport.TitleSafeArea, new Color(255, 255, 255, TransitionAlpha));

            if (((DreamWorldGame)ScreenManager.Game).Config.Subtitles)
            {
                spriteBatch.DrawString(_font, _text, _textPosition + new Vector2(2), Color.Black);
                spriteBatch.DrawString(_font, _text, _textPosition, Color.White);
            }

            spriteBatch.DrawString(_hintFont, _hintText, _hintPosition + new Vector2(2), Color.Black);
            spriteBatch.DrawString(_hintFont, _hintText, _hintPosition, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }


        protected abstract Screen LoadNextScreen();
        protected abstract void LoadCutscene();        
    }
}
