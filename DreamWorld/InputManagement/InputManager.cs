using DreamWorld.InputManagement.Handlers;
using DreamWorld.InputManagement.Types;
using Microsoft.Xna.Framework;

namespace DreamWorld.InputManagement
{
    public class InputManager : GameComponent
    {
        public PlayerInput Player { get; private set; }
        public MenuInput Menu { get; private set; }
        public DebugInput Debug { get; private set; }

        public GamePadHandler GamePad { get; private set; }
        public KeyboardHandler Keyboard { get; private set; }
        public MouseHandler Mouse { get; private set; }

        public InputManager(Game game) : base(game)
        {            
            Player = new PlayerInput    {InputManager = this};
            Menu = new MenuInput        {InputManager = this};
            Debug = new DebugInput      {InputManager = this};

            GamePad = new GamePadHandler    {InputManager = this};
            Keyboard = new KeyboardHandler  {InputManager = this};
            Mouse = new MouseHandler        {InputManager = this}; 
        }

        public override void Initialize()
        {
            GamePad.Initialize();
            Keyboard.Initialize();
            Mouse.Initialize();          
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            GamePad.Update(gameTime);
            Keyboard.Update(gameTime);
            Mouse.Update(gameTime);
            base.Update(gameTime);
        }
    } 
}
