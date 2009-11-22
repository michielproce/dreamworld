using System;
using DreamWorld.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Levels.VillageLevel.Entities
{
    public class TestEntity : Entity
    {
        protected override void LoadContent()
        {
            Model = GameScreen.Content.Load<Model>(@"Models\Test\Test");                    
            
            Position = new Vector3(0, -450, -100);
            
            base.LoadContent();
        }
    }
}
