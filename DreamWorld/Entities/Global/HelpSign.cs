using DreamWorld.Interface.Help;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Entities.Global
{
    public class HelpSign : Entity
    {
        public static bool ListInEditor = true;
        protected override void LoadContent()
        {
            Model = GameScreen.Content.Load<Model>(@"Models\Global\HelpSign");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            // If we're close to this entity, and we have a help item, set this as the active helper.
            if (Vector3.Distance(Level.Player.Body.Position, Body.Position) <= Help.HelpDistance
                && Help.Instance.HasHelp(this))
            {
                GameScreen.HelpSystem.Helper = this;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Creates all needed object information for the JigLibX physics simulator.
        /// </summary>
        /// <param name="body">Returns Body.</param>
        /// <param name="skin">Returns CollisionSkin.</param>
        /// <param name="centerOfMass">Returns the center of mass wich is usefull for drawing objects.</param>
        protected override void GetPhysicsInformation(out JigLibX.Physics.Body body, out JigLibX.Collision.CollisionSkin skin, out Vector3 centerOfMass)
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
            box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(-0.9407876f, 0.1776333f, -0.7712455f), Matrix.CreateScale(Scale)),
                Matrix.CreateFromQuaternion(new Quaternion(0f, 0f, 0f, 0.9999999f)),
                Vector3.Transform(new Vector3(1.95115f, 17.23676f, 1.587161f), Matrix.CreateScale(Scale)));

            skin.AddPrimitive(box, materialProperties);
            #endregion

            #region Primitive 1
            // MaterialProperties:
            materialProperties = new JigLibX.Collision.MaterialProperties();
            materialProperties.StaticRoughness = 0.5f;
            materialProperties.DynamicRoughness = 0.35f;
            materialProperties.Elasticity = 0.25f;

            // Primitive:
            box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(-9.185594f, 17.11884f, -0.8012028f), Matrix.CreateScale(Scale)),
                Matrix.CreateFromQuaternion(new Quaternion(0f, 0f, 0f, 0.9999999f)),
                Vector3.Transform(new Vector3(18.19898f, 9.549535f, 1.579426f), Matrix.CreateScale(Scale)));

            skin.AddPrimitive(box, materialProperties);
            #endregion

            #region Footer
            // Extract Mass Properties
            skin.GetMassProperties(primitiveProperties, out mass, out centerOfMass, out inertiaTensor, out inertiaTensorCoM);

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
