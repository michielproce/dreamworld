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

        private Point Position { get; set; }
        private MouseState PreviousState { get; set; }
        public Vector2 Movement { get; private set; }
        public bool IgnoreReset { get; set; }
        private Point _center;

        public override void Initialize()
        {
            InputManager.Game.IsMouseVisible = false;
            _center = new Point(InputManager.Game.GraphicsDevice.Viewport.Width / 2, InputManager.Game.GraphicsDevice.Viewport.Height / 2);
            ResetMouse();
        }

        protected override void HandleInput()
        {
            MouseState state = Mouse.GetState();
            
            Position = new Point(state.X, state.Y);
            
            Movement = new Vector2
               {
                   X =  _center.X - state.X,
                   Y = _center.Y - state.Y
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
            if(InputManager.DisableInput)
                return false;
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
            if (InputManager.DisableInput)
                return false;
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

        public void ResetMouse()
        {
            if(!IgnoreReset)
                Mouse.SetPosition(_center.X, _center.Y);
        }
    }
}
