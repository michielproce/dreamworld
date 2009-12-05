using DreamWorld.InputManagement;
using DreamWorld.Levels.VillageLevel;
using DreamWorld.ScreenManagement;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld
{
    public class DreamWorldGame : Game
    {
        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }
        public Config Config { get; private set; }
        public InputManager InputManager { get; private set; }
        public ScreenManager ScreenManager { get; private set; }
   
        public DreamWorldGame()
        {
            GraphicsDeviceManager = new GraphicsDeviceManager(this);

            GraphicsDeviceManager.MinimumPixelShaderProfile = ShaderProfile.PS_2_0;
            GraphicsDeviceManager.MinimumVertexShaderProfile = ShaderProfile.VS_2_0;

#if (XBOX)
            GraphicsDeviceManager.PreferredBackBufferWidth = 1280;
            GraphicsDeviceManager.PreferredBackBufferHeight = 720;

            GraphicsDeviceManager.PreferMultiSampling = true;
            GraphicsDeviceManager.SynchronizeWithVerticalRetrace = true;
#else
            Config = Config.Load();

            GraphicsDeviceManager.PreferredBackBufferWidth = Config.Width;
            GraphicsDeviceManager.PreferredBackBufferHeight = Config.Height;
            GraphicsDeviceManager.IsFullScreen = Config.Fullscreen;
            GraphicsDeviceManager.PreferMultiSampling = Config.AntiAliasing;
            GraphicsDeviceManager.SynchronizeWithVerticalRetrace = Config.VerticalSync;
#endif
            Content.RootDirectory = "Content";
            
            ScreenManager = new ScreenManager(this);
            InputManager = new InputManager(this);                     
            Components.Add(ScreenManager);
            Components.Add(InputManager);

            ScreenManager.AddScreen(new GameScreen(new VillageLevel()));
        }     
    }
}
