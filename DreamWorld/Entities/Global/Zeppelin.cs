using System;
using System.Collections.Generic;
using DreamWorld.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Entities.Global
{
    class Zeppelin : Entity
    {        
        private float propellorRotation;
        private List<ModelMesh> propellors;
        private ModelMesh[] cogs;
        
        
        private float time;
        
        public float Speed { get; set; }
        public Curve3D Path { get; set; }        

        public Zeppelin()
        {
            propellors = new List<ModelMesh>();
            cogs = new ModelMesh[4];
        }

        protected override void LoadContent()
        {
            Model = GameScreen.Content.Load<Model>(@"Models\Global\Zeppelin\Zeppelin");
           
            
            foreach (ModelMesh mesh in Model.Meshes)
            {
                String name = mesh.Name.ToLower();
                if(mesh.Name.ToLower().Contains("propellor"))
                    propellors.Add(mesh);
                if (mesh.Name.ToLower().Contains("cog01"))
                    cogs[0] = mesh;
                if (mesh.Name.ToLower().Contains("cog02"))
                    cogs[1] = mesh;
                if (mesh.Name.ToLower().Contains("cog03"))
                    cogs[2] = mesh;
                if (mesh.Name.ToLower().Contains("cog04"))
                    cogs[3] = mesh;               
            }
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Matrix rotation = Matrix.Identity;
            if(Path != null)
            {
                time += Speed;
                Position = Path.GetPointOnCurve(time);
                
                // TODO: The way rotation is handled could be better.
                rotation = Matrix.Invert(Matrix.CreateLookAt(Position, Path.GetPointOnCurve(time + Speed), Vector3.Up)) * Matrix.CreateRotationY(MathHelper.Pi); // Note: 180 degrees turn maybe not necessary in the future
                rotation.Translation = Vector3.Zero;                
            }

            propellorRotation += Speed * 500;
            foreach (ModelMesh mesh in propellors)            
                mesh.ParentBone.Transform = Matrix.CreateRotationY(propellorRotation);

            cogs[0].ParentBone.Transform = Matrix.CreateRotationY(propellorRotation * .15f); // Big, left
            cogs[1].ParentBone.Transform = Matrix.CreateRotationY(propellorRotation * .4f); // Small, left
            cogs[2].ParentBone.Transform = Matrix.CreateRotationY(-propellorRotation * .15f); // Big, right
            cogs[3].ParentBone.Transform = Matrix.CreateRotationY(-propellorRotation * .4f); // Small, right
            
            World = Matrix.CreateScale(Scale) *
                    rotation * 
                    Matrix.CreateTranslation(Position);         
        }
    }
}
