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

        public Matrix World { get; private set; }
        
        public Model Model { get; protected set; }

        protected Animation Animation { get; set; }

        public bool IgnoreEdgeDetection { get; protected set; }
        public bool RenderCollisionPrimitives { get; protected set; }
        
        protected readonly List<SoundEffect3D> Sounds;

        protected Matrix[] Transforms;

        protected Entity()
        {
            GameScreen = GameScreen.Instance;
            Level = GameScreen.Level;
            Content = GameScreen.Content;
            GraphicsDevice = GameScreen.GraphicsDevice;

            Scale = Vector3.One;
            Animation = new Animation();
            World = Matrix.Identity;
            Sounds = new List<SoundEffect3D>();
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

            if (SpawnInformation != null)
            {
                Body.Position = SpawnInformation.Position;
                Body.Orientation = Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(SpawnInformation.Rotation.Y),
                                                                 MathHelper.ToRadians(SpawnInformation.Rotation.X),
                                                                 MathHelper.ToRadians(SpawnInformation.Rotation.Z));
            }

            Body.Immovable = true;

            LoadContent();
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

            Transforms = new Matrix[Model.Bones.Count];
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
           foreach (SoundEffect3D sound in Sounds)
                sound.Update(gameTime);

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
            if (Model != null)
            {
                Model.CopyAbsoluteBoneTransformsTo(Transforms);
                foreach (ModelMesh mesh in Model.Meshes)
                {
                    foreach (Effect effect in mesh.Effects)
                    {
                        effect.CurrentTechnique = effect.Techniques[technique];
                        effect.Parameters["world"].SetValue(Transforms[mesh.ParentBone.Index]*World);
                        effect.Parameters["view"].SetValue(GameScreen.Camera.View);
                        effect.Parameters["projection"].SetValue(GameScreen.Camera.Projection);
                        if (Group != null)
                            effect.Parameters["Ambient"].SetValue(Group.Color.ToVector3());
                        if (Animation.Loaded)
                            effect.Parameters["Bones"].SetValue(Animation.SkinTransforms);

                    }
                    mesh.Draw();
                }
            }

            #if (DEBUG && !XBOX)
                if (RenderCollisionPrimitives && GameScreen.debugDrawer.Enabled && Skin.NumPrimitives > 0) 
                {
                    VertexPositionColor[] wf = Skin.GetLocalSkinWireframe();
                    if (Body.CollisionSkin != null)
                        Body.TransformWireframe(wf);

                    GameScreen.debugDrawer.DrawShape(wf);
                }
            #endif
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