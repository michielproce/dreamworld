using DreamWorld.Entities;
using Microsoft.Xna.Framework.Media;

namespace DreamWorld.Levels.VillageLevel
{
    public class VillageLevel : Level
    {
        public override string LevelInformationFileName
        {
            get { return "Village.xml"; }
        }

        public override void Initialize()
        {
            Skybox = new Skybox("Village") { Name = "Skybox" };

            Song ambient = GameScreen.Content.Load<Song>(@"Audio\Ambient\Village");
            MediaPlayer.Play(ambient);
            MediaPlayer.IsRepeating = true;

            base.Initialize();
        }

        protected override void InitializeSpecialEntities()
        {
            Terrain = new Terrain("Village") { Name = "Terrain" };
            Terrain.Spawn();
        }
    }
}
