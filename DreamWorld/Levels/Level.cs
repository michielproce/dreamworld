using System;
using System.Collections.Generic;
using DreamWorld.Entities;
using DreamWorld.Helpers.Renderers;
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

        private Dictionary<string, Entity> entities;
        

        private SpriteBatch spriteBatch;
        private Bloom bloom;
        private EdgeDetection edgeDetection;

        protected Level()
        {
            entities = new Dictionary<string, Entity>();
            
        }


        protected void AddEntity(string name, Entity entity)
        {
            if(entities.ContainsKey(name))
                throw new InvalidOperationException("Entity " + name + " already exists");
            entity.Level = this;
            entity.GameScreen = GameScreen;
            entity.Game = GameScreen.ScreenManager.Game as DreamWorldGame;
            entities.Add(name, entity);
        }

        public Entity FindEntity(string name)
        {
            return entities[name];
        }

        public virtual void Initialize()
        {
            Player = new Player
                         {
                             InputManager = ((DreamWorldGame) GameScreen.ScreenManager.Game).InputManager
                         };
            AddEntity("player", Player);
            foreach (Entity entity in entities.Values)
                entity.Initialize();
            
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            bloom = new Bloom(GameScreen.ScreenManager.Game, spriteBatch);
            edgeDetection = new EdgeDetection(GameScreen.ScreenManager.Game, spriteBatch);
        }


        public virtual void Update(GameTime gameTime)
        {
            foreach (Entity entity in entities.Values)
                entity.Update(gameTime);
        }


        public virtual void Draw(GameTime gameTime)
        {
            edgeDetection.PrepareDraw();
            edgeDetection.PrepareDrawNormalDepth();
            foreach (Entity entity in entities.Values)
                entity.Draw(gameTime, !entity.IgnoreEdgeDetection ? "NormalDepth" : "IgnoreNormalDepth");

            edgeDetection.PrepareDrawDefault();
            foreach (Entity entity in entities.Values)
                entity.Draw(gameTime, "Default");          

            edgeDetection.Draw(gameTime);
            bloom.Draw(gameTime);

            #if (DEBUG)
            LineRenderer.Render(GameScreen.CurrentCamera, Game.GraphicsDevice);
            #endif
        }



    }
}
