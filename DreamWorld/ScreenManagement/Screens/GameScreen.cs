using System;
using DreamWorld.Cameras;
using DreamWorld.Helpers.Renderers;
using DreamWorld.InputManagement;
using DreamWorld.InputManagement.Types;
using DreamWorld.Levels;
using JigLibX.Collision;
using JigLibX.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement.Screens
{
    public class GameScreen : Screen
    {
        public TutorialText TutorialText { get; private set; }
        public static GameScreen Instance { get; private set; }
        public Camera Camera { get; private set; }
        public Level Level { get; private set; }
        public InputManager InputManager { get; private set; }
        public GraphicsDevice GraphicsDevice { get; private set; }

        public DebugDrawer debugDrawer;

        public DreamWorldPhysicsSystem PhysicsSystem { get; private set; }

        public GameScreen(Level level)
        {
            Instance = this;            
            LoadingScreen = new LoadingScreen(@"Textures/Test/gradient");
            State = ScreenState.Hidden;
            
            
            Level = level;
            Level.GameScreen = this;           
        }

        public override void Initialize()
        {
            InputManager = ((DreamWorldGame) ScreenManager.Game).InputManager;
            GraphicsDevice = ScreenManager.Game.GraphicsDevice;

            PhysicsSystem = new DreamWorldPhysicsSystem {CollisionSystem = new CollisionSystemBrute()};

            base.Initialize();

            debugDrawer = new DebugDrawer(ScreenManager.Game, this);
            debugDrawer.Initialize();
            debugDrawer.Enabled = true;

            Camera = new ThirdPersonCamera();
            Level.Initialize();
            Camera.Initialize();

            TutorialText = new TutorialText(ScreenManager.SpriteBatch, 
                                            Content.Load<SpriteFont>(@"Fonts\tutorial"),
                                            Content.Load<Texture2D>(@"Textures\blank"),
                                            new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y) + new Vector2(50));
            
            LoadingScreen.Loaded = true;
        }

        public override void UnloadContent()
        {
            Content.Dispose();
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (!OtherScreenHasFocus)
            {
                #if (DEBUG && !XBOX)
                    if (((DreamWorldGame)ScreenManager.Game).InputManager.Debug.ToggleDebugCamera)
                    {
                        if (Camera is DebugCamera)
                        {
                            ((DebugCamera)Camera).DisposeForm();
                            Camera = new ThirdPersonCamera();                            
                        }
                        else
                            Camera = new DebugCamera { Position = Camera.Position };
                        Camera.Initialize();
                    }
                #endif

                float timeStep = (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                DreamWorldPhysicsSystem.CurrentPhysicsSystem.Integrate(timeStep);
                
                Camera.Update(gameTime);
                Level.Update(gameTime);
                if (Level.Skybox != null)
                    Level.Skybox.Update(gameTime); // TODO: This updates the skybox second time around, but we don't want the camera delay.
            }

            if (TutorialText != null)
                TutorialText.Update(gameTime);
            base.Update(gameTime);
        }

        public override void HandleInput()
        {
            PlayerInput playerInput = ScreenManager.InputManager.Player;
            if(playerInput.ShowPauseMenu)
            {
                const string message = "Are you sure you want to return to the main menu?";

                MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);
                confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

                ScreenManager.AddScreen(confirmExitMessageBox);
            }
        }

        private void ConfirmExitMessageBoxAccepted(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new MainMenuScreen());
            ExitScreen();
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.White);
            Level.Draw(gameTime);           
            #if (DEBUG && !XBOX)
            DebugCamera debugCamera = Camera as DebugCamera;
            if (debugCamera != null)
            {
                debugCamera.DrawReticle();
                GraphicsDevice.RenderState.DepthBufferEnable = true;
            }
                debugDrawer.Draw(gameTime);
            #endif

            if (TutorialText != null)            
                TutorialText.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
