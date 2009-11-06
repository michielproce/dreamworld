using DreamWorld.InputManagement;
using DreamWorld.ScreenManagement;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;

namespace DreamWorld
{
    public class DreamWorldGame : Game
    {
        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }
        public InputManager InputManager { get; private set; }
        public ScreenManager ScreenManager { get; private set; }
   
        public DreamWorldGame()
        {
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            InputManager = new InputManager(this);
            ScreenManager = new ScreenManager(this);
            Components.Add(InputManager);
            Components.Add(ScreenManager);
            ScreenManager.AddScreen(new GameScreen());
        }     
    }
}
