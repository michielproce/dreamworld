using System;
using DreamWorld.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DreamWorld.InputHandlers
{
    class MouseHandler : InputHandler
    {
        private MouseState prevMState;
        private Point center;

        public MouseHandler(Game game) : base(game)
        {
        }

        public override void Initialize()
        {            
            center = new Point(Game.GraphicsDevice.Viewport.Width/2, Game.GraphicsDevice.Viewport.Height/2);
            ResetMouse();
            base.Initialize();            
        }

        protected override void HandleInput(GameTime gameTime)
        {
            MouseState mState = Mouse.GetState();

            float deltaX = mState.X - center.X;
            float deltaY = mState.Y - center.Y;
            
            if (camera is DebugCamera)
            {
                DebugCamera dc = (DebugCamera)camera;
                dc.Rotate(deltaX, deltaY);
            }

            prevMState = mState;
            ResetMouse();
        }
        
        private void ResetMouse()
        {
            Mouse.SetPosition(center.X, center.Y);
        }
    }
}
