using Microsoft.Xna.Framework.Input;

namespace DreamWorld.InputManagement.Types
{
    public class MenuInput : Input
    {
        public bool Up
        {
            get
            {
                return
                    InputManager.Keyboard.NewlyPressed(Keys.Up)
                     ||
                    InputManager.GamePad.NewlyPressed(Buttons.RightThumbstickUp);
            }
        }

        public bool Down
        {
            get
            {
                return InputManager.Keyboard.NewlyPressed(Keys.Down)
                     ||
                    InputManager.GamePad.NewlyPressed(Buttons.RightThumbstickDown);
            }
        }

        public bool Select
        {
            get
            {
                return InputManager.Keyboard.NewlyPressed(Keys.Enter)
                     ||
                    InputManager.GamePad.NewlyPressed(Buttons.A);
            }
        }

        public bool Cancel
        {
            get
            {
                return InputManager.Keyboard.NewlyPressed(Keys.Escape)
                     ||
                    InputManager.GamePad.NewlyPressed(Buttons.B)
                     ||
                    InputManager.GamePad.NewlyPressed(Buttons.Back);
            }
        }


    }
}