﻿using System.Collections.Generic;
using DreamWorld.Cameras;
using DreamWorld.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Entities
{
    public abstract class Entity
    {        
        public Game Game { protected get; set; }
        public Camera Camera { protected get; set; }
        public Effect Effect { protected get; set; }
     
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; protected set; }
        public Vector3 Scale { get; protected set; }
        public Model Model { get; protected set; }
        
        public BoundingSphere[] BoundingSpheres { get; protected set; }
        public Matrix World { get; private set; }

        private Dictionary<ModelMeshPart, Effect> originalEffects;

        protected Entity()
        {
            Scale = Vector3.One;
            originalEffects = new Dictionary<ModelMeshPart, Effect>();
        }

        public virtual void Initialize()
        {
            LoadContent();
        }

        protected virtual void LoadContent()
        {
            if (Model != null)
            {
                foreach (ModelMesh mesh in Model.Meshes)
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        originalEffects.Add(part, part.Effect);
                    }
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            World = Matrix.CreateTranslation(Position) *
                Matrix.CreateFromQuaternion(Rotation) *
                Matrix.CreateScale(Scale);
        }

        public void Draw(GameTime gameTime)
        {
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = Effect;
                    part.Effect.Parameters["world"].SetValue(World);
                    part.Effect.Parameters["view"].SetValue(Camera.View);
                    part.Effect.Parameters["projection"].SetValue(Camera.Projection);
                    part.Effect.Parameters["Texture"].SetValue(((BasicEffect)originalEffects[part]).Texture);
                }
                mesh.Draw();
            }
        }
    }
}