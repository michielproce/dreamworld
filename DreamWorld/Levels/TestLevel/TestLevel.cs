using DreamWorld.Levels.TestLevel.Entities;

namespace DreamWorld.Levels.TestLevel
{
    class TestLevel : Level
    {
        public TestLevel()
        {
        }

        public override void Initialize()
        {
            AddEntity(new TestBall());
            base.Initialize();
        }
    }
}
