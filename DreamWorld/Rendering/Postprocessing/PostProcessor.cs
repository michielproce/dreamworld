using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Rendering.Postprocessing
{
    public abstract class PostProcessor
    {
        protected Game game;
        protected SpriteBatch spriteBatch;
        protected GraphicsDevice device;

        protected PostProcessor(Game game, SpriteBatch spriteBatch)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.device = game.GraphicsDevice;
        }

        public abstract void Draw(GameTime gameTime);
    }
}
