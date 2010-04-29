using System;
using DreamWorld.Cameras;
using DreamWorld.Entities;
using DreamWorld.Levels.PuzzleLevel1.Entities;
using DreamWorld.Rendering.Postprocessing;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DreamWorld.Levels.PuzzleLevel1
{
    class PuzzleLevel1 : PuzzleLevel
    {
        private Cow[] _cows;

        public PuzzleLevel1()
        {
            LoadingColor = Color.White;   
        }

        public override string LevelInformationFileName
        {
            get { return "PuzzleLevel1.xml"; }
        }

        public override void Initialize()
        {
            GameScreen.TransitionOnTime = TimeSpan.FromSeconds(3);

            MediaPlayer.Play(GameScreen.Content.Load<Song>(@"Audio\Music\Puzzle1"));

            Skybox = new Skybox("Puzzle") { Name = "Skybox" };

            Group group1 = new CowGroup();
            Group group2 = new CowGroup();
            Group group3 = new CowGroup();
            Group group4 = new CowGroup();

            SetGroup(group1, 15);
            SetGroup(group2, 16);
            SetGroup(group3, 17);
            SetGroup(group4, 18);

            base.Initialize();

            ThirdPersonCamera cam = GameScreen.Camera as ThirdPersonCamera;
            if (cam != null)
                cam.VerticalRotation = MathHelper.ToRadians(-15);            

            _cows = new Cow[4];
            _cows[0] = new Cow { Name = "Cow1", Scale = new Vector3(0.4f), startPosition = new Vector3(220, 30, -20) };
            _cows[1] = new Cow { Name = "Cow2", Scale = new Vector3(0.4f), startPosition = new Vector3(280, 30, -20) };

            _cows[0].Group = group1;
            _cows[1].Group = group2;
            group1.Center = _cows[1];
            group2.Center = _cows[0];

            _cows[0].Initialize();
            _cows[1].Initialize();

            _cows[2] = new Cow { Name = "Cow3", Scale = new Vector3(0.4f), startPosition = new Vector3(160, 30, -40) };
            _cows[3] = new Cow { Name = "Cow4", Scale = new Vector3(0.4f), startPosition = new Vector3(100, 30, -40) };

            _cows[2].Group = group3;
            _cows[3].Group = group4;
            group3.Center = _cows[3];
            group4.Center = _cows[2];

            _cows[2].Initialize();
            _cows[3].Initialize();
        }

        protected override bool GameIsWon()
        {
            #if(DEBUG)
            if (GameScreen.InputManager.Keyboard.NewlyPressed(Keys.OemTilde))
                return true;
            #endif

            foreach (Cow cow in _cows)
            {
                if(cow.Body.Position.Z < 260 || cow.Group.IsRotating || !cow.Group.IsColliding)
                    return false;
            }
            return true;
        }

        protected override void VictoryEventHandler()
        {
            Level villageLevel = new VillageLevel.VillageLevel(VillageLevel.VillageLevel.Stage.FINISHED_PUZZLE1);
            GameScreen.ScreenManager.AddScreen(new GameScreen(villageLevel));
            GameScreen.ExitScreen();
        }

        public override void Update(GameTime gameTime)
        {
            if (Player.Body.Position.Y < -50)
                Player.Respawn();

            foreach (Cow cow in _cows)
            {
                if (cow.Body.Position.Y < -25 || Vector3.Distance(cow.Body.Position, cow.Group.Center.Body.Position) < 10)
                {
                    cow.Respawn();
                    if (cow.Group.Center is Cow)
                        ((Cow)cow.Group.Center).Respawn();
                }
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

        public override void InitBloom(ref Bloom bloom)
        {
            bloom.BloomThreshold = .3f;
            bloom.BlurAmount = 8f;
            bloom.BloomIntensity = 1f;
            bloom.BaseIntensity = 1f;
            bloom.BloomSaturation = 2f;
            bloom.BaseSaturation = 2f;
        }
    }
}