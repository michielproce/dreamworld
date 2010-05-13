using System;
using DreamWorld.Entities.Global;
using DreamWorld.Util;
using Microsoft.Xna.Framework;

namespace DreamWorld.Levels.VillageLevel.Entities
{
    public class VillageZeppelin : Zeppelin
    {
        public static bool ListInEditor = true;

        private readonly Curve3D _path;
        
        public VillageZeppelin()
        {
            _path = new Curve3D(CurveLoopType.Cycle);
            _path.AddPoint(new Vector3(0, 0, -200));
            _path.AddPoint(new Vector3(500, -100, -1100));
            _path.AddPoint(new Vector3(-500, -30, -600));
            _path.AddPoint(new Vector3(-300, -10, 0));
            _path.AddPoint(new Vector3(0, 0, -200));                       
            _path.SetTangents();
        }

        public override void Initialize()
        {
            // I already created the perfect path.. We're boosting it initially so it comes into frame in the first second you enter the village.
            FastForward(450);
            base.Initialize();
        }

        protected override float Speed
        {
            get { return .7f; }
        }

        protected override Curve3D Path
        {
            get { return _path; }
        }
    }
}
