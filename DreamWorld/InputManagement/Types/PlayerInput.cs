using System;
using DreamWorld.InputManagement.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DreamWorld.InputManagement.Types
{
    public class PlayerInput : Input
    {
        public const float MovementSpeed = 1f;
        public const float HorizontalMouseRotationSpeed = MathHelper.Pi/2000f;
        public const float HorizontalGamePadRotationSpeed = MathHelper.Pi/100f;
        public const float VerticalMouseRotationSpeed = MathHelper.Pi/2000f;
        public const float VerticalGamePadRotationSpeed = MathHelper.Pi/100f;

        public Vector3 Movement
        {
            get
            {
                return Vector3.Clamp(
                    new Vector3
                        ((InputManager.Keyboard.State.IsKeyDown(Keys.A) ? -MovementSpeed : 0) +
                         (InputManager.Keyboard.State.IsKeyDown(Keys.D) ? MovementSpeed : 0) +
                         InputManager.GamePad.State.ThumbSticks.Left.X*MovementSpeed,
                         0,
                         (InputManager.Keyboard.State.IsKeyDown(Keys.W) ? -MovementSpeed : 0) +
                         (InputManager.Keyboard.State.IsKeyDown(Keys.S) ? MovementSpeed : 0) +
                         -InputManager.GamePad.State.ThumbSticks.Left.Y*MovementSpeed
                        ),
                    new Vector3(-MovementSpeed),
                    new Vector3(MovementSpeed));
            }
        }

        public float Rotation
        {
            get
            {
                return (Mouse.GetState().LeftButton == ButtonState.Released
                            ? InputManager.Mouse.Movement.X*HorizontalMouseRotationSpeed
                            : 0) +
                       (InputManager.GamePad.State.IsButtonUp(Buttons.RightTrigger) &&
                        InputManager.GamePad.State.IsButtonUp(Buttons.LeftTrigger)
                            ? -InputManager.GamePad.State.ThumbSticks.Right.X*HorizontalGamePadRotationSpeed
                            : 0);
            }
        }

        public float HorizontalRotation
        {
            get
            {
                return (Mouse.GetState().LeftButton == ButtonState.Pressed
                           ? InputManager.Mouse.Movement.X*HorizontalMouseRotationSpeed
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
                       (InputManager.GamePad.State.IsButtonUp(Buttons.RightTrigger) &&
                        InputManager.GamePad.State.IsButtonUp(Buttons.LeftTrigger)
                            ? InputManager.GamePad.State.ThumbSticks.Right.Y*VerticalGamePadRotationSpeed
                            : 0);
            }
        }

        public float Zoom
        {
            get 
            { 
                return InputManager.Mouse.ScrollWheel/100f +
                    (InputManager.GamePad.State.IsButtonDown(Buttons.LeftTrigger) 
                    ? InputManager.GamePad.State.ThumbSticks.Right.Y 
                    : 0); 
            }
        }

        public bool Jump
        {
            get { return InputManager.Keyboard.NewlyPressed(Keys.Space) || InputManager.GamePad.NewlyPressed(Buttons.A); }
        }

        public bool ShowPauseMenu
        {
            get
            {
                return InputManager.Keyboard.NewlyPressed(Keys.Escape)
                       ||
                       InputManager.GamePad.NewlyPressed(Buttons.Back);
            }
        }

        public int SelectGroup
        {
            get
            {
                if (InputManager.Keyboard.NewlyPressed(Keys.LeftShift) ||
                    InputManager.GamePad.NewlyPressed(Buttons.RightShoulder))
                    return 1;
                if (InputManager.Keyboard.NewlyPressed(Keys.LeftControl) ||
                    InputManager.GamePad.NewlyPressed(Buttons.LeftShoulder))
                    return -1;

                return 0;
            }
        }

        public Vector3 RotateGroup
        {
            get
            {
                // Translate the user input to a group rotation
                switch (InputManager.GamePad.NewGesture)
                {
                    default:
                        if (InputManager.Keyboard.NewlyPressed(Keys.J))
                            return Vector3.Backward;
                        if (InputManager.Keyboard.NewlyPressed(Keys.L))
                            return Vector3.Forward;
                        if (InputManager.Keyboard.NewlyPressed(Keys.I))
                            return Vector3.Left;
                        if (InputManager.Keyboard.NewlyPressed(Keys.K))
                            return Vector3.Right;
                        if (InputManager.Keyboard.NewlyPressed(Keys.U))
                            return Vector3.Up;
                        if (InputManager.Keyboard.NewlyPressed(Keys.O))
                            return Vector3.Down;
                        return Vector3.Zero;
                    case Gestures.Up:
                        return Vector3.Left;
                    case Gestures.Down:
                        return Vector3.Right;
                    case Gestures.Left:
                        return Vector3.Backward;
                    case Gestures.Right:
                        return Vector3.Forward;
                    case Gestures.Clockwise:
                        return Vector3.Down;
                    case Gestures.CounterClockwise:
                        return Vector3.Up;
                }
            }
        }
    }
}