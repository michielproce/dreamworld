using DreamWorld.Cameras;
using DreamWorld.Entities;
using DreamWorld.ScreenManagement.Screens;
using DreamWorld.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Interface.Help
{
    public class HelpSystem
    {                        
        private readonly GameScreen _gameScreen;
        private HelpScreen _helpScreen;

        private string _hint;        
        private Vector2 _hintSize;
        private Vector2 _hintPos;

        private Entity _customHelper;

        public bool ScreenActive { get; set; }
        public Entity Helper { get; set; }
        public SpriteFont HintFont { get; private set; }

        public HelpSystem(GameScreen gameScreen)
        {
            _gameScreen = gameScreen;

            Help.LoadInstance();
        
            HintFont = GameScreen.Instance.Content.Load<SpriteFont>(@"Fonts\helphint");
        }
        

        public void Update(GameTime gameTime)
        {
            if(_gameScreen.Camera is DebugCamera)
                return;

            if (Helper != null) 
            {                
                if(_hint == null) {
                    _hint = StringUtil.ParsePlatform("{Click the left mouse button|Press B} to read the sign.");
                    _hintSize = HintFont.MeasureString(_hint);
                    _hintPos = new Vector2(_gameScreen.GraphicsDevice.Viewport.Width / 2f - _hintSize.X / 2f, 50f);
                }

                if(_gameScreen.InputManager.Player.ApplyRotation)
                {                    
                    _helpScreen = new HelpScreen(this, Help.Instance.FindHelp(Helper));                    
                    _gameScreen.ScreenManager.AddScreen(_helpScreen);                    
                }

                // Remove the helper if we're to far away.
                if (Vector3.Distance(_gameScreen.Level.Player.Body.Position, Helper.Body.Position) > Help.HelpDistance)
                {
                    Helper = null;
                    _hint = null;
                }
            }
            if(_customHelper != null)
            {
                // Remove the custom helper if we're to far away.
                if (Vector3.Distance(_gameScreen.Level.Player.Body.Position, _customHelper.Body.Position) > Help.HelpDistance)
                {
                    _customHelper = null;
                    _hint = null;
                }                
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (_hint != null && !ScreenActive)
            {
                SpriteBatch spriteBatch = _gameScreen.ScreenManager.SpriteBatch;
                spriteBatch.Begin();
                spriteBatch.DrawString(HintFont, _hint, _hintPos + new Vector2(2), Color.Black);
                spriteBatch.DrawString(HintFont, _hint, _hintPos, Color.White);                
                spriteBatch.End();
            }
        }

        public void ShowCustomHint(string hint, Entity helper)
        {
            _hint = hint;
            _customHelper = helper;
        }
    }
}
