using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Entities
{
    public class GroupCenter : Entity
    {
        public static bool ListInEditor = true;

        protected override void LoadContent()
        {
            // Don't load a model if a deriving class already loaded one
            if(Model == null)
                Model = GameScreen.Content.Load<Model>(@"Models\Puzzle\GroupCenter");

            base.LoadContent();
        }
    }
}
