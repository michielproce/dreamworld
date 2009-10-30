using System.Collections.Generic;
using DreamWorld.Entities;
using Microsoft.Xna.Framework;

namespace DreamWorld.Levels
{
    public class Level : DrawableGameComponent
    {
        public List<Entity> Entities { get; protected set; }

        public Level(Game game)
            : base(game)
        {
            Entities = new List<Entity>();
        }

        public override void Initialize()
        {
            foreach (Entity entity in Entities)            
                entity.Initialize();            
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Entity entity in Entities)
                entity.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Entity entity in Entities)
                entity.Draw(gameTime);
        }
    }
}
