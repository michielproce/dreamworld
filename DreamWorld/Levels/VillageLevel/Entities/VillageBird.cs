using System;
using DreamWorld.Entities;
using DreamWorld.Entities.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Levels.VillageLevel.Entities
{
    public class VillageBird : Bird
    {
        public static bool ListInEditor = true;

        protected override float Speed
        {
            get { return .4f; }
        }

        protected override float MaxRotation
        {
            get { return MathHelper.ToRadians(Speed / 1.5f); }
        }

        public override void Initialize()
        {
            Animation.InitialClip = "Flying";
            Animation.Speed = 1.0f;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Model = GameScreen.Content.Load<Model>(@"Models\Village\Bird");
            base.LoadContent();
        }
    }
}
