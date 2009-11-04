using DreamWorld.InputManagement.Handlers;
using DreamWorld.InputManagement.Types;
using Microsoft.Xna.Framework;

namespace DreamWorld.InputManagement
{
    public class InputManager : GameComponent
    {
        public PlayerInput Player { get; private set; }
        public MenuInput Menu { get; private set; }

        public GamePadHandler GamePad { get; private set; }
        public KeyboardHandler Keyboard { get; private set; }
        public MouseHandler Mouse { get; private set; }

        public InputManager(Game game) : base(game)
        {
            Player = new PlayerInput(this);
            Menu = new MenuInput(this);

            GamePad = new GamePadHandler();
            Keyboard = new KeyboardHandler();
            Mouse = new MouseHandler();
        }

        public override void Update(GameTime gameTime)
        {
            GamePad.HandleInput();
            Keyboard.HandleInput();
            Mouse.HandleInput();
        }
    } 
}
