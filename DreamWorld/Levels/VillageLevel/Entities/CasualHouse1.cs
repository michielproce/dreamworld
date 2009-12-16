using DreamWorld.Entities;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Levels.VillageLevel.Entities
{
    public class CasualHouse1 : Entity
    {
        public static bool ListInEditor = true;
        protected override void LoadContent()
        {
            Model = GameScreen.Content.Load<Model>(@"Models\Village\CasualHouse1");
            base.LoadContent();
        }
    }
}
