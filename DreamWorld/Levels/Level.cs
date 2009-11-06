using System.Collections.Generic;
using DreamWorld.Entities;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;

namespace DreamWorld.Levels
{
    public abstract class Level
    {
        public GameScreen GameScreen { protected get; set; }
        protected Player Player { get; set; }
        
        private List<Entity> entities;            

        protected Level()
        {
            entities = new List<Entity>();
//            Player = new Player();
        }

        protected void AddEntity(Entity entity)
        {
            entity.Camera = GameScreen.CurrentCamera;
            entity.Game = GameScreen.ScreenManager.Game;
            entity.Effect = GameScreen.DefaultEffect;
            entities.Add(entity);
        }

        public virtual void Initialize()
        {
//            AddEntity(Player);
            foreach (Entity entity in entities)
                entity.Initialize();
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (Entity entity in entities)
                entity.Update(gameTime);
        }
        public virtual void Draw(GameTime gameTime)
        {
            foreach (Entity entity in entities)
                entity.Draw(gameTime);
        }
    }
}
