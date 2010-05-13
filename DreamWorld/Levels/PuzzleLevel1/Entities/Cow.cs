using System;
using DreamWorld.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Levels.PuzzleLevel1.Entities
{
    class Cow : GroupCenter
    {
        private const float Gravity = .05f;
        private float _velocity;
        private static readonly Random random = new Random();

        public Vector3 StartPosition = Vector3.Zero;

        public bool IsFalling
        {
            get { return _velocity < 0; }
        }

        public override void Initialize()
        {
            Animation.InitialClip = "Idle";
            Animation.Paused = true;
            
            base.Initialize();
            Body.Immovable = false;
            Respawn();
        }

        protected override void LoadContent()
        {
            Model = GameScreen.Content.Load<Model>(@"Models\Global\Cow");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Animation.Paused)
            {
                if (random.Next(75) == 1)
                    Animation.Paused = false;
            }

            float distance = Vector3.Distance(Body.Position, Group.Center.Body.Position);
            if (!IsFalling && (!(Group.Center is Cow) || !((Cow) Group.Center).IsFalling) &&
                (distance > 60.1 || distance < 59.9))
            {
                Vector3 direction = (Group.Center.Body.Position - Body.Position);
                direction.Normalize();
                Vector3 offset = direction*(distance - 60)/2;

                Body.Position += offset;
                Group.Center.Body.Position -= offset;
            }

            if (Group.IsRotating || IsFalling)
                Animation.StartClip("Flipping");
            else
            {
                if (!Animation.CurrentClip.Equals("Idle"))
                    Animation.ResetClip();
                Animation.StartClip("Idle");
            }

            if (!Group.IsRotating && !Group.IsColliding)
            {
                _velocity -= Gravity;
                Body.Position += new Vector3(0, _velocity, 0);
            }
            else
            {
                _velocity = 0;
            }
        }

        public void Respawn()
        {
            Animation.Paused = true;
            _velocity = 0;
            Body.MoveTo(StartPosition, Matrix.Identity);
        }

        protected override void GetPhysicsInformation(out JigLibX.Physics.Body body,
                                                      out JigLibX.Collision.CollisionSkin skin, out Vector3 centerOfMass)
        {
            #region Header

            // Variables
            JigLibX.Collision.MaterialProperties materialProperties;
            JigLibX.Geometry.PrimitiveProperties primitiveProperties;
            JigLibX.Geometry.Box box;

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
            materialProperties.Elasticity = 0.25f;

            // Primitive:
            box =
                new JigLibX.Geometry.Box(
                    Vector3.Transform(new Vector3(-20.59352f, -0.1788588f, -8.250205f), Matrix.CreateScale(Scale)),
                    Matrix.CreateFromQuaternion(new Quaternion(0f, 0f, 0f, 0.9999999f)),
                    Vector3.Transform(new Vector3(40.6418f, 25.50671f, 15.746f), Matrix.CreateScale(Scale)));

            skin.AddPrimitive(box, materialProperties);

            #endregion

            #region Footer

            // Extract Mass Properties
            skin.GetMassProperties(primitiveProperties, out mass, out centerOfMass, out inertiaTensor,
                                   out inertiaTensorCoM);

            // Set Mass Properties
            body.BodyInertia = inertiaTensorCoM;
            body.Mass = mass;

            // Sync Body & Skin
            body.MoveTo(Vector3.Zero, Matrix.Identity);
            skin.ApplyLocalTransform(new JigLibX.Math.Transform(Vector3.Zero, Matrix.CreateScale(1, 1, -1)));

            // Enable Body
            body.EnableBody();

            #endregion
        }
    }
}