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
                    X = (Keyboard.GetState().IsKeyDown(Keys.A) ? -MovementSpeed : 0) +
                        (Keyboard.GetState().IsKeyDown(Keys.D) ? MovementSpeed : 0),
                    Y = (Keyboard.GetState().IsKeyDown(Keys.LeftControl) ? -MovementSpeed : 0) +
                        (Keyboard.GetState().IsKeyDown(Keys.Space) ? MovementSpeed : 0),
                    Z = (Keyboard.GetState().IsKeyDown(Keys.W) ? -MovementSpeed : 0) +
                        (Keyboard.GetState().IsKeyDown(Keys.S) ? MovementSpeed : 0)
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
    }
}
