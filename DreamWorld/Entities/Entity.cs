using System;
using DreamWorld.Levels;
using DreamWorld.ScreenManagement.Screens;
using DreamWorldBase;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Entities
{
    public abstract class Entity
    {
        public DreamWorldGame Game { protected get; set; }
        public GameScreen GameScreen { protected get; set; }
        public Level Level { protected get; set; }
        
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; protected set; }
        public Vector3 Scale { get; protected set; }
        public Model Model { get; protected set; }
        
        public BoundingSphere[] BoundingSpheres { get; protected set; }
        public Matrix World { get; protected set; }
        
        public Animation Animation { get; private set; }

        public bool IgnoreEdgeDetection { get; protected set; }
        
        private Matrix[] transforms;

        protected Entity()
        {
            Scale = Vector3.One;
            Animation = new Animation();            
        }

        public virtual void Initialize()
        {
            LoadContent();
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

        public virtual void Update(GameTime gameTime)
        {
            World = Matrix.CreateScale(Scale)*
                    Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z)*
                    Matrix.CreateTranslation(Position);
             
           Animation.AdvanceAnimation(gameTime);
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
                    effect.Parameters["view"].SetValue(GameScreen.CurrentCamera.View);
                    effect.Parameters["projection"].SetValue(GameScreen.CurrentCamera.Projection);                    
                    if(Animation.Loaded)                    
                        effect.Parameters["Bones"].SetValue(Animation.SkinTransforms);
                    
                }
                mesh.Draw();
            }
        }
    }
}