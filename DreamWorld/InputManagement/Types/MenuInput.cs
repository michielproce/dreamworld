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
                    InputManager.GamePad.NewlyPressed(Buttons.RightThumbstickUp)
                     ||
                    InputManager.GamePad.NewlyPressed(Buttons.LeftThumbstickUp)
                     ||
                    InputManager.GamePad.NewlyPressed(Buttons.DPadUp);
            }
        }

        public bool Down
        {
            get
            {
                return InputManager.Keyboard.NewlyPressed(Keys.Down)
                     ||
                    InputManager.GamePad.NewlyPressed(Buttons.RightThumbstickDown)
                     ||
                    InputManager.GamePad.NewlyPressed(Buttons.LeftThumbstickDown)
                     ||
                    InputManager.GamePad.NewlyPressed(Buttons.DPadDown);
            }
        }

        public bool Select
        {
            get
            {
                return InputManager.Keyboard.NewlyPressed(Keys.Enter)
                     ||
                    InputManager.Keyboard.NewlyPressed(Keys.Space)
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