using Microsoft.Xna.Framework;

namespace DreamWorld.ScreenManagement
{
    public abstract class Screen
    {
        public ScreenManager ScreenManager { get; set; }

        protected Screen()
        {
        }

        public virtual void Initialize()
        {
            LoadContent();
        }

        protected virtual void LoadContent() { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime) { }
    }
}
