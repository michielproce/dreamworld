using System;
using DreamWorld.Audio;
using DreamWorld.Cameras;
using DreamWorld.Helpers.Renderers;
using DreamWorld.InputManagement;
using DreamWorld.InputManagement.Types;
using DreamWorld.Interface.Help;
using DreamWorld.Levels;
using DreamWorld.Levels.TutorialLevel;
using DreamWorld.Levels.VillageLevel;
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
        private DreamWorldPhysicsSystem PhysicsSystem { get; set; }
        public HelpSystem HelpSystem { get; private set; }
        
        public VoiceOver VoiceOver { get; set; }

        private bool _crosshairVisible;
        private Texture2D _crosshairTexture;
        private Vector2 _crosshairPosition;

        private SpriteFont _subtitleFont;

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

            _crosshairVisible = Level is PuzzleLevel;
            _crosshairTexture = Content.Load<Texture2D>(@"Textures\Interface\crosshair");
            _crosshairPosition = new Vector2(GraphicsDevice.Viewport.Width / 2 - _crosshairTexture.Width / 2,
                GraphicsDevice.Viewport.Height / 2 - _crosshairTexture.Height / 2);
            
            _subtitleFont = Content.Load<SpriteFont>(@"Fonts\subtitle");
            
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
                    if (InputManager.Debug.ToggleDebugCameraReticle)
                        _crosshairVisible = !_crosshairVisible;

                    if (((DreamWorldGame)ScreenManager.Game).InputManager.Debug.ToggleDebugCamera)
                    {
                        if (Camera is DebugCamera)
                        {
                            _crosshairVisible = Level is PuzzleLevel;
                            ((DebugCamera)Camera).DisposeForm();
                            Camera = new ThirdPersonCamera();                            
                        }
                        else 
                        {
                            _crosshairVisible = true;
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
                        _crosshairVisible = false;
                        Camera = new OverviewCamera
                        {
                            TargetPosition = Level.OverviewPosition,
                            TargetLookat = Level.OverviewLookat,
                            OldCamera = Camera as ThirdPersonCamera,
                            Player = Level.Player
                        };
                        Camera.Initialize();
                    }
                    else if (!((DreamWorldGame)ScreenManager.Game).InputManager.Player.ShowOverview && Camera is OverviewCamera)
                    {
                        OverviewCamera overviewCamera = (OverviewCamera)Camera;
                        overviewCamera.IsExitting = true;

                        if (overviewCamera.Transition == 0)
                        {
                            Camera = overviewCamera.OldCamera;
                            if (Level is PuzzleLevel)
                                _crosshairVisible = true;
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
                if (Level is TutorialLevel)
                {
                    const string message = "Are you sure you want to skip";

                    MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);
                    confirmExitMessageBox.Accepted += ConfirmExitTutorialMessageBoxAccepted;

                    ScreenManager.AddScreen(confirmExitMessageBox);
                }
                else
                {
                    const string message = "Are you sure you want to exit";

                    MessageBoxScreen confirmExitToMainMenuMessageBox = new MessageBoxScreen(message);
                    confirmExitToMainMenuMessageBox.Accepted += ConfirmExitToMainMenuMessageBoxAccepted;

                    ScreenManager.AddScreen(confirmExitToMainMenuMessageBox);
                }
            }
        }

        public override void HandleExit()
        {
            MediaPlayer.Stop();
        }

        private void ConfirmExitToMainMenuMessageBoxAccepted(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new MainMenuScreen());
            ExitScreen();
        }

        private void ConfirmExitTutorialMessageBoxAccepted(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new GameScreen(new VillageLevel(VillageLevel.Stage.FinishedTutorial)));
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
                    string text = StringUtil.CutLine(vp, _subtitleFont, VoiceOver.Text, 0.9f);
                    Vector2 textSize = _subtitleFont.MeasureString(text);
                    Vector2 textPosition = new Vector2(vp.Width / 2f - textSize.X / 2f, vp.Height - textSize.Y - 30f);

                    ScreenManager.SpriteBatch.Begin();
                    ScreenManager.SpriteBatch.DrawString(_subtitleFont, text, textPosition + new Vector2(2), Color.Black);
                    ScreenManager.SpriteBatch.DrawString(_subtitleFont, text, textPosition, Color.White);
                    ScreenManager.SpriteBatch.End();
                }
            }

            if (_crosshairVisible)
            {
                ScreenManager.SpriteBatch.Begin();
                ScreenManager.SpriteBatch.Draw(_crosshairTexture, _crosshairPosition, Color.White);
                ScreenManager.SpriteBatch.End();
            }
                        
            base.Draw(gameTime);
        }
    }
}
