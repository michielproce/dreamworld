using DreamWorld.Levels.Entities;
using Microsoft.Xna.Framework;

namespace DreamWorld.Levels
{
    class TestLevel : Level
    {
        public TestLevel(Game game) : base(game)
        {
            Entities.Add(new TestBall(game));
        }
    }
}
