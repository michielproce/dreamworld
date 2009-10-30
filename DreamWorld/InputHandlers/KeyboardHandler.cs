using System;
using DreamWorld.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DreamWorld.InputHandlers
{
    class KeyboardHandler : InputHandler
    {        
        private KeyboardState prevKbState;


        public KeyboardHandler(Game game) : base(game)
        {
        }

        protected override void HandleInput(GameTime gameTime)
        {
            KeyboardState kbState = Keyboard.GetState();
            if (camera is DebugCamera)
            {
                DebugCamera dc = (DebugCamera)camera;

                if (kbState.IsKeyDown(Keys.W))
                    dc.Move(Vector3.Forward);
                if (kbState.IsKeyDown(Keys.S))
                    dc.Move(Vector3.Backward);
                if (kbState.IsKeyDown(Keys.A))
                    dc.Move(Vector3.Left);
                if (kbState.IsKeyDown(Keys.D))
                    dc.Move(Vector3.Right);
                if (kbState.IsKeyDown(Keys.Space))
                    dc.Move(Vector3.Up);
                if (kbState.IsKeyDown(Keys.C) || kbState.IsKeyDown(Keys.LeftControl))
                    dc.Move(Vector3.Down);
            }

            prevKbState = kbState;
        }
    }
}