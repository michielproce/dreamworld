using System;
using DreamWorld.Entities;
using DreamWorld.Levels.TestPuzzleLevel.Entities;
using JigLibX.Physics;
using Microsoft.Xna.Framework;

namespace DreamWorld.Levels.TestPuzzleLevel
{
    class TestPuzzleLevel : PuzzleLevel
    {
        public override void Initialize()
        {
            Groups = new Group[3];
            Groups[0] = new Group();
            Groups[1] = new Group();
            Groups[2] = new Group();

            Element element = new TestElement();
            Element element2 = new TestElement{ Scale = new Vector3(1,2,1)};
            Groups[0].AddElement(element);
            Groups[0].AddElement(element2);


            Element element3 = new TestElement { Scale = new Vector3(3, 3, 3) };
            Groups[1].AddElement(element3);

            base.Initialize();

            Groups[0].Body.MoveTo(new Vector3(0, -20, 5f), Matrix.Identity);
            element.Body.MoveTo(new Vector3(0, -40, 0), Matrix.Identity);
            element2.Body.MoveTo(new Vector3(-10, -40, 10), Matrix.Identity);
            element3.Body.MoveTo(new Vector3(0, -40, -30), Matrix.Identity);
        }

        public override string LevelInformationFileName
        {
            get { return "TestPuzzel.xml"; }
        }
    }
}