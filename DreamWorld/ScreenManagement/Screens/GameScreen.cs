using System;
using DreamWorld.Audio;
using DreamWorld.Cameras;
using DreamWorld.Entities;
using DreamWorld.Helpers.Renderers;
using DreamWorld.InputManagement;
using DreamWorld.InputManagement.Types;
using DreamWorld.Interface.Help;
using DreamWorld.Levels;
using DreamWorld.Util;
using JigLibX.Collision;
using JigLibX.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace DreamWorld.ScreenManagement.Screens
{
    public class GameScreen : Screen
    {
        public static GameScreen Instance { get; private set; }
        public Camera Camera { get; private set; }
        public Level Level { get; private set; }
        public InputManager InputManager { get; private set; }
        public GraphicsDevice GraphicsDevice { get; private set; }
        public DebugDrawer debugDrawer;
        public DreamWorldPhysicsSystem PhysicsSystem { get; private set; }
        public HelpSystem HelpSystem { get; private set; }
        
        public VoiceOver VoiceOver { get; set; }

        private bool crosshairVisible;
        private Texture2D crosshairTexture;
        private Vector2 crosshairPosition;

        private SpriteFont subtitleFont;

        public GameScreen(Level level)
        {
            Instance = this;
            LoadingScreen = new LoadingScreen(level.LoadingColor);
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

            HelpSystem = new HelpSystem(this);

            crosshairVisible = Level is PuzzleLevel;
            crosshairTexture = Content.Load<Texture2D>(@"Textures\Interface\crosshair");
            crosshairPosition = new Vector2(GraphicsDevice.Viewport.Width / 2 - crosshairTexture.Width / 2,
                GraphicsDevice.Viewport.Height / 2 - crosshairTexture.Height / 2);
            
            subtitleFont = Content.Load<SpriteFont>(@"Fonts\subtitle");
            
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
                            crosshairVisible = Level is PuzzleLevel;
                            ((DebugCamera)Camera).DisposeForm();
                            Camera = new ThirdPersonCamera();                            
                        }
                        else 
                        {
                            crosshairVisible = true;
                            Camera = new DebugCamera { Position = Camera.Position };
                        }
                        Camera.Initialize();
                    }
#endif

                float timeStep = (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                DreamWorldPhysicsSystem.CurrentPhysicsSystem.Integrate(timeStep);
                               
                HelpSystem.Update(gameTime);

                if(!(Camera is DebugCamera))
                {
                    if (((DreamWorldGame)ScreenManager.Game).InputManager.Player.ShowOverview && Camera is ThirdPersonCamera)
                    {
                        crosshairVisible = false;
                        Camera = new OverviewCamera
                        {
                            targetPosition = Level.overviewPosition,
                            targetLookat = Level.overviewLookat,
                            oldCamera = Camera as ThirdPersonCamera,
                            player = Level.Player
                        };
                        Camera.Initialize();
                    }
                    else if (!((DreamWorldGame)ScreenManager.Game).InputManager.Player.ShowOverview && Camera is OverviewCamera)
                    {
                        OverviewCamera overviewCamera = (OverviewCamera)Camera;
                        overviewCamera.isExitting = true;

                        if (overviewCamera.transition == 0)
                        {
                            Camera = overviewCamera.oldCamera;
                            if (Level is PuzzleLevel)
                                crosshairVisible = true;
                        }
                    }
                }


                Level.Update(gameTime);
                Camera.Update(gameTime);

                if (Level.Skybox != null)
                    Level.Skybox.Update(gameTime); // TODO: This updates the skybox second time around, but we don't want the camera delay.
            }

            base.Update(gameTime);
        }

        public override void HandleInput()
        {
            PlayerInput playerInput = ScreenManager.InputManager.Player;
            if(playerInput.ShowPauseMenu)
            {
                const string message = "Are you sure you want to return to the main menu";

                MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);
                confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

                ScreenManager.AddScreen(confirmExitMessageBox);
            }
        }

        public override void HandleExit()
        {
            MediaPlayer.Stop();
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
            HelpSystem.Draw(gameTime);
            
            if(VoiceOver != null)
            {
                if (!VoiceOver.Started)
                {
                    VoiceOver.Audio.Play();
                    VoiceOver.Started = true;
                }

                if(VoiceOver.Audio.State == SoundState.Playing && ((DreamWorldGame) ScreenManager.Game).Config.Subtitles)
                {
                    Viewport vp = GraphicsDevice.Viewport;
                    string text = StringUtil.CutLine(vp, subtitleFont, VoiceOver.Text, 0.9f);
                    Vector2 textSize = subtitleFont.MeasureString(text);
                    Vector2 textPosition = new Vector2(vp.Width / 2f - textSize.X / 2f, vp.Height - textSize.Y - 30f);

                    ScreenManager.SpriteBatch.Begin();
                    ScreenManager.SpriteBatch.DrawString(subtitleFont, text, textPosition + new Vector2(2), Color.Black);
                    ScreenManager.SpriteBatch.DrawString(subtitleFont, text, textPosition, Color.White);
                    ScreenManager.SpriteBatch.End();
                }
            }

            if (crosshairVisible)
            {
                ScreenManager.SpriteBatch.Begin();
                ScreenManager.SpriteBatch.Draw(crosshairTexture, crosshairPosition, Color.White);
                ScreenManager.SpriteBatch.End();
            }
                        
            base.Draw(gameTime);
        }
    }
}
