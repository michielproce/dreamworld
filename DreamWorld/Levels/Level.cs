using System;
using System.Collections.Generic;
using DreamWorld.Entities;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;

namespace DreamWorld.Levels
{
    public abstract class Level
    {
        public GameScreen GameScreen { get; set; }
        public Terrain Terrain { get; protected set; }

        public Player Player { get; private set; }
        
        private List<Entity> entities;            

        protected Level()
        {
            entities = new List<Entity>();
            
        }

        protected void AddEntity(Entity entity)
        {
            entity.Level = this;
            entity.Camera = GameScreen.CurrentCamera;
            entity.Game = GameScreen.ScreenManager.Game;
            entity.Effect = GameScreen.DefaultEffect;            
            entities.Add(entity);
        }

        public virtual void Initialize()
        {
            Player = new Player
                         {
                             InputManager = ((DreamWorldGame) GameScreen.ScreenManager.Game).InputManager
                         };
            AddEntity(Player);
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
