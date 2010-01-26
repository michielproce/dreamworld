using System;
using DreamWorld.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Levels.VillageLevel.Entities
{
    public class Morwir : Entity
    {
        public static bool ListInEditor = true;

        public override void Initialize()
        {
            Animation.InitialClip = "Standing";
            Animation.Speed = 1.0f;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Model = GameScreen.Content.Load<Model>(@"Models\Village\Morwir");
            base.LoadContent();
        }
    }
}
