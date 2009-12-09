using System;
using DreamWorld.Entities;
using DreamWorld.Levels.TestPuzzleLevel.Entities;
using Microsoft.Xna.Framework;

namespace DreamWorld.Levels.TestPuzzleLevel
{
    class TestPuzzleLevel : PuzzleLevel
    {
        public override void Initialize()
        {
            Groups = new Group[3];
            Groups[0] = new Group
            { 
                Position = new Vector3(0, -200, -500f), 
            };
            Groups[1] = new Group();
            Groups[2] = new Group();


            Element element = new TestElement
            {
                Position = new Vector3(0, -20, 0),
            };
            Groups[0].AddElement(element);

            base.Initialize();
        }

        public override string LevelInformationFileName
        {
            get { return "testpuzzellevel.xml"; } // Doesn't exist (yet), results in empty LevelInformation
        }
    }
}