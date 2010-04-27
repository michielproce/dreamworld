using DreamWorld.Entities;
using DreamWorld.Levels;
using DreamWorld.ScreenManagement;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Interface.Help
{
    public class HelpSystem
    {        
        private const string HINT_TEXT = "Click the left mouse button or press B to read the sign.";
        private GameScreen gameScreen;
        private HelpScreen helpScreen;

        
        private SpriteFont hintFont;
        private Vector2 hintSize;
        private Vector2 hintPos;

        public Entity Helper { get; set; }
        public bool HintVisible { get; set; }

        public HelpSystem(GameScreen gameScreen)
        {
            this.gameScreen = gameScreen;

            Help.LoadInstance();
            
            HintVisible = true;
            hintFont = GameScreen.Instance.Content.Load<SpriteFont>(@"Fonts\helphint");
            hintSize = hintFont.MeasureString(HINT_TEXT);
            hintPos = new Vector2(gameScreen.GraphicsDevice.Viewport.Width / 2f - hintSize.X / 2f, 50f);

        }
        

        public void Update(GameTime gameTime)
        {
            if (Helper != null)
            {                
                if(gameScreen.InputManager.Player.ApplyRotation)
                {                    
                    helpScreen = new HelpScreen(this, Help.Instance.FindHelp(Helper));                    
                    gameScreen.ScreenManager.AddScreen(helpScreen);                    
                }

                // Remove the helper if we're to far away.
                if (Vector3.Distance(gameScreen.Level.Player.Body.Position, Helper.Body.Position) > Help.HELP_DISTANCE)
                    Helper = null;
            }

        }

        public void Draw(GameTime gameTime)
        {
            if (Helper != null && HintVisible)
            {
                SpriteBatch spriteBatch = gameScreen.ScreenManager.SpriteBatch;
                spriteBatch.Begin();
                spriteBatch.DrawString(hintFont, HINT_TEXT, hintPos + new Vector2(2), Color.Black);
                spriteBatch.DrawString(hintFont, HINT_TEXT, hintPos, Color.White);                
                spriteBatch.End();
            }
        }
    }
}
