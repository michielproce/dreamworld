using System;
using DreamWorld.InputManagement.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DreamWorld.InputManagement.Types
{
    public class PlayerInput : Input
    {
        public const float HorizontalMouseRotationSpeed = MathHelper.Pi/2000f;
        public const float HorizontalGamePadRotationSpeed = MathHelper.Pi/100f;
        public const float VerticalMouseRotationSpeed = MathHelper.Pi/2000f;
        public const float VerticalGamePadRotationSpeed = MathHelper.Pi/100f;

        public Vector3 Movement
        {
            get
            {
                Vector3 movement = new Vector3
                    ((InputManager.Keyboard.State.IsKeyDown(Keys.A) ? -1 : 0) +
                     (InputManager.Keyboard.State.IsKeyDown(Keys.D) ? 1 : 0) +
                     InputManager.GamePad.State.ThumbSticks.Left.X,
                     0,
                     (InputManager.Keyboard.State.IsKeyDown(Keys.W) ? -1 : 0) +
                     (InputManager.Keyboard.State.IsKeyDown(Keys.S) ? 1 : 0) +
                     -InputManager.GamePad.State.ThumbSticks.Left.Y
                    );

                if (movement.Length() > 1)
                {
                    movement.Normalize();
                }
                return movement;
            }
        }

        public float Rotation
        {
            get
            {
                return (InputManager.Mouse.Movement.X*HorizontalMouseRotationSpeed)
                       + -InputManager.GamePad.State.ThumbSticks.Right.X*HorizontalGamePadRotationSpeed;
            }
        }

        public float HorizontalRotation
        {
            get
            {      
                return (Mouse.GetState().LeftButton == ButtonState.Pressed && InputManager.Keyboard.State.IsKeyDown(Keys.LeftControl)
                            ? InputManager.Mouse.Movement.X * HorizontalMouseRotationSpeed
                            : 0) +
                       (InputManager.GamePad.State.IsButtonDown(Buttons.LeftTrigger)
                            ? InputManager.GamePad.State.ThumbSticks.Right.X * HorizontalGamePadRotationSpeed
                            : 0);
            }
        }

        public bool ResetHorizontalRotation
        {
            get
            {
                return InputManager.Mouse.NewlyReleased(MouseHandler.Buttons.LeftButton) ||
                       InputManager.GamePad.NewlyReleased(Buttons.LeftTrigger);
            }
        }

        public float VerticalRotation
        {
            get
            {
                return (InputManager.Mouse.Movement.Y*VerticalMouseRotationSpeed) +
                       (InputManager.GamePad.State.ThumbSticks.Right.Y*VerticalGamePadRotationSpeed);
                  
            }
        }

        public float Zoom
        {
            get
            {
                return InputManager.Keyboard.State.IsKeyDown(Keys.LeftControl) ? InputManager.Mouse.ScrollWheel/100f : 0 +
                       (InputManager.GamePad.State.IsButtonDown(Buttons.LeftTrigger)
                            ? InputManager.GamePad.State.ThumbSticks.Right.Y
                            : 0);
            }
        }

        public bool Jump
        {
            get { return InputManager.Keyboard.NewlyPressed(Keys.Space) || InputManager.GamePad.NewlyPressed(Buttons.A); }
        }

        public bool Interact
        {
            get { return InputManager.Keyboard.NewlyPressed(Keys.Enter) || InputManager.GamePad.NewlyPressed(Buttons.B); }
        }

        public bool ShowPauseMenu
        {
            get
            {
                return InputManager.Keyboard.NewlyPressed(Keys.Escape)
                       ||
                       InputManager.GamePad.NewlyPressed(Buttons.Start);
            }
        }

        public int CycleAxle
        {
            get
            {
                return (InputManager.Mouse.ScrollWheel > 0 ? 1 : 0) +
                       (InputManager.Mouse.ScrollWheel < 0 ? -1 : 0) +
                       (InputManager.GamePad.NewlyPressed(Buttons.X) ? 1 : 0);
            }
        }

        public bool CycleAxleDirection
        {
            get
            {
                return InputManager.Mouse.NewlyPressed(MouseHandler.Buttons.RightButton) ||
                       InputManager.GamePad.NewlyPressed(Buttons.Y);
            }
        }

        public bool ApplyRotation
        {
            get
            {
                return InputManager.Mouse.NewlyPressed(MouseHandler.Buttons.LeftButton) ||
                       InputManager.GamePad.NewlyPressed(Buttons.B);
            }
        }
    }
}