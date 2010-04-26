using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Entities.Global
{
    public class HelpSign : Entity
    {
        public static bool ListInEditor = true;
        protected override void LoadContent()
        {
            Model = GameScreen.Content.Load<Model>(@"Models\Global\HelpSign");
            base.LoadContent();
        }
    }
}
