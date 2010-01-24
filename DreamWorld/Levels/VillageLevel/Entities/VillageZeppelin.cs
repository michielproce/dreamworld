using DreamWorld.Entities.Global;
using DreamWorld.Util;
using Microsoft.Xna.Framework;

namespace DreamWorld.Levels.VillageLevel.Entities
{
    public class VillageZeppelin : Zeppelin
    {
        public static bool ListInEditor = true;

        private Curve3D path;
        
        public VillageZeppelin()
        {
            path = new Curve3D(CurveLoopType.Cycle);
            path.AddPoint(new Vector3(0, 0, -200));
            path.AddPoint(new Vector3(500, -100, -1100));
            path.AddPoint(new Vector3(-500, -30, -600));
            path.AddPoint(new Vector3(-300, -10, 0));
            path.AddPoint(new Vector3(0, 0, -200));
            path.SetTangents();
        }

        protected override float Speed
        {
            get { return .7f; }
        }

        protected override Curve3D Path
        {
            get { return path; }
        }
    }
}
