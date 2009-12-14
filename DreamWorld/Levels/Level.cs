using System;
using System.Collections.Generic;
using DreamWorld.Cameras;
using DreamWorld.Entities;
using DreamWorld.Helpers.Renderers;
using DreamWorld.Levels.VillageLevel.Entities;
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

        public Dictionary<string, Entity> Entities { get; private set; }
        private Dictionary<string, ParticleSystem> particleSystems;

        public LevelInformation LevelInformation { get; private set; }

        private bool initialized;

        private SpriteBatch spriteBatch;
        protected Bloom bloom;
        protected EdgeDetection edgeDetection;

        protected Level()
        {
            Entities = new Dictionary<string, Entity>();
            particleSystems = new Dictionary<string, ParticleSystem>();
        }


        public void AddEntity(string name, Entity entity)
        {
            if(Entities.ContainsKey(name))
                throw new InvalidOperationException("Entity " + name + " already exists");
            
            Entities.Add(name, entity);
            
            if(initialized)
                entity.Initialize();            
        }

        public void AddParticleSystem(string name, ParticleSystem particleSystem)
        {
            if (Entities.ContainsKey(name))
                throw new InvalidOperationException("Particle System " + name + " already exists");

            particleSystems.Add(name, particleSystem);
            
            if(initialized)
                particleSystem.Initialize();
        }


        public Entity FindEntity(string name)
        {            
            return Entities[name];
        }               

        public void RenameEntity(string oldName, string newName)
        {
            if (!Entities.ContainsKey(oldName))
                throw new InvalidOperationException("Entity " + oldName + " doesn't exist");
            if (Entities.ContainsKey(newName))
                throw new InvalidOperationException("Entity " + newName + " already exists");

            Entity entity = Entities[oldName];
            Entities.Remove(oldName);
            Entities.Add(newName, entity);
        }

        public void RemoveEntity(string name)
        {
            if (!Entities.ContainsKey(name))
                throw new InvalidOperationException("Entity " + name + " doesn't exist");
            Entities.Remove(name);
        }

        public bool EntityNameExists(string name)
        {
            return Entities.ContainsKey(name);
        }       

        public virtual void Initialize()
        {
            Game = (DreamWorldGame) GameScreen.ScreenManager.Game;

            LevelInformation = LevelInformation.Load(LevelInformationFileName);
            
            foreach (SpawnInformation spawn in LevelInformation.Spawns)
            {
                Entity entity = CreateEntity(spawn);
                AddEntity(spawn.Name, entity);
            }
            InitializeSpecialEntities();

            Player = new Player();
            AddEntity("player", Player);

            foreach (Entity entity in Entities.Values)
                entity.Initialize();
            foreach (ParticleSystem particleSystem in particleSystems.Values)
                particleSystem.Initialize();

            Player.Body.MoveTo(
                LevelInformation.PlayerStartPosition, 
                Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(LevelInformation.PlayerStartRotation.Y),
                                              MathHelper.ToRadians(LevelInformation.PlayerStartRotation.X),
                                              MathHelper.ToRadians(LevelInformation.PlayerStartRotation.Z)));
            
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            if(Game.Config.Bloom)
                bloom = new Bloom(Game, spriteBatch);
            if(Game.Config.EdgeDetect)
                edgeDetection = new EdgeDetection(Game, spriteBatch);
            initialized = true;
        }


        public virtual void Update(GameTime gameTime)
        {
            foreach (Entity entity in Entities.Values)
                entity.Update(gameTime);
            foreach (ParticleSystem particleSystem in particleSystems.Values)
                particleSystem.Update(gameTime);
        }


        public virtual void Draw(GameTime gameTime)
        {
            if (edgeDetection != null)
            {
                edgeDetection.PrepareDraw();
                edgeDetection.PrepareDrawNormalDepth();
                foreach (Entity entity in Entities.Values)
                    entity.Draw(gameTime, !entity.IgnoreEdgeDetection ? "NormalDepth" : "IgnoreNormalDepth");

                edgeDetection.PrepareDrawDefault();
                               
                DrawEntities(gameTime);                

                edgeDetection.Draw(gameTime);
            }
            else
            {
                DrawEntities(gameTime);
            }

            if(Game.Config.Particles)
                foreach (ParticleSystem particleSystem in particleSystems.Values)
                    particleSystem.Draw(gameTime);
            
            if(bloom != null)
                bloom.Draw(gameTime);
        }

        private void DrawEntities(GameTime gameTime)
        {
            foreach (Entity entity in Entities.Values)
            {
                #if (DEBUG)
                DebugCamera debugCamera = GameScreen.Camera as DebugCamera;
                if (debugCamera != null && entity == debugCamera.Form.Entity)
                    entity.Draw(gameTime, "DebugHighlight");
                else
                    entity.Draw(gameTime, "Default");
                #else
                    entity.Draw(gameTime, "Default");
                #endif
            }
        }

        public abstract string LevelInformationFileName { get; }
        protected virtual void InitializeSpecialEntities() {  }

        public Entity CreateEntity(SpawnInformation spawn)
        {
            Type t = Type.GetType(spawn.TypeName);
            Entity entity = (Entity)Activator.CreateInstance(t);
            entity.SpawnInformation = spawn;
            return entity;                        
        }
    }
}
