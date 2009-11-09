using Microsoft.Xna.Framework.Input;

namespace DreamWorld.InputManagement.Handlers
{
    public class KeyboardHandler : InputHandler
    {
        public KeyboardState State { get; private set; }
        public KeyboardState PreviousState { get; private set; }

        public override void HandleInput()
        {
            PreviousState = State;
            State = Keyboard.GetState();
        }

        public bool NewlyPressed(Keys key)
        {
            return PreviousState.IsKeyUp(key) && State.IsKeyDown(key);
        }

        public bool NewlyReleased(Keys key)
        {
            return PreviousState.IsKeyDown(key) && State.IsKeyUp(key);
        }
    }
}