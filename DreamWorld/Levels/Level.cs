﻿using System;
using System.Collections.Generic;
using DreamWorld.Cameras;
using DreamWorld.Entities;
using DreamWorld.Rendering.Particles;
using DreamWorld.Rendering.Postprocessing;
using DreamWorld.ScreenManagement.Screens;
using DreamWorld.Util;
using JigLibX.Collision;
using JigLibX.Geometry;
using JigLibX.Physics;
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
        
        public Color LoadingColor { get; protected set;  }

        private bool initialized;

        private SpriteBatch spriteBatch;
        protected Bloom bloom;
        private EdgeDetection edgeDetection;

        public Vector3 overviewPosition = new Vector3(50, 500, 150);
        public Vector3 overviewLookat = new Vector3(150, 0, 150);

        protected Level()
        {
            Groups = new Dictionary<int, Group>();
            particleSystems = new Dictionary<string, ParticleSystem>();
            LoadingColor = Color.Black;
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

            foreach(GroupColorInformation colorInfo in LevelInformation.GroupColors)
            {
                GetGroup(colorInfo.ID).Color = new Color(colorInfo.R, colorInfo.G, colorInfo.B);
            }

            foreach (SpawnInformation spawn in LevelInformation.Spawns)
            {
                Entity entity = Entity.CreateFromSpawnInfo(spawn);
                entity.Spawn();
            }

            foreach (KeyValuePair<int, Group> group in Groups)
            {
                group.Value.groupId = group.Key;
                group.Value.Initialize();
            }
            foreach (ParticleSystem particleSystem in particleSystems.Values)
                particleSystem.Initialize();

            Player.Initialize();
            Player.SpawnPosition = LevelInformation.PlayerStartPosition;
            Player.SpawnOrientation = Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(LevelInformation.PlayerStartRotation.Y),
                                              MathHelper.ToRadians(LevelInformation.PlayerStartRotation.X),
                                              MathHelper.ToRadians(LevelInformation.PlayerStartRotation.Z));

            Player.Respawn();
            Player.Group = GetGroup(0);
            
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            bloom = new Bloom(Game, spriteBatch);
            InitBloom(ref bloom);

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

        public void SetGroup(Group group, int groupId)
        {
            // In case of a class that derive from Group, add it manually.
            Groups.Add(groupId, group);
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
            foreach (ParticleSystem particleSystem in particleSystems.Values)
                particleSystem.Update(gameTime);
            foreach (Group group in Groups.Values)
                group.Update(gameTime);               
        }

        public virtual void Draw(GameTime gameTime)
        {
            List<CollisionSkin> ignoreList = new List<CollisionSkin>();

            if(GameScreen.Camera is ThirdPersonCamera)
            {
                CollisionSkin skin = ((ThirdPersonCamera) GameScreen.Camera).GetCollidingSkin();

                if (skin != null)
                    ignoreList.Add(skin);
            }

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
                    if(!ignoreList.Contains(entity.Skin))
                        entity.Draw(gameTime, !entity.IgnoreEdgeDetection ? "NormalDepth" : "IgnoreNormalDepth");
                }
            }

            edgeDetection.PrepareDrawDefault();                

            DrawEntities(gameTime, ignoreList);

            edgeDetection.Draw(gameTime);

            foreach (ParticleSystem particleSystem in particleSystems.Values)
                particleSystem.Draw(gameTime);

            
      
            bloom.Draw(gameTime);
        }

        private void DrawEntities(GameTime gameTime, List<CollisionSkin> ignoreList)
        {
            if (Skybox != null)
                Skybox.Draw(gameTime, "Skybox");
            if (Terrain != null)
                Terrain.Draw(gameTime, "Terrain");

            int[] keys = new int[Groups.Count];
            Groups.Keys.CopyTo(keys, 0);

            foreach (Group group in Groups.Values)
            {
                foreach (Entity entity in group.Entities.Values)
                {
                    if (!ignoreList.Contains(entity.Skin))
                    {
#if (DEBUG && !XBOX)
                        DebugCamera debugCamera = GameScreen.Camera as DebugCamera;
                        if (debugCamera != null && entity == debugCamera.Form.Entity)
                            entity.Draw(gameTime, "Highlight");
                        else
                        {
#endif

                            if (this is PuzzleLevel)
                            {
                                Group selectedGroup = ((PuzzleLevel)this).GetSelectedGroup();
                                if (group == selectedGroup)
                                    entity.Draw(gameTime, "Highlight");
                                else
                                    entity.Draw(gameTime, "Default");
                            }
                            else
                            {
                                entity.Draw(gameTime, "Default");
                            }

#if (DEBUG&& !XBOX)
                        }
#endif
                    }
                }
            }
        }

        public abstract string LevelInformationFileName { get; }
        public abstract void InitBloom(ref Bloom bloom);
    }
}
