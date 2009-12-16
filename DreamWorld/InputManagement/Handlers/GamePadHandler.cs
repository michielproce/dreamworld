using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DreamWorld.InputManagement.Handlers
{
    public class GamePadHandler : InputHandler
    {
        public GamePadState State { get; private set; }
        private GamePadState PreviousState { get; set; }

        private Gesture Gesture;

        public Gestures NewGesture { 
            get
            {
                return Gesture == null ? Gestures.None : Gesture.NewGesture;
            }
        }

        public override void HandleInput()
        {
            PreviousState = State;
            State = GamePad.GetState(PlayerIndex.One);

            if(NewlyPressed(Buttons.RightTrigger))
            {
                Gesture = new Gesture { gamePadHandler = this };
            }
            else if (NewlyReleased(Buttons.RightTrigger))
            {
                Gesture = null;
            }

            if (Gesture != null)
            {
                Gesture.HandleInput();
            }
        }

        public bool NewlyPressed(Buttons button)
        {
            return PreviousState.IsButtonUp(button) && State.IsButtonDown(button);
        }

        public bool NewlyReleased(Buttons button)
        {
            return PreviousState.IsButtonDown(button) && State.IsButtonUp(button);
        }
    }
}
