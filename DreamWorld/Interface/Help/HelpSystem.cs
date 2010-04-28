using DreamWorld.Cameras;
using DreamWorld.Entities;
using DreamWorld.Levels;
using DreamWorld.ScreenManagement;
using DreamWorld.ScreenManagement.Screens;
using DreamWorld.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Interface.Help
{
    public class HelpSystem
    {                        
        private GameScreen gameScreen;
        private HelpScreen helpScreen;

        private string hint;
        private SpriteFont hintFont;
        private Vector2 hintSize;
        private Vector2 hintPos;

        private Entity customHelper;
        public bool HintVisible { get; set; }
        public Entity Helper { get; set; }        

        public HelpSystem(GameScreen gameScreen)
        {
            this.gameScreen = gameScreen;

            Help.LoadInstance();
            
            HintVisible = true;
            hintFont = GameScreen.Instance.Content.Load<SpriteFont>(@"Fonts\helphint");
        }
        

        public void Update(GameTime gameTime)
        {
            if(gameScreen.Camera is DebugCamera)
                return;

            if (Helper != null) 
            {                
                if(hint == null) {
                    hint = StringUtil.ParsePlatform("{Click the left mouse button|Press B} to read the sign.");
                    hintSize = hintFont.MeasureString(hint);
                    hintPos = new Vector2(gameScreen.GraphicsDevice.Viewport.Width / 2f - hintSize.X / 2f, 50f);
                }

                if(gameScreen.InputManager.Player.ApplyRotation)
                {                    
                    helpScreen = new HelpScreen(this, Help.Instance.FindHelp(Helper));                    
                    gameScreen.ScreenManager.AddScreen(helpScreen);                    
                }

                // Remove the helper if we're to far away.
                if (Vector3.Distance(gameScreen.Level.Player.Body.Position, Helper.Body.Position) > Help.HELP_DISTANCE)
                {
                    Helper = null;
                    hint = null;
                }
            }
            if(customHelper != null)
            {
                // Remove the custom helper if we're to far away.
                if (Vector3.Distance(gameScreen.Level.Player.Body.Position, customHelper.Body.Position) > Help.HELP_DISTANCE)
                {
                    customHelper = null;
                    hint = null;
                }                
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (hint != null && HintVisible)
            {
                SpriteBatch spriteBatch = gameScreen.ScreenManager.SpriteBatch;
                spriteBatch.Begin();
                spriteBatch.DrawString(hintFont, hint, hintPos + new Vector2(2), Color.Black);
                spriteBatch.DrawString(hintFont, hint, hintPos, Color.White);                
                spriteBatch.End();
            }
        }

        public void ShowCustomHint(string hint, Entity helper)
        {
            this.hint = hint;
            this.customHelper = helper;
        }
    }
}
