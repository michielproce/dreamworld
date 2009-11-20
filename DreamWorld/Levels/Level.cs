using System;
using System.Collections.Generic;
using DreamWorld.Entities;
using DreamWorld.Rendering.Postprocessing;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Levels
{
    public abstract class Level
    {
        public DreamWorldGame Game { protected get; set; }
        public GameScreen GameScreen { get; set; }
        public Terrain Terrain { get; protected set; }
        public Skybox Skybox { get; protected set; }

        public Player Player { get; private set; }
        
        private List<Entity> entities;

        private SpriteBatch spriteBatch;
        private Bloom bloom;
        private EdgeDetection edgeDetection;

        protected Level()
        {
            entities = new List<Entity>();
            
        }


        protected void AddEntity(Entity entity)
        {
            entity.Level = this;
            entity.GameScreen = GameScreen;
            entity.Game = GameScreen.ScreenManager.Game as DreamWorldGame;
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

            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            bloom = new Bloom(GameScreen.ScreenManager.Game, spriteBatch);
            edgeDetection = new EdgeDetection(GameScreen.ScreenManager.Game, spriteBatch);

        }


        public virtual void Update(GameTime gameTime)
        {
            foreach (Entity entity in entities)
                entity.Update(gameTime);
        }


        public virtual void Draw(GameTime gameTime)
        {
            edgeDetection.PrepareDraw();
            edgeDetection.PrepareDrawNormalDepth();
            foreach (Entity entity in entities)
                entity.Draw(gameTime, !entity.IgnoreEdgeDetection ? "NormalDepth" : "IgnoreNormalDepth");

            edgeDetection.PrepareDrawDefault();
            foreach (Entity entity in entities)
                entity.Draw(gameTime, "Default");          

            edgeDetection.Draw(gameTime);
            bloom.Draw(gameTime);
        }



    }
}
