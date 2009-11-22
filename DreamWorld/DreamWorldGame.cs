using System.Threading;
using DreamWorld.InputManagement;
using DreamWorld.ScreenManagement;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

            GraphicsDeviceManager.MinimumPixelShaderProfile = ShaderProfile.PS_2_0;
            GraphicsDeviceManager.MinimumVertexShaderProfile = ShaderProfile.VS_2_0;

            GraphicsDeviceManager.PreferredBackBufferWidth = 1024;
            GraphicsDeviceManager.PreferredBackBufferHeight = 768;

            Content.RootDirectory = "Content";
            
            ScreenManager = new ScreenManager(this);
            InputManager = new InputManager(this);                     
            Components.Add(ScreenManager);
            Components.Add(InputManager);

            ScreenManager.AddScreen(new GameScreen());
        }     
    }
}
