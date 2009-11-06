namespace DreamWorld.InputManagement
{
    public abstract class InputHandler
    {
        public InputManager InputManager { protected get; set; }
        public virtual void Initialize() { }
        public abstract void HandleInput();
    }
}