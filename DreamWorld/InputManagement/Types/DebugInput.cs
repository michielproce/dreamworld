using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DreamWorld.InputManagement.Types
{
    public class DebugInput : Input
    {
        public const float MovementSpeed = .5f;
        public const float RotationSpeed = MathHelper.Pi / 2000f;

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

        public Vector2 Rotation
        {
            get
            {
                return new Vector2
                   {
                       X = InputManager.Mouse.Movement.X*-RotationSpeed,
                       Y = InputManager.Mouse.Movement.Y*-RotationSpeed
                   };
            }
        }
    }
}
