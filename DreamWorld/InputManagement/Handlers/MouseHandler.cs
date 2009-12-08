using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DreamWorld.InputManagement.Handlers
{
    public class MouseHandler : InputHandler
    {
        public enum Buttons
        {
            LeftButton,
            RightButton
        }

        public MouseState PreviousState { get; private set; }
        public Vector2 Movement { get; private set; }
        public bool IgnoreReset { get; set; }
        private Point center;

        public MouseHandler()
        {
        }

        public override void Initialize()
        {
            InputManager.Game.IsMouseVisible = false;
            center = new Point(InputManager.Game.GraphicsDevice.Viewport.Width / 2, InputManager.Game.GraphicsDevice.Viewport.Height / 2);
            ResetMouse();
        }

        public override void HandleInput()
        {
            MouseState state = Mouse.GetState();

            Movement = new Vector2
               {
                   X =  center.X - state.X,
                   Y = center.Y - state.Y
               };            

            PreviousState = Mouse.GetState();
            ResetMouse();
        }

        public int ScrollWheel
        {
            get
            {
                return Mouse.GetState().ScrollWheelValue - PreviousState.ScrollWheelValue;
            }
        }

        public bool NewlyPressed(Buttons button)
        {
            switch (button)
            {               
                case Buttons.LeftButton:
                    return Mouse.GetState().LeftButton == ButtonState.Pressed && PreviousState.LeftButton == ButtonState.Released;                    
                case Buttons.RightButton:
                    return Mouse.GetState().RightButton == ButtonState.Pressed && PreviousState.RightButton == ButtonState.Released;
                default:
                    throw new InvalidOperationException("Unknown Button");
            }           
        }

        public bool NewlyReleased(Buttons button)
        {
            switch (button)
            {
                case Buttons.LeftButton:
                    return Mouse.GetState().LeftButton == ButtonState.Released && PreviousState.LeftButton == ButtonState.Pressed;
                case Buttons.RightButton:
                    return Mouse.GetState().RightButton == ButtonState.Released && PreviousState.RightButton == ButtonState.Pressed;
                default:
                    throw new InvalidOperationException("Unknown Button");
            }
        }

        private void ResetMouse()
        {
            if(!IgnoreReset)
                Mouse.SetPosition(center.X, center.Y);
        }
    }
}
