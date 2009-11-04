using Microsoft.Xna.Framework.Input;

namespace DreamWorld.InputManagement.Handlers
{
    public class KeyboardHandler : InputHandler
    {        
        public KeyboardState PreviousState { get; private set; }

        public override void HandleInput()
        {
            PreviousState = Keyboard.GetState();
        }
    }
}