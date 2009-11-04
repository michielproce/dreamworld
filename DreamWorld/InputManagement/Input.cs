namespace DreamWorld.InputManagement
{
    public abstract class Input
    {
        protected InputManager InputManager { get; private set; }

        protected Input(InputManager inputManager)
        {
            InputManager = inputManager;
        }
    }
}