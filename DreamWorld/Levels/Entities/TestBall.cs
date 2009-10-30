using System;
using DreamWorld.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Levels.Entities
{
    class TestBall : Entity
    {
        public TestBall(Game game) : base(game)
        {
            
        }

        public override void Initialize()
        {
            Position = new Vector3(0, 0, -5);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Model = Game.Content.Load<Model>(@"Models\ball");
            base.LoadContent();
        }
    }
}
