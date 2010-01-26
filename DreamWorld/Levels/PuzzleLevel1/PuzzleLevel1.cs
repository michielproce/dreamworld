using System;
using DreamWorld.Entities;
using DreamWorld.Levels.PuzzleLevel1.Entities;
using DreamWorld.Rendering.Postprocessing;
using Microsoft.Xna.Framework;

namespace DreamWorld.Levels.PuzzleLevel1
{
    class PuzzleLevel1 : PuzzleLevel
    {
        private bool initialTutorialShown;

        private Cow[] Cows;

        public PuzzleLevel1()
        {
            
        }

        public override string LevelInformationFileName
        {
            get { return "PuzzleLevel1.xml"; }
        }

        public override void Initialize()
        {
            base.Initialize();

            GameScreen.TransitionOnTime = TimeSpan.FromSeconds(3);

            Group group1 = new CowGroup();
            Group group2 = new CowGroup();
            SetGroup(group1, 15);
            SetGroup(group2, 16);

            Cows = new Cow[4];
            Cows[0] = new Cow { Name = "Cow1", Scale = new Vector3(0.3f), startPosition = new Vector3(200, 30, -20) };
            Cows[1] = new Cow { Name = "Cow2", Scale = new Vector3(0.3f), startPosition = new Vector3(260, 30, -20) };

            Cows[0].Initialize();
            Cows[1].Initialize();

            Cows[0].Group = group1;
            Cows[1].Group = group2;
            group1.Center = Cows[1];
            group2.Center = Cows[0];

            Group group3 = new CowGroup();
            Group group4 = new CowGroup();
            SetGroup(group3, 17);
            SetGroup(group4, 18);

            Cows[2] = new Cow { Name = "Cow3", Scale = new Vector3(0.3f), startPosition = new Vector3(160, 30, -40) };
            Cows[3] = new Cow { Name = "Cow4", Scale = new Vector3(0.3f), startPosition = new Vector3(100, 30, -40) };

            Cows[2].Initialize();
            Cows[3].Initialize();

            Cows[2].Group = group3;
            Cows[3].Group = group4;
            group3.Center = Cows[3];
            group4.Center = Cows[2];
        }

        protected override bool GameIsWon()
        {
            foreach (Cow cow in Cows)
            {
                if(cow.Body.Position.Z < 410 || cow.Group.IsRotating || !cow.Group.IsColliding)
                    return false;
            }
            return true;
        }

        public override void Update(GameTime gameTime)
        {
            if (Player.Body.Position.Y < -50)
                Player.Respawn();

            foreach (Cow cow in Cows)
            {
                if (cow.Body.Position.Y < -25 || Vector3.Distance(cow.Body.Position, cow.Group.Center.Body.Position) < 10)
                {
                    cow.Respawn();
                    if (cow.Group.Center is Cow)
                        ((Cow)cow.Group.Center).Respawn();
                }
            }

            if (!initialTutorialShown && GameScreen.TutorialText != null)
            {
                GameScreen.TutorialText.SetText(
                    "Use the shoulder buttons to select groups. Use the right trigger and the thumbsticks to rotate the groups. Find your way to get the cows to the other side.",
                    gameTime.TotalGameTime + TimeSpan.FromSeconds(15));
                initialTutorialShown = true;
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