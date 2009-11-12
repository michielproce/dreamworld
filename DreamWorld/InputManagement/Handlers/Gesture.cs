using System;
using Microsoft.Xna.Framework;

namespace DreamWorld.InputManagement.Handlers
{
    public enum Gestures
    {
        None,
        Up,
        Down,
        Left,
        Right,
        Clockwise,
        CounterClockwise
    }

    public class Gesture
    {
        private Vector2 state;
        private Vector2 previousState;

        private Vector2 direction;
        private double gestureAngle;
        private bool gestureIsHandled;

        private const float MinimumDirectionLength = 0.75f;
        private const float MinimumRotationLength = 0.5f;
        private const double GestureAngleTrigger = 67.5;

        public Gestures NewGesture
        {
            get
            {
                if (!gestureIsHandled)
                {
                    if (gestureAngle > GestureAngleTrigger)
                    {
                        return Gestures.Clockwise;
                    }

                    if (gestureAngle < -GestureAngleTrigger)
                    {
                        return Gestures.CounterClockwise;
                    }

                    if (state == Vector2.Zero && direction != Vector2.Zero)
                    {
                        float angle = MathHelper.ToDegrees((float)Math.Atan2(direction.X, direction.Y));
                        if (angle > 45 && angle < 135)
                        {
                            return Gestures.Right;
                        }
                        if (angle > 135 || angle < -135)
                        {
                            return Gestures.Down;
                        }
                        if (angle > -135 && angle < -45)
                        {
                            return Gestures.Left;
                        }
                        if (angle > -45 || angle < 45)
                        {
                            return Gestures.Up;
                        }
                    }
                }
                return Gestures.None;
            }
        }

        public GamePadHandler gamePadHandler { private get; set; }

        public Gesture()
        {
            ResetGesture();
        }

        internal void HandleInput()
        {
            if (NewGesture != Gestures.None)
            {
                // Gestures should be handled once
                gestureIsHandled = true;
            }

            state = gamePadHandler.State.ThumbSticks.Right;

            if (state == Vector2.Zero && gestureIsHandled)
            {
                ResetGesture();
            }

            // Handle directions
            if (state.Length() > MinimumDirectionLength)
            {
                direction = state;
            }

            // Handle rotation
            if (previousState.Length() > MinimumRotationLength && state.Length() > MinimumRotationLength)
            {
                float angle = MathHelper.ToDegrees((float)
                                (Math.Atan2(state.X, state.Y) -
                                 Math.Atan2(previousState.X, previousState.Y)));

                if (angle > 300)
                {
                    angle = angle - 360;
                }
                else if (angle < -300)
                {
                    angle = angle + 360;
                }
                gestureAngle += angle;
            }

            previousState = state;
        }

        public void ResetGesture()
        {
            gestureIsHandled = false;
            gestureAngle = 0;
            previousState = Vector2.Zero;
            state = Vector2.Zero;
            direction = Vector2.Zero;
        }
    }
}
