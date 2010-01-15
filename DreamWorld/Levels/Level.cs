﻿using System;
using System.Collections.Generic;
using DreamWorld.Cameras;
using DreamWorld.Entities;
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

        public Dictionary<int, Group> Groups { get; private set; }
        private Dictionary<string, ParticleSystem> particleSystems;

        public LevelInformation LevelInformation { get; private set; }

        private bool initialized;

        private SpriteBatch spriteBatch;
        private Bloom bloom;
        private EdgeDetection edgeDetection;

        protected Level()
        {
            Groups = new Dictionary<int, Group>();
            particleSystems = new Dictionary<string, ParticleSystem>();
        }

        public virtual void Initialize()
        {
            Game = (DreamWorldGame) GameScreen.ScreenManager.Game;

            LevelInformation = LevelInformation.Load(LevelInformationFileName);

            Player = new Player { Name = "Player" };

            if (Terrain != null)
                Terrain.Initialize();
            if (Skybox != null)
                Skybox.Initialize();

            GetGroup(0).AllowedRotations = Vector3.Zero;

            foreach (SpawnInformation spawn in LevelInformation.Spawns)
            {
                Entity entity = Entity.CreateFromSpawnInfo(spawn);
                entity.Spawn();
            }

            foreach (Group group in Groups.Values)
                group.Initialize();
            foreach (ParticleSystem particleSystem in particleSystems.Values)
                particleSystem.Initialize();

            Player.Initialize();
            Player.StartPosition = LevelInformation.PlayerStartPosition;
            Player.StartOrientation = Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(LevelInformation.PlayerStartRotation.Y),
                                              MathHelper.ToRadians(LevelInformation.PlayerStartRotation.X),
                                              MathHelper.ToRadians(LevelInformation.PlayerStartRotation.Z));

            Player.Respawn();
            Player.Group = GetGroup(0);
            
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            if(Game.Config.Bloom)
                bloom = new Bloom(Game, spriteBatch);
            if(Game.Config.EdgeDetect)
                edgeDetection = new EdgeDetection(Game, spriteBatch);
            initialized = true;
        }

        public Group GetGroup(int groupId)
        {
            Group group;
            if (Groups.TryGetValue(groupId, out group))
                return group;

            group = new Group();
            Groups.Add(groupId, group);
            return group;
        }

        public bool EntityNameExists(string name)
        {
            foreach (Group group in Groups.Values)
            {
                if(group.EntityNameExists(name))
                    return true;
            }
            return false;
        } 

        public Entity FindEntity(string name)
        {
            foreach (Group group in Groups.Values)
            {
                if (group.EntityNameExists(name))
                    return group.FindEntity(name);
            }
            throw new InvalidOperationException("Entity " + name + " doesn't exist");
        }

        public void AddParticleSystem(string name, ParticleSystem particleSystem)
        {
            if (particleSystems.ContainsKey(name))
                throw new InvalidOperationException("Particle System " + name + " already exists");

            particleSystems.Add(name, particleSystem);
            
            if(initialized)
                particleSystem.Initialize();
        }

        public virtual void Update(GameTime gameTime)
        {
           
            foreach (Group group in Groups.Values)
                group.Update(gameTime);
            foreach (ParticleSystem particleSystem in particleSystems.Values)
                particleSystem.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            
            if (edgeDetection != null)
            {
                edgeDetection.PrepareDraw();
                edgeDetection.PrepareDrawNormalDepth();
                
                if (Skybox != null)
                    Skybox.Draw(gameTime, "IgnoreNormalDepth");

                if(Terrain != null)
                    Terrain.Draw(gameTime, "IgnoreNormalDepth");
                
                foreach (Group group in Groups.Values)
                {
                    foreach(Entity entity in group.Entities.Values)
                    {
                        entity.Draw(gameTime, !entity.IgnoreEdgeDetection ? "NormalDepth" : "IgnoreNormalDepth");
                    }
                }

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
            if (Skybox != null)
                Skybox.Draw(gameTime, "Skybox");
            if (Terrain != null)
                Terrain.Draw(gameTime, "Terrain");            

            foreach (Group group in Groups.Values)
            {
                foreach (Entity entity in group.Entities.Values)
                {
                    #if (DEBUG)
                    DebugCamera debugCamera = GameScreen.Camera as DebugCamera;
                    if (debugCamera != null && entity == debugCamera.Form.Entity)
                        entity.Draw(gameTime, "Highlight");
                    else
                        entity.Draw(gameTime, "Default");
                    #else
                        entity.Draw(gameTime, "Default");
                    #endif
                }
            }
        }

        public abstract string LevelInformationFileName { get; }        
    }
}
