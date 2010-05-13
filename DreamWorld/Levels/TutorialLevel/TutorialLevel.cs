using System;
using DreamWorld.Cameras;
using DreamWorld.Entities;
using DreamWorld.Rendering.Postprocessing;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DreamWorld.Levels.TutorialLevel
{
    class TutorialLevel : PuzzleLevel
    {
        // True if the player is reading the last sign
        private bool _readingLastSign;

        public TutorialLevel()
        {
            LoadingColor = Color.White;   
        }

        public override string LevelInformationFileName
        {
            get { return "TutorialLevel.xml"; }
        }

        public override void Initialize()
        {
            MediaPlayer.Play(GameScreen.Content.Load<Song>(@"Audio\Music\Tutorial"));

            Skybox = new Skybox("Tutorial") { Name = "Skybox" };

            base.Initialize();

            OverviewPosition = new Vector3(146.2f, 117.2f, -328.4f);
            OverviewLookat = new Vector3(146.7f, 116.7f, -327.8f);

            ThirdPersonCamera cam = GameScreen.Camera as ThirdPersonCamera;
            if (cam != null)
                cam.VerticalRotation = MathHelper.ToRadians(-15);     

            Hud.Hidden = true;

            GameScreen.TransitionOnTime = TimeSpan.FromSeconds(3);            
        }

        public override void Update(GameTime gameTime)
        {
            if (Player.Body.Position.Y < -50)
                Player.Respawn();

            if (GameScreen.HelpSystem.Helper != null)
            {
                if (GameScreen.HelpSystem.Helper.Name == "tutorialSign03" && GameScreen.HelpSystem.ScreenActive)
                    Hud.Hidden = false;

                if (GameScreen.HelpSystem.Helper.Name == "tutorialSign11" && GameScreen.HelpSystem.ScreenActive)
                    _readingLastSign = true;
            }

            // Update for bloom
            float intensity = 1 - GameScreen.TransitionAlpha / 255f;
            bloom.BaseIntensity = 1f + intensity * 6f;
            bloom.BloomIntensity = 1f + intensity * 6f;
            const float sat = 4f;
            bloom.BaseSaturation = sat - intensity * sat;
            bloom.BloomSaturation = sat - intensity * sat;
            base.Update(gameTime);
        }

        protected override void InitBloom(ref Bloom bloom)
        {
            bloom.BloomThreshold = .3f;
            bloom.BlurAmount = 8f;
            bloom.BloomIntensity = 1f;
            bloom.BaseIntensity = 1f;
            bloom.BloomSaturation = 2f;
            bloom.BaseSaturation = 2f;
        }

        protected override bool GameIsWon()
        {
#if(DEBUG)
            if (GameScreen.InputManager.Keyboard.NewlyPressed(Keys.OemTilde))
                return true;
#endif
            // The game is won when the player is done reading the last sign.
            return _readingLastSign && GameScreen.HelpSystem.Helper != null && !GameScreen.HelpSystem.ScreenActive;
        }

        protected override void VictoryEventHandler()
        {
            if (!GameScreen.IsExiting)
            {                
                GameScreen.ExitScreen();
                GameScreen.ScreenManager.AddScreen(new GameScreen(new VillageLevel.VillageLevel(VillageLevel.VillageLevel.Stage.FinishedTutorial)));
            }
        }
    }
}