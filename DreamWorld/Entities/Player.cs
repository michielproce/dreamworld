using System;
using DreamWorld.Cameras;
using DreamWorld.InputManagement;
using JigLibX.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Entities
{
    public class Player : Entity
    {
        enum PlayerState
        {
            OnTerrain,
            OnPlatform,
            Jumping
        }

        private const float jumpStart = 1.5f;
        private const float jumpGravity = .05f;
        private float jumpVelocity;

        private PlayerState playerState = PlayerState.OnTerrain;

        public readonly Vector3 CameraOffset = new Vector3(0,3,0);

        private InputManager inputManager;

        public override void Initialize()
        {
            inputManager = GameScreen.InputManager;
            Animation.InitialClip = "Idle";
            Animation.Speed = 1.0f;
            Scale = new Vector3(.15f);

            base.Initialize();

            Body.Immovable = false;

            PutOnTerrain();
        }

        protected override void LoadContent()
        {            
            Model = GameScreen.Content.Load<Model>(@"Models\Global\Player");                       
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            #if(DEBUG)
                if (GameScreen.Camera is DebugCamera)
                    return;
            #endif

            Body.Orientation *= Matrix.CreateRotationY(inputManager.Player.Rotation);

            Vector3 movement = Vector3.Transform(inputManager.Player.Movement, Body.Orientation);

            if (IsOnTerrain())
            {
                PutOnTerrain();
            } 
            else if (IsOnPlatform())
            {
                playerState = PlayerState.OnPlatform;
                jumpVelocity = 0;
            }
            else
            {
                playerState = PlayerState.Jumping;
            }

            if (inputManager.Player.Jump && playerState != PlayerState.Jumping)
                StartJumping();

            if (playerState == PlayerState.Jumping)
            {
                jumpVelocity -= jumpGravity;
                movement.Y = jumpVelocity;
            }

            for (int i = 0; i <= Skin.Collisions.Count - 1; i++)
            {
                float dot = Vector3.Dot(movement, Skin.Collisions[i].DirToBody0);
                if (dot < 0)
                    movement -= Skin.Collisions[i].DirToBody0 * dot;
            }

            jumpVelocity = movement.Y;

            Body.Position += movement;

            if (playerState != PlayerState.Jumping)
            {
                if (inputManager.Player.Movement.Length() != 0)
                    Animation.StartClip("Run");
                else
                    Animation.StartClip("Idle");
            }


            base.Update(gameTime);
        }

        private void StartJumping()
        {
            Animation.StartClip("Jump");
            playerState = PlayerState.Jumping;
            jumpVelocity = jumpStart;
        }

        private bool IsOnTerrain()
        {
            return Level.Terrain != null &&
                   Body.Position.Y - CenterOfMass.Y - Level.Terrain.HeightMapInfo.GetHeight(Body.Position + CenterOfMass) <= 0;
        }

        private bool IsOnPlatform()
        {
            for (int i = 0; i <= Skin.Collisions.Count - 1; i++)
            {
                if (Skin.Collisions[i].DirToBody0.Y > .4f)
                    return true;
            }
            return false;
        }
        
        private void PutOnTerrain()
        {
            if (Level.Terrain != null)
            {
                Body.Position = new Vector3(Body.Position.X, Level.Terrain.HeightMapInfo.GetHeight(Body.Position+CenterOfMass) + CenterOfMass.Y, Body.Position.Z);
                jumpVelocity = 0;
                playerState = PlayerState.OnTerrain;
            }
        }

        protected override void GetPhysicsInformation(out JigLibX.Physics.Body body, out JigLibX.Collision.CollisionSkin skin, out Vector3 centerOfMass)
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
            capsule = new JigLibX.Geometry.Capsule(new Vector3(0, 11, 0),
                Matrix.CreateRotationX(MathHelper.PiOver2),
                4,
                7f);

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
            skin.ApplyLocalTransform(new JigLibX.Math.Transform(-centerOfMass, Matrix.Identity));

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
                Matrix.CreateTranslation(-CenterOfMass) *
                Body.Orientation * Matrix.CreateRotationY(-MathHelper.PiOver2)*
                Matrix.CreateTranslation(Body.Position);            
        }
    }
}
