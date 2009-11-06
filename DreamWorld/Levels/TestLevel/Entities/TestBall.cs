using DreamWorld.Entities;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Levels.TestLevel.Entities
{
    class TestBall : Entity
    {
        protected override void LoadContent()
        {
            Model = Game.Content.Load<Model>(@"Models\Test\Ball");
            base.LoadContent();
        }
    }
}
