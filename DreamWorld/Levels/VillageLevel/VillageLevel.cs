using DreamWorld.Levels.VillageLevel.Entities;

namespace DreamWorld.Levels.VillageLevel
{
    class VillageLevel : Level
    {
        public override void Initialize()
        {
            AddEntity(new VillageTerrain());
            base.Initialize();
        }
    }
}
