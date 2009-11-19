using System.Threading;
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
//            GraphicsDeviceManager.PreferMultiSampling = true;
            GraphicsDeviceManager.PreferredBackBufferWidth = 800;
            GraphicsDeviceManager.PreferredBackBufferHeight = 600;

            Content.RootDirectory = "Content";
            
            ScreenManager = new ScreenManager(this);
            InputManager = new InputManager(this);                     
            Components.Add(ScreenManager);
            Components.Add(InputManager);

            ScreenManager.AddScreen(new GameScreen());
        }     
    }
}
