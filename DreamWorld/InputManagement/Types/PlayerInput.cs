using DreamWorld.InputManagement.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DreamWorld.InputManagement.Types
{
    public class PlayerInput : Input
    {
        public const float MovementSpeed = .5f;
        public const float HorizontalRotationSpeed = MathHelper.Pi / 2000f;
        public const float VerticalRotationSpeed = MathHelper.Pi / 2000f;

        public Vector3 Movement
        {
            get
            {
                return new Vector3
                {
                    X = (InputManager.Keyboard.State.IsKeyDown(Keys.A) ? -MovementSpeed : 0) +
                        (InputManager.Keyboard.State.IsKeyDown(Keys.D) ? MovementSpeed : 0),
                    Y = 0,
                    Z = (InputManager.Keyboard.State.IsKeyDown(Keys.W) ? -MovementSpeed : 0) +
                        (InputManager.Keyboard.State.IsKeyDown(Keys.S) ? MovementSpeed : 0)
                };
            }
        }

        public float HorizontalRotation
        {
            get
            {
                return InputManager.Mouse.Movement.X * HorizontalRotationSpeed;
            }
        }

        public float VerticalRotation
        {
            get
            {
                return InputManager.Mouse.Movement.Y * VerticalRotationSpeed;
            }
        }

        public float Zoom
        {
            get
            {
                return InputManager.Mouse.ScrollWheel / 100f;
            }
        }

        public bool Jump
        {
            get
            {
                return InputManager.Keyboard.NewlyPressed(Keys.Space);   
            }
        }

        public bool ShowPauseMenu
        {
            get
            {
                return InputManager.Keyboard.NewlyPressed(Keys.Escape)
                     ||
                    InputManager.GamePad.NewlyPressed(Buttons.B);
            }
        }

        public Gestures RotateGroup
        {
            get
            {
                return InputManager.GamePad.NewGesture;
            }
        }
    }
}
