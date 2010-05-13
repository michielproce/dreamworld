using DreamWorld.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Levels.VillageLevel.Entities
{
    public class Gate : Entity
    {
        public static bool ListInEditor = true;
        protected override void LoadContent()
        {
            Model = GameScreen.Content.Load<Model>(@"Models\Village\Gate");
            base.LoadContent();
        }

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
            box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(-57.56879f, 0.3336029f, -6.107193f), Matrix.CreateScale(Scale)),
                Matrix.CreateFromQuaternion(new Quaternion(0f, 0f, 0f, 0.9999999f)),
                Vector3.Transform(new Vector3(114.9535f, 67.46237f, 6.150543f), Matrix.CreateScale(Scale)));

            skin.AddPrimitive(box, materialProperties);
            #endregion

            #region Primitive 1
            // MaterialProperties:
            materialProperties = new JigLibX.Collision.MaterialProperties();
            materialProperties.StaticRoughness = 0.5f;
            materialProperties.DynamicRoughness = 0.35f;
            materialProperties.Elasticity = 0.25f;

            // Primitive:
            box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(-56.45338f, 0.4354181f, -17.75252f), Matrix.CreateScale(Scale)),
                Matrix.CreateFromQuaternion(new Quaternion(0f, 0f, 0f, 0.9999999f)),
                Vector3.Transform(new Vector3(7.790543f, 21.50752f, 12.56228f), Matrix.CreateScale(Scale)));

            skin.AddPrimitive(box, materialProperties);
            #endregion

            #region Primitive 2
            // MaterialProperties:
            materialProperties = new JigLibX.Collision.MaterialProperties();
            materialProperties.StaticRoughness = 0.5f;
            materialProperties.DynamicRoughness = 0.35f;
            materialProperties.Elasticity = 0.25f;

            // Primitive:
            box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(47.4451f, -0.8470869f, -17.49867f), Matrix.CreateScale(Scale)),
                Matrix.CreateFromQuaternion(new Quaternion(0f, 0f, 0f, 0.9999999f)),
                Vector3.Transform(new Vector3(8.221245f, 18.34381f, 10.54014f), Matrix.CreateScale(Scale)));

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
