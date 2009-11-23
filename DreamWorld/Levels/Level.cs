using System;
using System.Collections.Generic;
using DreamWorld.Entities;
using DreamWorld.Helpers.Renderers;
using DreamWorld.Rendering.Particles;
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

        protected Dictionary<string, Entity> entities;
        private Dictionary<string, ParticleSystem> particleSystems;

        private bool initialized;

        private SpriteBatch spriteBatch;
        protected Bloom bloom;
        protected EdgeDetection edgeDetection;

        protected Level()
        {
            entities = new Dictionary<string, Entity>();
            particleSystems = new Dictionary<string, ParticleSystem>();
        }


        public void AddEntity(string name, Entity entity)
        {
            if(entities.ContainsKey(name))
                throw new InvalidOperationException("Entity " + name + " already exists");
            entity.Level = this;
            entity.GameScreen = GameScreen;
            entity.Game = GameScreen.ScreenManager.Game as DreamWorldGame;
            entities.Add(name, entity);
            if(initialized)
                entity.Initialize();            
        }

        public void AddParticleSystem(string name, ParticleSystem particleSystem)
        {
            if (entities.ContainsKey(name))
                throw new InvalidOperationException("Particle System " + name + " already exists");
            particleSystem.GameScreen = GameScreen;
            particleSystem.Content = GameScreen.Content;
            particleSystem.GraphicsDevice = Game.GraphicsDevice;
            particleSystems.Add(name, particleSystem);
            if(initialized)
                particleSystem.Initialize();
        }

        public Entity FindEntity(string name)
        {
            return entities[name];
        }

        public virtual void Initialize()
        {
            Game = (DreamWorldGame) GameScreen.ScreenManager.Game;
            Player = new Player
                         {
                             InputManager = Game.InputManager
                         };
            AddEntity("player", Player);
            foreach (Entity entity in entities.Values)
                entity.Initialize();
            foreach (ParticleSystem particleSystem in particleSystems.Values)
                particleSystem.Initialize();
            
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            bloom = new Bloom(Game, spriteBatch);
            edgeDetection = new EdgeDetection(Game, spriteBatch);
            initialized = true;
        }


        public virtual void Update(GameTime gameTime)
        {
            foreach (Entity entity in entities.Values)
                entity.Update(gameTime);
            foreach (ParticleSystem particleSystem in particleSystems.Values)
                particleSystem.Update(gameTime);
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
            foreach (ParticleSystem particleSystem in particleSystems.Values)
                particleSystem.Draw(gameTime);
            bloom.Draw(gameTime);
            
            #if (DEBUG)
            LineRenderer.Render(GameScreen.CurrentCamera, Game.GraphicsDevice);
            BoundingSphereRenderer.Render(GameScreen.CurrentCamera, Game.GraphicsDevice);
            #endif
        }



    }
}
