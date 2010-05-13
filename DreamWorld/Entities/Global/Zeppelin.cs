using System;
using System.Collections.Generic;
using DreamWorld.Audio;
using DreamWorld.Rendering.Particles.Systems;
using DreamWorld.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Entities.Global
{
    public abstract class Zeppelin : Entity
    {
        private Matrix _rotation;

        private readonly ZeppelinSmokeParticleSystem _smokeParticleSystem;

        private float _propellorRotation;
        private readonly List<ModelMesh> _propellors;
        private readonly ModelMesh[] _cogs;        
        
        private float _time;
        
        protected abstract float Speed { get; }
        protected abstract Curve3D Path { get; }

        protected Zeppelin()
        {
            _propellors = new List<ModelMesh>();
            _cogs = new ModelMesh[4];
            _smokeParticleSystem = new ZeppelinSmokeParticleSystem();            
        }

        public override void Initialize()
        {
            Level.AddParticleSystem(SpawnInformation.Name + "_smoke", _smokeParticleSystem);
                        
            base.Initialize();          
        }

        protected override void LoadContent()
        {
            Model = GameScreen.Content.Load<Model>(@"Models\Global\Zeppelin\Zeppelin");
            
            foreach (ModelMesh mesh in Model.Meshes)
            {
                String name = mesh.Name.ToLower();
                if (name.Contains("propellor"))
                    _propellors.Add(mesh);
                if (name.Contains("cog01"))
                    _cogs[0] = mesh;
                if (name.Contains("cog02"))
                    _cogs[1] = mesh;
                if (name.Contains("cog03"))
                    _cogs[2] = mesh;
                if (name.Contains("cog04"))
                    _cogs[3] = mesh;               
            }
            SoundEffect3D sound = new SoundEffect3D(this, GameScreen.Content.Load<SoundEffect>(@"Audio\Effects\Steam Engine"));
            Sounds.Add(sound);
            sound.Boost = .75f;
            sound.Play();

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _rotation = Matrix.Identity;
            if(Path != null)
            {
                _time += Speed;
                Body.Position = SpawnInformation.Position + Path.GetPointOnCurve(_time);
                
                // TODO: The way rotation is handled could be better.
                Vector3 lookAt = SpawnInformation.Position + Path.GetPointOnCurve(_time + Speed);
                lookAt.Y = Body.Position.Y;
                _rotation = Matrix.Invert(Matrix.CreateLookAt(Body.Position, lookAt, Vector3.Up)) * Matrix.CreateRotationY(MathHelper.Pi); // Note: 180 degrees turn maybe not necessary in the future
                _rotation.Translation = Vector3.Zero;                
            }

            _propellorRotation += Speed / MathHelper.TwoPi * 1.7f;
            foreach (ModelMesh mesh in _propellors)            
                mesh.ParentBone.Transform = Matrix.CreateRotationY(_propellorRotation);

            _cogs[0].ParentBone.Transform = Matrix.CreateRotationY(_propellorRotation * .15f); // Big, left
            _cogs[1].ParentBone.Transform = Matrix.CreateRotationY(_propellorRotation * .4f); // Small, left
            _cogs[2].ParentBone.Transform = Matrix.CreateRotationY(-_propellorRotation * .15f); // Big, right
            _cogs[3].ParentBone.Transform = Matrix.CreateRotationY(-_propellorRotation * .4f); // Small, right

            // Smoke particles
            Vector3 rightPropellor = Body.Position + Vector3.Transform(new Vector3(-68, 55, -60), _rotation);
            Vector3 leftPropellor = Body.Position + Vector3.Transform(new Vector3(68, 55, -60), _rotation);
            _smokeParticleSystem.AddParticle(rightPropellor, Vector3.Zero);
            _smokeParticleSystem.AddParticle(leftPropellor, Vector3.Zero);
                        
            Vector3 gravity = Vector3.Transform(Vector3.Backward, _rotation);
            gravity.Y = 0;
            _smokeParticleSystem.Settings.Gravity = gravity;                
            
            base.Update(gameTime);  
        }
       
        protected void FastForward(float time)
        {
            _time += time;
        }

        protected override Matrix GenerateWorldMatrix()
        {
            return Matrix.CreateScale(Scale) *
                    _rotation *
                    Matrix.CreateTranslation(Body.Position);       
        }
    }
}
