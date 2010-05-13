using DreamWorld.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Levels.VillageLevel.Entities
{
    public class Tree : Entity
    {
        public static bool ListInEditor = true;
        protected override void LoadContent()
        {
            Model = GameScreen.Content.Load<Model>(@"Models\Village\Tree");
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
            box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(-4.279139f, -0.03513908f, -3.72629f), Matrix.CreateScale(Scale)),
		        Matrix.CreateFromQuaternion(new Quaternion(0f, 0f, 0f, 0.9999999f)),
		        Vector3.Transform(new Vector3(9.576451f, 50.00568f, 9.14602f), Matrix.CreateScale(Scale)));
        	
	        skin.AddPrimitive(box, materialProperties);
	        #endregion
        	
	        #region Primitive 1
	        // MaterialProperties:
	        materialProperties = new JigLibX.Collision.MaterialProperties();
	        materialProperties.StaticRoughness = 0.5f;
	        materialProperties.DynamicRoughness = 0.35f;
	        materialProperties.Elasticity = 0.25f;
        	
	        // Primitive:
	        box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(1.044252f, 0.155241f, -6.357302f), Matrix.CreateScale(Scale)),
		        Matrix.CreateFromQuaternion(new Quaternion(-4.025135E-05f, -0.4033178f, 5.515133E-07f, 0.9150599f)),
		        Vector3.Transform(new Vector3(10.1043f, 50.1779f, 9.779099f), Matrix.CreateScale(Scale)));
        	
	        skin.AddPrimitive(box, materialProperties);
	        #endregion
        	
	        #region Primitive 2
	        // MaterialProperties:
	        materialProperties = new JigLibX.Collision.MaterialProperties();
	        materialProperties.StaticRoughness = 0.5f;
	        materialProperties.DynamicRoughness = 0.35f;
	        materialProperties.Elasticity = 0.25f;
        	
	        // Primitive:
	        box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(-0.5846357f, 23.92927f, -11.17512f), Matrix.CreateScale(Scale)),
		        Matrix.CreateFromQuaternion(new Quaternion(-0.2050987f, -0.0165523f, 0.01240025f, 0.9785228f)),
		        Vector3.Transform(new Vector3(7.391443f, 37.74773f, 19.01f), Matrix.CreateScale(Scale)));
        	
	        skin.AddPrimitive(box, materialProperties);
	        #endregion
        	
	        #region Primitive 3
	        // MaterialProperties:
	        materialProperties = new JigLibX.Collision.MaterialProperties();
	        materialProperties.StaticRoughness = 0.5f;
	        materialProperties.DynamicRoughness = 0.35f;
	        materialProperties.Elasticity = 0.25f;
        	
	        // Primitive:
	        box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(-6.175274f, 50.19572f, -3.642176f), Matrix.CreateScale(Scale)),
		        Matrix.CreateFromQuaternion(new Quaternion(0.1531299f, 0.0389909f, 0.002142292f, 0.9874341f)),
		        Vector3.Transform(new Vector3(9.790846f, 74.62482f, 10.63992f), Matrix.CreateScale(Scale)));
        	
	        skin.AddPrimitive(box, materialProperties);
	        #endregion

            #region Primitive 4
            // MaterialProperties:
            materialProperties = new JigLibX.Collision.MaterialProperties();
            materialProperties.StaticRoughness = 0.5f;
            materialProperties.DynamicRoughness = 0.35f;
            materialProperties.Elasticity = 0.25f;

            // Primitive:
            box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(-0.1591086f, 43.53473f, 9.915249f), Matrix.CreateScale(Scale)),
                Matrix.CreateFromQuaternion(new Quaternion(-0.3266442f, 0.1223507f, 0.08217592f, 0.933585f)),
                Vector3.Transform(new Vector3(6.703924f, 11.50876f, 13.70471f), Matrix.CreateScale(Scale)));

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
