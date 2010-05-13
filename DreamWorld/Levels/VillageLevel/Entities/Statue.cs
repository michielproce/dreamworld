using System;
using DreamWorld.Entities;
using DreamWorld.Rendering.Particles.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Levels.VillageLevel.Entities
{
    public class Statue : Entity
    {
        private const string FoodSymbol = "mesh_cowSymbol";

        private static Random random = new Random();
        private int _frames;
        private GoldSparkleParticleSystem _particleSystem;

        public static bool ListInEditor = true;

        public override void Initialize()
        {
            _particleSystem = new GoldSparkleParticleSystem();
            Level.AddParticleSystem(Name + "_particleSystem", _particleSystem);

            base.Initialize();
        }

        protected override void LoadContent()
        {

            Model = GameScreen.Content.Load<Model>(@"Models\Village\Statue");
            base.LoadContent();
        } 

        public override void Draw(GameTime gameTime, string technique)
        {
            VillageLevel vl = Level as VillageLevel;
            if (vl != null && vl.CurrentStage == VillageLevel.Stage.FinishedPuzzle1)
            {
                Vector3 offset = new Vector3(0, (float)(random.NextDouble() - .5) * 3, (float)(random.NextDouble() - .5) * 3);
                if (_frames++ % 30 == 0)
                    _particleSystem.AddParticle(Body.Position + new Vector3(-10, 29, 1) + offset, Vector3.Zero);

                Model.CopyAbsoluteBoneTransformsTo(Transforms);
                foreach (ModelMesh mesh in Model.Meshes)
                {
                    bool isCow = FoodSymbol.Equals(mesh.Name);
                    foreach (Effect effect in mesh.Effects)
                    {
                        effect.CurrentTechnique = isCow ? effect.Techniques["Highlight"] : effect.Techniques["Default"];
                        effect.Parameters["world"].SetValue(Transforms[mesh.ParentBone.Index]*World);
                        effect.Parameters["view"].SetValue(GameScreen.Camera.View);
                        effect.Parameters["projection"].SetValue(GameScreen.Camera.Projection);
                        if (isCow)
                            effect.Parameters["Ambient"].SetValue(Color.Gold.ToVector3());
                    }                    
                    mesh.Draw();
                }
            } 
            else
                base.Draw(gameTime, technique);
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
            box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(-2.042447f, -0.03338391f, -1.104232f), Matrix.CreateScale(Scale)),
                Matrix.CreateFromQuaternion(new Quaternion(-0.01284643f, -0.13176f, 0.1374352f, 0.9816241f)),
                Vector3.Transform(new Vector3(1.273594f, 0.1210749f, 1.085283f), Matrix.CreateScale(Scale)));

            skin.AddPrimitive(box, materialProperties);
            #endregion

            #region Primitive 1
            // MaterialProperties:
            materialProperties = new JigLibX.Collision.MaterialProperties();
            materialProperties.StaticRoughness = 0.5f;
            materialProperties.DynamicRoughness = 0.35f;
            materialProperties.Elasticity = 0.25f;

            // Primitive:
            box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(-1.273293f, -0.03294668f, -1.943602f), Matrix.CreateScale(Scale)),
                Matrix.CreateFromQuaternion(new Quaternion(-0.04821458f, -0.3831583f, 0.1293432f, 0.9133101f)),
                Vector3.Transform(new Vector3(1.26149f, 0.1178056f, 1.017585f), Matrix.CreateScale(Scale)));

            skin.AddPrimitive(box, materialProperties);
            #endregion

            #region Primitive 2
            // MaterialProperties:
            materialProperties = new JigLibX.Collision.MaterialProperties();
            materialProperties.StaticRoughness = 0.5f;
            materialProperties.DynamicRoughness = 0.35f;
            materialProperties.Elasticity = 0.25f;

            // Primitive:
            box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(-0.06900755f, -0.04147579f, -2.335705f), Matrix.CreateScale(Scale)),
                Matrix.CreateFromQuaternion(new Quaternion(-0.0802528f, -0.6079859f, 0.112326f, 0.7818539f)),
                Vector3.Transform(new Vector3(1.273594f, 0.1210749f, 1.085283f), Matrix.CreateScale(Scale)));

            skin.AddPrimitive(box, materialProperties);
            #endregion

            #region Primitive 3
            // MaterialProperties:
            materialProperties = new JigLibX.Collision.MaterialProperties();
            materialProperties.StaticRoughness = 0.5f;
            materialProperties.DynamicRoughness = 0.35f;
            materialProperties.Elasticity = 0.25f;

            // Primitive:
            box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(1.069988f, -0.03051516f, -2.021573f), Matrix.CreateScale(Scale)),
                Matrix.CreateFromQuaternion(new Quaternion(-0.1073609f, -0.794663f, 0.08684432f, 0.5911365f)),
                Vector3.Transform(new Vector3(1.273594f, 0.1210749f, 1.085283f), Matrix.CreateScale(Scale)));

            skin.AddPrimitive(box, materialProperties);
            #endregion

            #region Primitive 4
            // MaterialProperties:
            materialProperties = new JigLibX.Collision.MaterialProperties();
            materialProperties.StaticRoughness = 0.5f;
            materialProperties.DynamicRoughness = 0.35f;
            materialProperties.Elasticity = 0.25f;

            // Primitive:
            box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(1.933133f, -0.03386061f, -1.171714f), Matrix.CreateScale(Scale)),
                Matrix.CreateFromQuaternion(new Quaternion(0.1264879f, 0.9225684f, -0.05542238f, -0.3602731f)),
                Vector3.Transform(new Vector3(1.273594f, 0.1210749f, 1.085283f), Matrix.CreateScale(Scale)));

            skin.AddPrimitive(box, materialProperties);
            #endregion

            #region Primitive 5
            // MaterialProperties:
            materialProperties = new JigLibX.Collision.MaterialProperties();
            materialProperties.StaticRoughness = 0.5f;
            materialProperties.DynamicRoughness = 0.35f;
            materialProperties.Elasticity = 0.25f;

            // Primitive:
            box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(2.253327f, -0.03353542f, -0.0397355f), Matrix.CreateScale(Scale)),
                Matrix.CreateFromQuaternion(new Quaternion(0.1364985f, 0.984121f, -0.02109852f, -0.1114841f)),
                Vector3.Transform(new Vector3(1.273594f, 0.1210749f, 1.085283f), Matrix.CreateScale(Scale)));

            skin.AddPrimitive(box, materialProperties);
            #endregion

            #region Primitive 6
            // MaterialProperties:
            materialProperties = new JigLibX.Collision.MaterialProperties();
            materialProperties.StaticRoughness = 0.5f;
            materialProperties.DynamicRoughness = 0.35f;
            materialProperties.Elasticity = 0.25f;

            // Primitive:
            box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(1.976751f, -0.02303737f, 1.087355f), Matrix.CreateScale(Scale)),
                Matrix.CreateFromQuaternion(new Quaternion(0.1373772f, 0.9800579f, 0.01437698f, 0.1428541f)),
                Vector3.Transform(new Vector3(1.273594f, 0.1210749f, 1.085283f), Matrix.CreateScale(Scale)));

            skin.AddPrimitive(box, materialProperties);
            #endregion

            #region Primitive 7
            // MaterialProperties:
            materialProperties = new JigLibX.Collision.MaterialProperties();
            materialProperties.StaticRoughness = 0.5f;
            materialProperties.DynamicRoughness = 0.35f;
            materialProperties.Elasticity = 0.25f;

            // Primitive:
            box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(1.173185f, -0.01837546f, 1.947948f), Matrix.CreateScale(Scale)),
                Matrix.CreateFromQuaternion(new Quaternion(0.129325f, 0.9124895f, 0.04851059f, 0.3850774f)),
                Vector3.Transform(new Vector3(1.273594f, 0.1210749f, 1.085283f), Matrix.CreateScale(Scale)));

            skin.AddPrimitive(box, materialProperties);
            #endregion

            #region Primitive 8
            // MaterialProperties:
            materialProperties = new JigLibX.Collision.MaterialProperties();
            materialProperties.StaticRoughness = 0.5f;
            materialProperties.DynamicRoughness = 0.35f;
            materialProperties.Elasticity = 0.25f;

            // Primitive:
            box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(0.04177752f, -0.02590962f, 2.300441f), Matrix.CreateScale(Scale)),
                Matrix.CreateFromQuaternion(new Quaternion(0.1126271f, 0.7838972f, 0.07995252f, 0.6053329f)),
                Vector3.Transform(new Vector3(1.273594f, 0.1210749f, 1.085283f), Matrix.CreateScale(Scale)));

            skin.AddPrimitive(box, materialProperties);
            #endregion

            #region Primitive 9
            // MaterialProperties:
            materialProperties = new JigLibX.Collision.MaterialProperties();
            materialProperties.StaticRoughness = 0.5f;
            materialProperties.DynamicRoughness = 0.35f;
            materialProperties.Elasticity = 0.25f;

            // Primitive:
            box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(-1.085008f, -0.01800987f, 2.053501f), Matrix.CreateScale(Scale)),
                Matrix.CreateFromQuaternion(new Quaternion(0.08953092f, 0.6113815f, 0.1051476f, 0.7791923f)),
                Vector3.Transform(new Vector3(1.273594f, 0.1210749f, 1.085283f), Matrix.CreateScale(Scale)));

            skin.AddPrimitive(box, materialProperties);
            #endregion

            #region Primitive 10
            // MaterialProperties:
            materialProperties = new JigLibX.Collision.MaterialProperties();
            materialProperties.StaticRoughness = 0.5f;
            materialProperties.DynamicRoughness = 0.35f;
            materialProperties.Elasticity = 0.25f;

            // Primitive:
            box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(-1.94619f, -0.00250642f, 1.239705f), Matrix.CreateScale(Scale)),
                Matrix.CreateFromQuaternion(new Quaternion(0.06018217f, 0.3957556f, 0.1242637f, 0.9079175f)),
                Vector3.Transform(new Vector3(1.273594f, 0.1210749f, 1.085283f), Matrix.CreateScale(Scale)));

            skin.AddPrimitive(box, materialProperties);
            #endregion

            #region Primitive 11
            // MaterialProperties:
            materialProperties = new JigLibX.Collision.MaterialProperties();
            materialProperties.StaticRoughness = 0.5f;
            materialProperties.DynamicRoughness = 0.35f;
            materialProperties.Elasticity = 0.25f;

            // Primitive:
            box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(-2.309048f, -0.01767018f, 0.06278136f), Matrix.CreateScale(Scale)),
                Matrix.CreateFromQuaternion(new Quaternion(0.02394645f, 0.1325801f, 0.1359582f, 0.9815113f)),
                Vector3.Transform(new Vector3(1.273594f, 0.1210749f, 1.085283f), Matrix.CreateScale(Scale)));

            skin.AddPrimitive(box, materialProperties);
            #endregion

            #region Primitive 12
            // MaterialProperties:
            materialProperties = new JigLibX.Collision.MaterialProperties();
            materialProperties.StaticRoughness = 0.5f;
            materialProperties.DynamicRoughness = 0.35f;
            materialProperties.Elasticity = 0.25f;

            // Primitive:
            box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(-0.3102444f, 0.3318961f, -1.109585f), Matrix.CreateScale(Scale)),
                Matrix.CreateFromQuaternion(new Quaternion(0f, 0f, 0f, 0.9999999f)),
                Vector3.Transform(new Vector3(0.6189461f, 1.000128f, 2.261881f), Matrix.CreateScale(Scale)));

            skin.AddPrimitive(box, materialProperties);
            #endregion

            #region Primitive 13
            // MaterialProperties:
            materialProperties = new JigLibX.Collision.MaterialProperties();
            materialProperties.StaticRoughness = 0.5f;
            materialProperties.DynamicRoughness = 0.35f;
            materialProperties.Elasticity = 0.25f;

            // Primitive:
            box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(0.3127279f, 0.3318929f, -1.108474f), Matrix.CreateScale(Scale)),
                Matrix.CreateFromQuaternion(new Quaternion(-1.404421E-06f, -0.2656454f, 2.652548E-05f, 0.9640708f)),
                Vector3.Transform(new Vector3(0.6189461f, 1.000128f, 2.261881f), Matrix.CreateScale(Scale)));

            skin.AddPrimitive(box, materialProperties);
            #endregion

            #region Primitive 14
            // MaterialProperties:
            materialProperties = new JigLibX.Collision.MaterialProperties();
            materialProperties.StaticRoughness = 0.5f;
            materialProperties.DynamicRoughness = 0.35f;
            materialProperties.Elasticity = 0.25f;

            // Primitive:
            box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(0.8332851f, 0.3319148f, -0.8027613f), Matrix.CreateScale(Scale)),
                Matrix.CreateFromQuaternion(new Quaternion(-5.200357E-06f, -0.5048661f, 5.021806E-05f, 0.8631977f)),
                Vector3.Transform(new Vector3(0.6189461f, 1.000128f, 2.261881f), Matrix.CreateScale(Scale)));

            skin.AddPrimitive(box, materialProperties);
            #endregion

            #region Primitive 15
            // MaterialProperties:
            materialProperties = new JigLibX.Collision.MaterialProperties();
            materialProperties.StaticRoughness = 0.5f;
            materialProperties.DynamicRoughness = 0.35f;
            materialProperties.Elasticity = 0.25f;

            // Primitive:
            box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(1.130137f, 0.3319661f, -0.2883475f), Matrix.CreateScale(Scale)),
                Matrix.CreateFromQuaternion(new Quaternion(-5.743456E-06f, -0.7070181f, 7.036445E-05f, 0.7071953f)),
                Vector3.Transform(new Vector3(0.6189461f, 1.000128f, 2.261881f), Matrix.CreateScale(Scale)));

            skin.AddPrimitive(box, materialProperties);
            #endregion

            #region Primitive 16
            // MaterialProperties:
            materialProperties = new JigLibX.Collision.MaterialProperties();
            materialProperties.StaticRoughness = 0.5f;
            materialProperties.DynamicRoughness = 0.35f;
            materialProperties.Elasticity = 0.25f;

            // Primitive:
            box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(-0.835217f, 0.3319549f, -0.8023916f), Matrix.CreateScale(Scale)),
                Matrix.CreateFromQuaternion(new Quaternion(1.82997E-05f, 0.2593707f, -1.838076E-05f, 0.9657778f)),
                Vector3.Transform(new Vector3(0.6189461f, 1.000128f, 2.261881f), Matrix.CreateScale(Scale)));

            skin.AddPrimitive(box, materialProperties);
            #endregion

            #region Primitive 17
            // MaterialProperties:
            materialProperties = new JigLibX.Collision.MaterialProperties();
            materialProperties.StaticRoughness = 0.5f;
            materialProperties.DynamicRoughness = 0.35f;
            materialProperties.Elasticity = 0.25f;

            // Primitive:
            box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(-1.13162f, 0.3319914f, -0.2886084f), Matrix.CreateScale(Scale)),
                Matrix.CreateFromQuaternion(new Quaternion(4.874352E-05f, 0.4952257f, -8.749445E-06f, 0.8687643f)),
                Vector3.Transform(new Vector3(0.6189461f, 1.000128f, 2.261881f), Matrix.CreateScale(Scale)));

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
