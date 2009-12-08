using DreamWorld.Entities;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Interface
{
    public class DebugHUD
    {
        private Vector2 margin = new Vector2(10, 10);

        private Texture2D reticleTexture;
        private Vector2 reticlePosition;

        private SpriteBatch spriteBatch;
        private SpriteFont infoFont;
        private Texture2D infoBackgroundTexture;
        private Viewport viewport;
        private Vector2 infoPosition;
        private string infoText;
        
        public void Initialize()
        {
            spriteBatch = new SpriteBatch(GameScreen.Instance.GraphicsDevice);
            viewport = GameScreen.Instance.GraphicsDevice.Viewport;
            
            LoadContent();
        }

        protected void LoadContent()
        {
            infoFont = GameScreen.Instance.Content.Load<SpriteFont>(@"Fonts\DebugHUDFont");
            infoBackgroundTexture = GameScreen.Instance.Content.Load<Texture2D>(@"Textures\Debug\DebugHUDInfoBackground");
            reticleTexture = GameScreen.Instance.Content.Load<Texture2D>(@"Textures\Debug\Reticle");
            reticlePosition = new Vector2(viewport.Width / 2 - reticleTexture.Width / 2, viewport.Height / 2 - reticleTexture.Height / 2);
        }

        public void Update(GameTime time)
        {
            Entity selectedEntity = GameScreen.Instance.SelectedEntity;
            if (selectedEntity == null)
                infoText = "No entity selected";
            else
                infoText =
                    " === " + GameScreen.Instance.SelectedEntityName + " === \n" +
                    "Position: " + selectedEntity.Position;
            infoPosition = new Vector2(margin.X, viewport.Height - infoFont.MeasureString(infoText).Y - margin.Y);
        }


        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(reticleTexture, reticlePosition, Color.White);
            for (int x = 0; x < viewport.Width; x += infoBackgroundTexture.Width)
                spriteBatch.Draw(infoBackgroundTexture, new Vector2(x, infoPosition.Y - 25), Color.White);            
            spriteBatch.DrawString(infoFont, infoText, infoPosition, Color.White);
            
            spriteBatch.End();
        }

        
    }
}
