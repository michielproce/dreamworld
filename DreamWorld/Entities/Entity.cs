using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Entities
{
    public class Entity : DrawableGameComponent
    {       
        public Vector3 Position { get; protected set; }
        public Quaternion Rotation { get; protected set; }
        public Vector3 Scale { get; protected set; }
        public Model Model { get; protected set; }
        public Effect Effect { get; protected set; }
        public BoundingSphere[] BoundingSpheres { get; protected set; }
        public Matrix World { get; private set; }

        protected DreamWorldGame DWG { get; private set; }

        private Dictionary<ModelMeshPart, Effect> originalEffects;
        
        public Entity(Game game) : base(game)
        {
            DWG = (DreamWorldGame) game;
            originalEffects = new Dictionary<ModelMeshPart, Effect>();
        }

        public override void Initialize()
        {
            Scale = Vector3.One;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            if(Model != null)
            {
                foreach (ModelMesh mesh in Model.Meshes)
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        originalEffects.Add(part, part.Effect);
                    }
                }
            }
            base.LoadContent();
        }


        public override void Update(GameTime gameTime)
        {
            World = Matrix.CreateTranslation(Position)*
                Matrix.CreateFromQuaternion(Rotation) *
                Matrix.CreateScale(Scale);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach(ModelMesh mesh in Model.Meshes)
            {
                foreach(ModelMeshPart part in mesh.MeshParts)
                {             
                    if(DWG.DrawNormalDepth)
                    {
                        part.Effect = DWG.NormalDepthEffect;
                        part.Effect.Parameters["world"].SetValue(World);
                        part.Effect.Parameters["view"].SetValue(DWG.CurrentCamera.View);
                        part.Effect.Parameters["projection"].SetValue(DWG.CurrentCamera.Projection);
                        
                    } 
                    else
                    {
                        part.Effect = DWG.DefaultEffect;                        
                        part.Effect.Parameters["world"].SetValue(World);
                        part.Effect.Parameters["view"].SetValue(DWG.CurrentCamera.View);
                        part.Effect.Parameters["projection"].SetValue(DWG.CurrentCamera.Projection);
                        part.Effect.Parameters["Texture"].SetValue(((BasicEffect)originalEffects[part]).Texture);
                    }
                   
                }
                mesh.Draw();
            }
        }
    }
}
