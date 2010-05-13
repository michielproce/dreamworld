using DreamWorld.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Levels.PuzzleLevel1.Entities
{
    class Stairs : Entity
    {
        public static bool ListInEditor = true;

        protected override void LoadContent()
        {
            Model = GameScreen.Content.Load<Model>(@"Models\Puzzle\Level1\Stairs");

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
            box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(0f, 0f, 0f), Matrix.CreateScale(Scale)),
                Matrix.CreateFromQuaternion(new Quaternion(0f, 0f, 0f, 0.9999999f)),
                Vector3.Transform(new Vector3(10f, 2f, 20f), Matrix.CreateScale(Scale)));

            skin.AddPrimitive(box, materialProperties);
            #endregion

            #region Primitive 1
            // MaterialProperties:
            materialProperties = new JigLibX.Collision.MaterialProperties();
            materialProperties.StaticRoughness = 0.5f;
            materialProperties.DynamicRoughness = 0.35f;
            materialProperties.Elasticity = 0.25f;

            // Primitive:
            box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(13.49716f, 4.97757f, -0.003091812f), Matrix.CreateScale(Scale)),
                Matrix.CreateFromQuaternion(new Quaternion(0f, 0f, 0f, 0.9999999f)),
                Vector3.Transform(new Vector3(3.185906f, 2.044872f, 10.00039f), Matrix.CreateScale(Scale)));

            skin.AddPrimitive(box, materialProperties);
            #endregion

            #region Primitive 2
            // MaterialProperties:
            materialProperties = new JigLibX.Collision.MaterialProperties();
            materialProperties.StaticRoughness = 0.5f;
            materialProperties.DynamicRoughness = 0.35f;
            materialProperties.Elasticity = 0.25f;

            // Primitive:
            box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(20f, 10f, 0f), Matrix.CreateScale(Scale)),
                Matrix.CreateFromQuaternion(new Quaternion(0f, 0f, 0f, 0.9999999f)),
                Vector3.Transform(new Vector3(10f, 2f, 20f), Matrix.CreateScale(Scale)));

            skin.AddPrimitive(box, materialProperties);
            #endregion

            #region Primitive 3
            // MaterialProperties:
            materialProperties = new JigLibX.Collision.MaterialProperties();
            materialProperties.StaticRoughness = 0.5f;
            materialProperties.DynamicRoughness = 0.35f;
            materialProperties.Elasticity = 0.25f;

            // Primitive:
            box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(10.43245f, 0.5228279f, -0.09022045f), Matrix.CreateScale(Scale)),
                Matrix.CreateFromQuaternion(new Quaternion(0f, 0f, 0.4787785f, 0.8779357f)),
                Vector3.Transform(new Vector3(7.161446f, 1.203526f, 10.28542f), Matrix.CreateScale(Scale)));

            skin.AddPrimitive(box, materialProperties);
            #endregion	
            
            #region Primitive 4
	        // MaterialProperties:
	        materialProperties = new JigLibX.Collision.MaterialProperties();
	        materialProperties.StaticRoughness = 0.5f;
	        materialProperties.DynamicRoughness = 0.35f;
	        materialProperties.Elasticity = 0.25f;
        	
	        // Primitive:
            box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(17.09687f, 5.478969f, -0.5655527f), Matrix.CreateScale(Scale)),
		        Matrix.CreateFromQuaternion(new Quaternion(0f, 0f, 0.4787785f, 0.8779357f)),
		        Vector3.Transform(new Vector3(7.161446f, 1.203526f, 10.28542f), Matrix.CreateScale(Scale)));
        	
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