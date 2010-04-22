using DreamWorld.Entities;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Levels.VillageLevel.Entities
{
    public class StableTrashed : Entity
    {
        public static bool ListInEditor = true;
        protected override void LoadContent()
        {
            Model = GameScreen.Content.Load<Model>(@"Models\Village\StableTrashed");
            base.LoadContent();
        }
    }
}
