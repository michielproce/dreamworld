using System;
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
        
        public Entity(Game game) : base(game)
        {
            DWG = (DreamWorldGame) game;
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
                    BasicEffect be = (BasicEffect) part.Effect;
                    be.Projection = DWG.CurrentCamera.Projection;
                    be.View = DWG.CurrentCamera.View;
                    be.World = World;
                }
                mesh.Draw();
            }
        }
    }
}
