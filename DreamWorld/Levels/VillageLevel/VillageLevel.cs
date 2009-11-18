using DreamWorld.Levels.VillageLevel.Entities;

namespace DreamWorld.Levels.VillageLevel
{
    class VillageLevel : Level
    {
        public override void Initialize()
        {
            Skybox = new VillageSkybox();
            AddEntity(Skybox);
            Terrain = new VillageTerrain();
            AddEntity(Terrain);
            base.Initialize();
        }
    }
}
