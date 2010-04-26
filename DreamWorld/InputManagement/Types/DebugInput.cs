using DreamWorld.InputManagement.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DreamWorld.InputManagement.Types
{
    public class DebugInput : Input
    {
        public const float MovementSpeed = 3f;
        public const float RotationSpeed = MathHelper.Pi / 2000f;

        public Vector3 Movement
        {
            get
            {
                return new Vector3
                    {
                        X = (InputManager.Keyboard.State.IsKeyDown(Keys.A) ? -MovementSpeed : 0) +
                            (InputManager.Keyboard.State.IsKeyDown(Keys.D) ? MovementSpeed : 0),
                        Y = (InputManager.Keyboard.State.IsKeyDown(Keys.LeftControl) ? -MovementSpeed : 0) +
                            (InputManager.Keyboard.State.IsKeyDown(Keys.Space) ? MovementSpeed : 0),
                        Z = (InputManager.Keyboard.State.IsKeyDown(Keys.W) ? -MovementSpeed : 0) +
                            (InputManager.Keyboard.State.IsKeyDown(Keys.S) ? MovementSpeed : 0)
                    };
            }
        }

        public Vector2 Rotation
        {
            get
            {
                return new Vector2
                   {
                       X = InputManager.Mouse.Movement.X * RotationSpeed,
                       Y = InputManager.Mouse.Movement.Y * RotationSpeed
                   };
            }
        }

        public bool ToggleDebugCamera
        {
            get
            {
                return InputManager.Keyboard.NewlyPressed(Keys.Tab);
            }
        }

        public bool ToggleDebugCameraReticle
        {
            get
            {
                return InputManager.Keyboard.NewlyPressed(Keys.F1);
            }
        }

        public bool SelectEntity 
        {
            get
            {
                return InputManager.Mouse.NewlyPressed(MouseHandler.Buttons.LeftButton);
            }
        }

        public bool NewEntity
        {
            get
            {
                return InputManager.Mouse.NewlyPressed(MouseHandler.Buttons.RightButton);
            }
        }
    }
}
