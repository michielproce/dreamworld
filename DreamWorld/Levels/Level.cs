using System;
using System.Collections.Generic;
using DreamWorld.Cameras;
using DreamWorld.Entities;
using DreamWorld.Levels.VillageLevel.Entities;
using DreamWorld.Rendering.Particles;
using DreamWorld.Rendering.Postprocessing;
using DreamWorld.ScreenManagement.Screens;
using JigLibX.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Levels
{
    public abstract class Level
    {
        protected DreamWorldGame Game { get; set; }
        public GameScreen GameScreen { get; set; }
        public Terrain Terrain { get; protected set; }
        public Skybox Skybox { get; protected set; }

        public Player Player { get; private set; }

        public Dictionary<int, Group> Groups { get; private set; }
        private readonly Dictionary<string, ParticleSystem> _particleSystems;

        public LevelInformation LevelInformation { get; private set; }
        
        public Color LoadingColor { get; protected set;  }

        private bool _initialized;

        private SpriteBatch _spriteBatch;
        protected Bloom bloom;
        private EdgeDetection _edgeDetection;

        public Vector3 OverviewPosition = new Vector3(50, 500, 150);
        public Vector3 OverviewLookat = new Vector3(150, 0, 150);

        protected Level()
        {
            Groups = new Dictionary<int, Group>();
            _particleSystems = new Dictionary<string, ParticleSystem>();
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
                group.Value.GroupId = group.Key;
                group.Value.Initialize();
            }
            foreach (ParticleSystem particleSystem in _particleSystems.Values)
                particleSystem.Initialize();

            Player.Initialize();
            Player.SpawnPosition = LevelInformation.PlayerStartPosition;
            Player.SpawnOrientation = Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(LevelInformation.PlayerStartRotation.Y),
                                              MathHelper.ToRadians(LevelInformation.PlayerStartRotation.X),
                                              MathHelper.ToRadians(LevelInformation.PlayerStartRotation.Z));

            Player.Respawn();
            Player.Group = GetGroup(0);
            
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            bloom = new Bloom(Game, _spriteBatch);
            InitBloom(ref bloom);

            _edgeDetection = new EdgeDetection(Game, _spriteBatch);
            _initialized = true;
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

        protected void SetGroup(Group group, int groupId)
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
            if (_particleSystems.ContainsKey(name))
                throw new InvalidOperationException("Particle System " + name + " already exists");

            _particleSystems.Add(name, particleSystem);
            
            if(_initialized)
                particleSystem.Initialize();
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (ParticleSystem particleSystem in _particleSystems.Values)
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

            _edgeDetection.PrepareDraw();
            _edgeDetection.PrepareDrawNormalDepth();
            
            if (Skybox != null)
                Skybox.Draw(gameTime, "IgnoreNormalDepth");

            if(Terrain != null)
                Terrain.Draw(gameTime, "IgnoreNormalDepth");
            
            foreach (Group group in Groups.Values)
            {
                foreach(Entity entity in group.Entities.Values)
                {       
                    // HACK: remove some items from the ignore list.
                    if(ignoreList.Contains(entity.Skin) && (
                        entity is Statue ||
                        entity is TrashedHouse))
                        ignoreList.Remove(entity.Skin);
                    // END OF HACK
                    if(!ignoreList.Contains(entity.Skin)) 
                        entity.Draw(gameTime, !entity.IgnoreEdgeDetection ? "NormalDepth" : "IgnoreNormalDepth");

                }
            }

            _edgeDetection.PrepareDrawDefault();                

            DrawEntities(gameTime, ignoreList);

            _edgeDetection.Draw(gameTime);

            foreach (ParticleSystem particleSystem in _particleSystems.Values)
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
        protected abstract void InitBloom(ref Bloom bloom);
    }
}
