﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DreamWorld.InputManagement.Handlers
{
    public class GamePadHandler : InputHandler
    {
        public GamePadState State { get; private set; }
        public GamePadState PreviousState { get; private set; }

        public override void HandleInput()
        {
            PreviousState = State;
            State = GamePad.GetState(PlayerIndex.One);
        }

        public bool NewlyPressed(Buttons button)
        {
            return PreviousState.IsButtonUp(button) && State.IsButtonDown(button);
        }

        public bool NewlyReleased(Buttons button)
        {
            return PreviousState.IsButtonDown(button) && State.IsButtonUp(button);
        }
    }
}
