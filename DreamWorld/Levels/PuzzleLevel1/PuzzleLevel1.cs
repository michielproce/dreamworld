using System;
using DreamWorld.Entities;
using DreamWorld.Levels.PuzzleLevel1.Entities;
using Microsoft.Xna.Framework;

namespace DreamWorld.Levels.PuzzleLevel1
{
    class PuzzleLevel1 : PuzzleLevel
    {
        private bool initialTutorialShown;

        private Cow[] Cows;

        public override string LevelInformationFileName
        {
            get { return "PuzzleLevel1.xml"; }
        }

        public override void Initialize()
        {
            base.Initialize();

            Group group1 = new CowGroup();
            Group group2 = new CowGroup();
            SetGroup(group1, 15);
            SetGroup(group2, 16);

            Cows = new Cow[4];
            Cows[0] = new Cow { Name = "Cow1", Scale = new Vector3(0.3f) };
            Cows[1] = new Cow { Name = "Cow2", Scale = new Vector3(0.3f) };

            Cows[0].Initialize();
            Cows[1].Initialize();
            Cows[0].Body.MoveTo(new Vector3(200, 30, -20), Matrix.Identity);
            Cows[1].Body.MoveTo(new Vector3(260, 30, -20), Matrix.Identity);

            Cows[0].Group = group1;
            Cows[1].Group = group2;
            group1.Center = Cows[1];
            group2.Center = Cows[0];

            Group group3 = new CowGroup();
            Group group4 = new CowGroup();
            SetGroup(group3, 17);
            SetGroup(group4, 18);

            Cows[2] = new Cow { Name = "Cow3", Scale = new Vector3(0.3f) };
            Cows[3] = new Cow { Name = "Cow4", Scale = new Vector3(0.3f) };

            Cows[2].Initialize();
            Cows[3].Initialize();
            Cows[2].Body.MoveTo(new Vector3(160, 30, -40), Matrix.Identity);
            Cows[3].Body.MoveTo(new Vector3(100, 30, -40), Matrix.Identity);

            Cows[2].Group = group3;
            Cows[3].Group = group4;
            group3.Center = Cows[3];
            group4.Center = Cows[2];
        }

        protected override bool GameIsLost()
        {
            if (Player.Body.Position.Y < -50)
                return true;

            foreach (Cow cow in Cows)
            {
                if(cow.Body.Position.Y < -25)
                    return true;
            }

            return false;
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
            if (!initialTutorialShown && GameScreen.TutorialText != null)
            {
                GameScreen.TutorialText.SetText(
                    "Use the shoulder buttons to select groups. Use the right trigger and the thumbsticks to rotate the groups. Find your way to get the cows to the other side.",
                    gameTime.TotalGameTime + TimeSpan.FromSeconds(10));
                initialTutorialShown = true;
            }
            base.Update(gameTime);
        }
    }
}