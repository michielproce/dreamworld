using System;
using System.Collections.Generic;
using DreamWorld.Audio;
using DreamWorld.Levels;
using DreamWorld.ScreenManagement.Screens;
using DreamWorldBase;
using JigLibX.Collision;
using JigLibX.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Entities
{
    public abstract class Entity
    {
        public Group Group
        {
            get { return _group; }
            internal set
            {
                if (_group != null)
                    _group.RemoveEntity(Name);

                _group = value;
                if (_group != null)
                    _group.AddEntity(Name, this);
            }
        }

        private Group _group;

        protected GameScreen GameScreen {get; private set; }
        private ContentManager Content { get; set; }
        protected GraphicsDevice GraphicsDevice { get; private set; }        
        protected Level Level { get; private set; }

        public SpawnInformation SpawnInformation { get; set; }
        public string Name { get; set; }
        public Vector3 Scale { get; set; }
        public Body Body { get; private set; }
        public CollisionSkin Skin { get; private set; }
        public Vector3 CenterOfMass;
        
        public Matrix World { get; private set; }
        
        public Model Model { get; protected set; }

        public Animation Animation { get; private set; }

        public bool IgnoreEdgeDetection { get; protected set; }
        public bool RenderCollisionPrimitives { get; protected set; }
        
        private List<SoundEffect3D> sounds;

        protected Matrix[] transforms;

        protected bool initialized;

        protected Entity()
        {
            GameScreen = GameScreen.Instance;
            Level = GameScreen.Level;
            Content = GameScreen.Content;
            GraphicsDevice = GameScreen.GraphicsDevice;

            Scale = Vector3.One;
            Animation = new Animation();
            World = Matrix.Identity;
            sounds = new List<SoundEffect3D>();
        }

        ~Entity()
        {
            DisablePhysics();
        } 

        public void DisablePhysics()
        {
            DreamWorldPhysicsSystem.CurrentPhysicsSystem.RemoveBody(Body);
        }

        public virtual void Initialize()
        {
            if (SpawnInformation != null)
                Scale = SpawnInformation.Scale;

            Body body;
            CollisionSkin skin;
            Vector3 centerOfMass;
            GetPhysicsInformation(out body, out skin, out centerOfMass);

            Body = body;
            Skin = skin;
            CenterOfMass = centerOfMass;

            foreach (SoundEffect3D sound in sounds)
                sound.Initialize();

            if (SpawnInformation != null)
            {
                Body.Position = SpawnInformation.Position;
                Body.Orientation = Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(SpawnInformation.Rotation.Y),
                                                                 MathHelper.ToRadians(SpawnInformation.Rotation.X),
                                                                 MathHelper.ToRadians(SpawnInformation.Rotation.Z));
            }

            Body.Immovable = true;

            LoadContent();
            initialized = true;
        }

        public void Spawn()
        {
            Group = Level.GetGroup(SpawnInformation == null ? 0 : SpawnInformation.Group);

            if (this is GroupCenter)
                Group.Center = this as GroupCenter;
        }

        protected virtual void LoadContent()
        {
            if (Model == null)
                throw new InvalidOperationException("Tried to create an entity without a Model.");

            #if (DEBUG)
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    if(part.Effect is BasicEffect)
                    {
                        throw new InvalidOperationException("Found BasicEffect, are you using the Default Model Processor?");
                    }   
                }
            }
            #endif

            SkinningData sd = Model.Tag as SkinningData;
            if (sd != null)
                Animation.Load(sd);

            transforms = new Matrix[Model.Bones.Count];
        }

        protected virtual void GetPhysicsInformation(out Body body, out CollisionSkin skin, out Vector3 centerOfMass)
        {
            body = new Body();
            skin = new CollisionSkin(body);
            body.CollisionSkin = skin;
            centerOfMass = Vector3.Zero;
            body.EnableBody();
            return;
        }

        public virtual void Update(GameTime gameTime)
        {
           foreach (SoundEffect3D sound in sounds)
           {
               sound.Emitter.Position = Body.Position;
               sound.Update(gameTime);
           }

           Animation.Update(gameTime);
            
           World = GenerateWorldMatrix();
        }

        protected virtual Matrix GenerateWorldMatrix()
        {
            return
                Matrix.CreateScale(Scale) *
                Body.Orientation *
                Matrix.CreateTranslation(Body.Position);
        }

        public virtual void Draw(GameTime gameTime, string technique)
        {
            Model.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach(Effect effect in mesh.Effects)
                {
                    effect.CurrentTechnique = effect.Techniques[technique];
                    effect.Parameters["world"].SetValue(transforms[mesh.ParentBone.Index] * World);
                    effect.Parameters["view"].SetValue(GameScreen.Camera.View);
                    effect.Parameters["projection"].SetValue(GameScreen.Camera.Projection);
                    if (Group != null)
                        effect.Parameters["Ambient"].SetValue(Group.Color.ToVector3());
                    if(Animation.Loaded)                    
                        effect.Parameters["Bones"].SetValue(Animation.SkinTransforms);
                    
                }
                mesh.Draw();
            }

            #if (DEBUG)
                if (RenderCollisionPrimitives && GameScreen.debugDrawer.Enabled && Skin.NumPrimitives > 0) 
                {
                    VertexPositionColor[] wf = Skin.GetLocalSkinWireframe();
                    if (Body.CollisionSkin != null)
                        Body.TransformWireframe(wf);

                    GameScreen.debugDrawer.DrawShape(wf);
                }
            #endif
        }

        protected void AddSoundEffect(SoundEffect3D sound)
        {
            sounds.Add(sound);
            if(initialized)
                sound.Initialize();
        }

        public static Entity CreateFromSpawnInfo(SpawnInformation spawn)
        {
            Type t = Type.GetType(spawn.TypeName);
            Entity entity = (Entity)Activator.CreateInstance(t);
            entity.SpawnInformation = spawn;
            entity.Name = spawn.Name;
            return entity;
        }
    }
}