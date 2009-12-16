using System;
using DreamWorld.Entities;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Levels.VillageLevel.Entities
{
    public class MorwirsHouse : Entity
    {
        public static bool ListInEditor = true;
        protected override void LoadContent()
        {
            Model = GameScreen.Content.Load<Model>(@"Models\Village\MorwirsHouse");
            base.LoadContent();
        }
    }
}
