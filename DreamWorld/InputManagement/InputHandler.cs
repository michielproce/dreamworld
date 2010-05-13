using Microsoft.Xna.Framework;

namespace DreamWorld.InputManagement
{
    public abstract class InputHandler
    {
        public InputManager InputManager { protected get; set; }
        public virtual void Initialize() { }

        public void Update(GameTime gameTime)
        {
            if (InputManager.Game.IsActive && !InputManager.DisableInput)
                HandleInput();
        }

        protected abstract void HandleInput();
    }
}