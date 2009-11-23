using System;
using System.Collections.Generic;
using DreamWorld.Audio;
using DreamWorld.Helpers.Renderers;
using DreamWorld.Levels;
using DreamWorld.ScreenManagement.Screens;
using DreamWorldBase;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Entities
{
    public abstract class Entity
    {
        protected GameScreen GameScreen {get; private set; }
        protected ContentManager Content { get; private set; }
        protected GraphicsDevice GraphicsDevice { get; private set; }        
        protected Level Level { get; private set; }
                
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }
        
        public Matrix World { get; protected set; }
        
        public Model Model { get; protected set; }
        
        public BoundingSphere BoundingSphere { get; protected set; }

        public Animation Animation { get; private set; }

        public bool IgnoreEdgeDetection { get; protected set; }
        public bool RenderSpheres { get; protected set; }
        
        private List<SoundEffect3D> sounds;

        protected Matrix[] transforms;

        private bool initialized;

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

        public virtual void Initialize()
        {
            foreach (SoundEffect3D sound in sounds)
                sound.Initialize();                

            LoadContent();
            initialized = true;
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
            
            foreach (ModelMesh mesh in Model.Meshes)
            {
                BoundingSphere = BoundingSphere.CreateMerged(BoundingSphere, mesh.BoundingSphere);
            }

            SkinningData sd = Model.Tag as SkinningData;
            if (sd != null)
                Animation.Load(sd);

            transforms = new Matrix[Model.Bones.Count];            
        }

        public virtual void Update(GameTime gameTime)
        {
           World = GenerateWorldMatrix();

           foreach (SoundEffect3D sound in sounds)
           {
               sound.Emitter.Position = Position;
               sound.Update(gameTime);
           }

            Animation.Update(gameTime);
        }

        protected virtual Matrix GenerateWorldMatrix()
        {
            return  Matrix.CreateScale(Scale) *
                    Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) *
                    Matrix.CreateTranslation(Position);
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
                    if(Animation.Loaded)                    
                        effect.Parameters["Bones"].SetValue(Animation.SkinTransforms);
                    
                }
                mesh.Draw();
                
                #if (DEBUG)
                if (RenderSpheres)
                    BoundingSphereRenderer.AddSphere(mesh.BoundingSphere, (transforms[mesh.ParentBone.Index] * World), Color.Yellow);
                #endif
            }

            #if (DEBUG)
            if (RenderSpheres)
                BoundingSphereRenderer.AddSphere(BoundingSphere, World, Color.BlueViolet);
            #endif
        }

        protected void AddSoundEffect(SoundEffect3D sound)
        {
            sounds.Add(sound);
            if(initialized)
                sound.Initialize();   
        }
    }
}