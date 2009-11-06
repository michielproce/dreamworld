using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DreamWorld.InputManagement.Handlers
{
    public class MouseHandler : InputHandler
    {
        public MouseState PreviousState { get; private set; }
        public Vector2 Movement { get; private set; }
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
                   X = state.X - center.X,
                   Y = state.Y - center.Y
               };            

            PreviousState = Mouse.GetState();
            ResetMouse();
        }

        private void ResetMouse()
        {
            Mouse.SetPosition(center.X, center.Y);
        }
    }
}
