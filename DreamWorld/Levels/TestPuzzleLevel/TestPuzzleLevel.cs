using DreamWorld.Entities;
using DreamWorld.Levels.TestPuzzleLevel.Entities;
using Microsoft.Xna.Framework;

namespace DreamWorld.Levels.TestPuzzleLevel
{
    class TestPuzzleLevel : PuzzleLevel
    {
        public override string LevelInformationFileName
        {
            get { return "TestPuzzel.xml"; }
        }
    }
}