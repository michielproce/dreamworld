﻿using System;
using System.Collections.Generic;
using DreamWorld.Audio;
using DreamWorld.Cameras;
using DreamWorld.Entities.Global;
using DreamWorld.InputManagement;
using DreamWorld.Levels;
using DreamWorld.Levels.VillageLevel;
using JigLibX.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Entities
{
    public class Player : Entity
    {
        private static readonly Random random = new Random();

        enum PlayerState
        {
            OnTerrain,
            OnPlatform,
            Jumping
        }

        private const float MovementSpeed = .7f;
        private const float JumpStart = 1f;
        private const float JumpGravity = .05f;
        private float _jumpVelocity;

        private PlayerState _playerState = PlayerState.OnTerrain;

        public readonly Vector3 CameraOffset = new Vector3(0,12,0);

        private InputManager _inputManager;

        public Vector3 SpawnPosition { get; set;}
        public Matrix SpawnOrientation { get; set; }

        private SoundEffect3D _footStepSound;
        private List<SoundEffect3D> _jumpSounds;

        public override void Initialize()
        {
            _inputManager = GameScreen.InputManager;
            Animation.InitialClip = "Standing";
            Scale = new Vector3(.12f);

            base.Initialize();

            Body.Immovable = false;

            PutOnTerrain();
        }

        protected override void LoadContent()
        {            
            Model = GameScreen.Content.Load<Model>(@"Models\Global\Player");                       
            base.LoadContent();

            _footStepSound = new SoundEffect3D(this, GameScreen.Content.Load<SoundEffect>(@"Audio\Effects\Footstep"));
            _footStepSound.Boost = .8f;

            _jumpSounds = new List<SoundEffect3D>();
            for (int i = 1; i <= 3; i++)
            {
                SoundEffect3D jumpSound = new SoundEffect3D(this, GameScreen.Content.Load<SoundEffect>(@"Audio\Effects\Jump" + i), false);
                _jumpSounds.Add(jumpSound);
                jumpSound.Boost = .9f;
                Sounds.Add(jumpSound);                
            }

            Sounds.Add(_footStepSound);
        }

        public override void Update(GameTime gameTime)
        {
            #if(DEBUG && !XBOX)
                if (GameScreen.Camera is DebugCamera)
                    return;
            #endif

            Body.Orientation *= Matrix.CreateRotationY(_inputManager.Player.Rotation);

            Vector3 movement = Vector3.Transform(_inputManager.Player.Movement * MovementSpeed, Body.Orientation);

            if (IsOnTerrain())
            {
                PutOnTerrain();
            } 
            else if (IsOnPlatform())
            {
                _playerState = PlayerState.OnPlatform;
                _jumpVelocity = 0;
            }
            else
            {
                _playerState = PlayerState.Jumping;
            }

            if (_inputManager.Player.Jump && _playerState != PlayerState.Jumping)
                StartJumping();

            if (_playerState == PlayerState.Jumping)
            {
                _jumpVelocity -= JumpGravity;
                movement.Y = _jumpVelocity;
            }

            for (int i = 0; i <= Skin.Collisions.Count - 1; i++)
            {
                float dot = Vector3.Dot(movement, Skin.Collisions[i].DirToBody0);
                if (dot < 0)
                {
                    if (!IsCheckPoint(Skin.Collisions[i].SkinInfo.Skin1))
                    {
                        movement -= Skin.Collisions[i].DirToBody0*dot;

                        // Check if this conflicts with other collisionskins
                        // TODO: do mathmatical magic to calculate a direction that doesn't conflict with any skin
                        for (int j = i - 1; j >= 0; j--)
                            if (Vector3.Dot(movement, Skin.Collisions[j].DirToBody0) < 0)
                                movement = Vector3.Zero;
                    }

                }
            }

            if(Level is VillageLevel)
            {
                float currentHeight = Level.Terrain.HeightMapInfo.GetHeight(Body.Position);
                float nextHeight = Level.Terrain.HeightMapInfo.GetHeight(Body.Position + movement);
                if (Body.Position.Y > ((VillageLevel)Level).MaximumWalkingHeight && currentHeight < nextHeight)
                    movement = new Vector3(0, movement.Y, 0);
            }

            if(_playerState == PlayerState.Jumping)
                _jumpVelocity = movement.Y;

            Body.Position += movement;

            if (_playerState != PlayerState.Jumping)
            {
                if (new Vector2(movement.X, movement.Z).Length() != 0)
                {
                    _footStepSound.Play();
                    Animation.StartClip("Running");
                }
                else
                {
                    _footStepSound.Stop();
                    Animation.StartClip("Standing");
                }
            } 


            base.Update(gameTime);
        }

        private void StartJumping()
        {
            _footStepSound.Stop();
            _jumpSounds[random.Next(_jumpSounds.Count)].Play();
            Animation.StartClip("Jumping");
            _playerState = PlayerState.Jumping;
            _jumpVelocity = JumpStart;
        }

        private bool IsOnTerrain()
        {
            return Level.Terrain != null &&
                   Body.Position.Y - Level.Terrain.HeightMapInfo.GetHeight(Body.Position) <= 0;
        }

        private bool IsOnPlatform()
        {
            for (int i = 0; i <= Skin.Collisions.Count - 1; i++)
            {
                if (!IsCheckPoint(Skin.Collisions[i].SkinInfo.Skin1) && Skin.Collisions[i].DirToBody0.Y > .4f)
                    return true;
            }
            return false;
        }
        
        private void PutOnTerrain()
        {
            if (Level.Terrain != null)
            {
                Body.Position = new Vector3(Body.Position.X, Level.Terrain.HeightMapInfo.GetHeight(Body.Position), Body.Position.Z);
                _jumpVelocity = 0;
                _playerState = PlayerState.OnTerrain;
            }
        }

        private bool IsCheckPoint(CollisionSkin skin)
        {
            // Check checkpoints in Puzzle levels.
            if (GameScreen.Level is PuzzleLevel)
            {
                foreach (Group group in GameScreen.Level.Groups.Values)
                    foreach (Entity entity in group.Entities.Values)
                        if (entity is CheckPoint && entity.Skin == skin)
                        {
                            SpawnPosition = entity.Body.Position;
                            SpawnOrientation = entity.Body.Orientation;
                            return true;
                        }
            }
            return false;
        }

        public void Respawn()
        {
            _jumpVelocity = 0;
            Body.MoveTo(SpawnPosition, SpawnOrientation);
        }

        protected override void GetPhysicsInformation(out JigLibX.Physics.Body body, out CollisionSkin skin, out Vector3 centerOfMass)
        {
            #region Header
            // Variables
            JigLibX.Collision.MaterialProperties materialProperties;
            JigLibX.Geometry.PrimitiveProperties primitiveProperties;
            JigLibX.Geometry.Capsule capsule;

            #region Mass Variables
            float mass;
            Matrix inertiaTensor;
            Matrix inertiaTensorCoM;
            #endregion

            // Create Skin & Body
            body = new JigLibX.Physics.Body();
            skin = new JigLibX.Collision.CollisionSkin(body);
            body.CollisionSkin = skin;
            #endregion

            #region Primitive Properties
            primitiveProperties = new JigLibX.Geometry.PrimitiveProperties();
            primitiveProperties.MassDistribution = JigLibX.Geometry.PrimitiveProperties.MassDistributionEnum.Solid;
            primitiveProperties.MassType = JigLibX.Geometry.PrimitiveProperties.MassTypeEnum.Mass;
            primitiveProperties.MassOrDensity = 0.001f;
            #endregion

            #region Primitive 0
            // MaterialProperties:
            materialProperties = new JigLibX.Collision.MaterialProperties();
            materialProperties.StaticRoughness = 0.5f;
            materialProperties.DynamicRoughness = 0.35f;
            materialProperties.Elasticity = 0.0025f;

            // Primitive:
            capsule = new JigLibX.Geometry.Capsule(new Vector3(0, 8.8f, 0),
                Matrix.CreateRotationX(MathHelper.PiOver2),
                2.8f,
                6f);

            skin.AddPrimitive(capsule, materialProperties);
            #endregion

            #region Footer
            // Extract Mass Properties
            skin.GetMassProperties(primitiveProperties, out mass, out centerOfMass, out inertiaTensor, out inertiaTensorCoM);

            // Set Mass Properties
            body.BodyInertia = inertiaTensorCoM;
            body.Mass = mass;

            // Sync Body & Skin
            body.MoveTo(Vector3.Zero, Matrix.Identity);
            skin.ApplyLocalTransform(new JigLibX.Math.Transform(Vector3.Zero, Matrix.Identity));

            // Enable Body
            body.EnableBody();
            #endregion
        }

        // TODO: This is a hack for the rotated player model.
        // When the model is correct you can remove this entire method.
        protected override Matrix GenerateWorldMatrix()
        {
            return
                Matrix.CreateScale(Scale) *
                Body.Orientation * Matrix.CreateRotationY(MathHelper.Pi)*
                Matrix.CreateTranslation(Body.Position - new Vector3(0,0.5f,0));            
        }
    }
}
