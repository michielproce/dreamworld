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
        
        private float time;
        
        public float Speed { get; set; }
        public Curve3D Path { get; set; }        

        public Zeppelin()
        {
            propellors = new List<ModelMesh>();          
        }

        protected override void LoadContent()
        {
            if(Path != null)
            {
            }

            Model = Game.Content.Load<Model>(@"Models\Global\Zeppelin");
           
            foreach (ModelMesh mesh in Model.Meshes)
            {
                if(mesh.Name.ToLower().Contains("propellor"))
                    propellors.Add(mesh);
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
                rotation = Matrix.Invert(Matrix.CreateLookAt(Position, Path.GetPointOnCurve(time + Speed), Vector3.Up));
                rotation.Translation = Vector3.Zero;                
            }

            propellorRotation += Speed * 500;
            foreach (ModelMesh mesh in propellors)            
                mesh.ParentBone.Transform = Matrix.CreateRotationY(propellorRotation);


            World = Matrix.CreateScale(Scale) *
                    rotation *
                    Matrix.CreateTranslation(Position);         
        }
    }
}
