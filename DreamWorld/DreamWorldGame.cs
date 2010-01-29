using DreamWorld.InputManagement;
using DreamWorld.Levels.PuzzleLevel1;
using DreamWorld.Levels.VillageLevel;
using DreamWorld.ScreenManagement;
using DreamWorld.ScreenManagement.Screens;
using DreamWorld.ScreenManagement.Screens.Cutscenes;
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
            Content.RootDirectory = "Content";
            GraphicsDeviceManager = new GraphicsDeviceManager(this);

            GraphicsDeviceManager.MinimumPixelShaderProfile = ShaderProfile.PS_2_0;
            GraphicsDeviceManager.MinimumVertexShaderProfile = ShaderProfile.VS_2_0;

            #if (XBOX)
            Config = new Config();
            GraphicsDeviceManager.PreferredBackBufferWidth = 768;
            GraphicsDeviceManager.PreferredBackBufferHeight = 576;
            GraphicsDeviceManager.PreferMultiSampling = true;
            GraphicsDeviceManager.SynchronizeWithVerticalRetrace = true;

            Config.Bloom = true;
            Config.EdgeDetect = true;
            Config.Particles = true;
            Config.Shadows = true;

            #else
            ApplyConfig();
            #endif

            ScreenManager = new ScreenManager(this);
            InputManager = new InputManager(this);                     
            Components.Add(ScreenManager);
            Components.Add(InputManager);

            ScreenManager.AddScreen(new FloatingKoalaGamesIntroScreen());
//            ScreenManager.AddScreen(new GameScreen(new VillageLevel()));
//            ScreenManager.AddScreen(new GameScreen(new VillageLevel{LevelsCompleted = 1}));
//            ScreenManager.AddScreen(new GameScreen(new PuzzleLevel1()));
        }

        public void ApplyConfig()
        {
            Config = Config.Load();
            GraphicsDeviceManager.PreferredBackBufferWidth = Config.Width;
            GraphicsDeviceManager.PreferredBackBufferHeight = Config.Height;
            GraphicsDeviceManager.IsFullScreen = Config.Fullscreen;
            GraphicsDeviceManager.PreferMultiSampling = Config.AntiAliasing;
            GraphicsDeviceManager.SynchronizeWithVerticalRetrace = Config.VerticalSync;
            
            GraphicsDeviceManager.ApplyChanges();
        }
    }
}
