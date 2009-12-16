using System;
using DreamWorld.Entities;
using DreamWorld.Entities.Global;
using DreamWorld.Levels.VillageLevel.Entities;
using DreamWorld.Util;
using Microsoft.Xna.Framework;
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
            Skybox = new Skybox("Village");
            AddEntity("skybox", Skybox);

            Song ambient = GameScreen.Content.Load<Song>(@"Audio\Ambient\Village");
            MediaPlayer.Play(ambient);
            MediaPlayer.IsRepeating = true;

            base.Initialize();
        }

        protected override void InitializeSpecialEntities()
        {

            Terrain = new Terrain("Village");
            AddEntity("terrain", Terrain);
        }
    }
}
