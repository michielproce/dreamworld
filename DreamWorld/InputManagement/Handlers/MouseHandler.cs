using Microsoft.Xna.Framework.Input;

namespace DreamWorld.InputManagement.Handlers
{
    public class MouseHandler : InputHandler
    {
        public MouseState PreviousState { get; set; }

        public override void HandleInput()
        {
            PreviousState = Mouse.GetState();    
        }
    }
}
