using DreamWorld.Cameras;
using DreamWorld.Entities;
using DreamWorld.InputHandlers;
using DreamWorld.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld
{
    public class DreamWorldGame : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }
        public InputHandler[] InputHandlers { get; private set; }
        public Player Player { get; private set; }
        public Camera CurrentCamera { get; set; }
        public Level CurrentLevel { get; set; }
        
        public DreamWorldGame()
        {
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            InputHandlers = new InputHandler[3];
            InputHandlers[0] = new GamepadHandler(this);
            InputHandlers[1] = new KeyboardHandler(this);
            InputHandlers[2] = new MouseHandler(this);
            foreach (InputHandler inputHandler in InputHandlers)
            {
                Components.Add(inputHandler);   
            }

//            Player = new Player(this);
//            Components.Add(Player);

            CurrentCamera = new DebugCamera(this);
            Components.Add(CurrentCamera);

            CurrentLevel = new TestLevel(this);
            Components.Add(CurrentLevel);

            base.Initialize();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
