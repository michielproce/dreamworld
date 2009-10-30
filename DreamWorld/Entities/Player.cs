using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Entities
{
    public class Player : Entity
    {
        public Player(Game game) : base(game)
        {
        }

        protected override void LoadContent()
        {
            Model = Game.Content.Load<Model>(@"Models\ball");
            base.LoadContent();
        }
    }
}
