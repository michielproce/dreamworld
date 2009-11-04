using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DreamWorld.InputManagement.Handlers
{
    public class GamePadHandler : InputHandler
    {
        public GamePadState PreviousState { get; private set; }

        public override void HandleInput()
        {
            PreviousState = GamePad.GetState(PlayerIndex.One);
        }
    }
}
