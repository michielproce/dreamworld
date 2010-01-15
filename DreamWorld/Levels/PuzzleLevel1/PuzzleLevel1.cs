using System;
using DreamWorld.Entities;
using DreamWorld.Levels.PuzzleLevel1.Entities;
using Microsoft.Xna.Framework;

namespace DreamWorld.Levels.PuzzleLevel1
{
    class PuzzleLevel1 : PuzzleLevel
    {
        public override string LevelInformationFileName
        {
            get { return "PuzzleLevel1.xml"; }
        }

        public override void Initialize()
        {
            base.Initialize();

            Group group1 = GetGroup(15);
            Group group2 = GetGroup(16);
            Cow cow1 = new Cow { Name = "Cow1" };
            Cow cow2 = new Cow { Name = "Cow2" };
            cow1.Initialize();
            cow2.Initialize();
            cow1.Body.MoveTo(new Vector3(10, 20, 0), Matrix.Identity);
            cow2.Body.MoveTo(new Vector3(50, 20, 0), Matrix.Identity);
            cow1.Group = group1;
            cow2.Group = group2;
            group1.Center = cow2;
            group2.Center = cow1;

            GetGroup(3).AllowedRotations = Vector3.Up;
        }

        public override bool NeedsRespawn()
        {
            return Player.Body.Position.Y < -50;
        }
    }
}