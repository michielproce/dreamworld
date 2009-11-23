using DreamWorld.Entities;
using DreamWorld.Entities.Global;
using DreamWorld.Levels.VillageLevel.Entities;
using DreamWorld.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace DreamWorld.Levels.VillageLevel
{
    class VillageLevel : Level
    {
        public override void Initialize()
        {
            Skybox = new Skybox("Village");
            AddEntity("skybox", Skybox);
            Terrain = new Terrain("Village");
            AddEntity("terrain", Terrain);

            AddEntity("test", new TestEntity());

            Curve3D zeppelinPath = new Curve3D(CurveLoopType.Cycle);            
            zeppelinPath.AddPoint(new Vector3(0, -350, -300));
            zeppelinPath.AddPoint(new Vector3(500, -350, -900));
            zeppelinPath.AddPoint(new Vector3(-500, -350, -600));
            zeppelinPath.AddPoint(new Vector3(-300, -350, 0));
            zeppelinPath.AddPoint(new Vector3(0, -350, -300));
            zeppelinPath.SetTangents();

            Zeppelin zeppelin = new Zeppelin
                                    {
                                        Path = zeppelinPath,
                                        Speed = .0005f
                                    };
            AddEntity("zeppelin", zeppelin);
            Song ambient = GameScreen.Content.Load<Song>(@"Audio\Ambient\Village");
            MediaPlayer.Play(ambient);
            MediaPlayer.IsRepeating = true;
            base.Initialize();
        }
    }
}
