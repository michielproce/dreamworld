using System;
using DreamWorld.Entities;
using DreamWorld.Levels.PuzzleLevel1.Entities;
using Microsoft.Xna.Framework;

namespace DreamWorld.Levels.PuzzleLevel1
{
    class PuzzleLevel1 : PuzzleLevel
    {
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

            Cows = new Cow[2];
            Cows[0] = new Cow { Name = "Cow1" };
            Cows[1] = new Cow { Name = "Cow2" };

            Cows[0].Initialize();
            Cows[1].Initialize();
            Cows[0].Body.MoveTo(new Vector3(210, 28, -20), Matrix.Identity);
            Cows[1].Body.MoveTo(new Vector3(270, 28, -20), Matrix.Identity);

            Cows[0].Group = group1;
            Cows[1].Group = group2;
            group1.Center = Cows[1];
            group2.Center = Cows[0];
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
    }
}