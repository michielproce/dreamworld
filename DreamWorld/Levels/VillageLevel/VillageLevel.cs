using DreamWorld.Levels.VillageLevel.Entities;
using Microsoft.Xna.Framework.Media;

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
            Song ambient = Game.Content.Load<Song>(@"Audio\Ambient\Village");
            MediaPlayer.Play(ambient);
            MediaPlayer.IsRepeating = true;
            base.Initialize();
        }
    }
}
