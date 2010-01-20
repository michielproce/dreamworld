using DreamWorld.Entities;
using Microsoft.Xna.Framework.Media;

namespace DreamWorld.Levels.VillageLevel
{
    public class VillageLevel : Level
    {
        public float maximumWalkingHeight { get; private set; }

        public override string LevelInformationFileName
        {
            get { return "Village.xml"; }
        }

        public override void Initialize()
        {
            maximumWalkingHeight = -525;
            Skybox = new Skybox("Village") { Name = "Skybox" };
            Terrain = new Terrain("Village") { Name = "Terrain" };

            Song ambient = GameScreen.Content.Load<Song>(@"Audio\Ambient\Village");
            MediaPlayer.Play(ambient);
            MediaPlayer.IsRepeating = true;

            base.Initialize();
        }
    }
}
