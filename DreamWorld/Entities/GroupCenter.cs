using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Entities
{
    public class GroupCenter : Entity
    {
        public static bool ListInEditor = true;

        protected override void LoadContent()
        {
            Model = GameScreen.Content.Load<Model>(@"Models\Test\Test");

            base.LoadContent();
        }
    }
}
