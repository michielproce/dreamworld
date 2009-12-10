using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DreamWorld.Entities;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Levels.VillageLevel.Entities
{
    public class WallEntrance : Entity
    {
        public static bool ListInEditor = true;
        protected override void LoadContent()
        {
            Model = GameScreen.Content.Load<Model>(@"Models\Village\WallEntrance");
            base.LoadContent();
        }
    }
}
