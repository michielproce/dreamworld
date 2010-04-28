﻿using System;
using DreamWorld.Entities;
using DreamWorld.Interface.Help;
using DreamWorld.ScreenManagement.Screens.Cutscenes;
using DreamWorld.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Levels.VillageLevel.Entities
{
    public class Morwir : Entity
    {
        public static bool ListInEditor = true;

        public override void Initialize()
        {
            Animation.InitialClip = "Standing";
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Model = GameScreen.Content.Load<Model>(@"Models\Village\Morwir");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (Vector3.Distance(Level.Player.Body.Position, Body.Position) <= Help.HELP_DISTANCE)
            {
                GameScreen.HelpSystem.ShowCustomHint(
                    StringUtil.ParsePlatform("{Click the left mouse button|Press B} to talk."), this);
                
                if(GameScreen.InputManager.Player.ApplyRotation)
                {
                    GameScreen.ExitScreen();
                    GameScreen.ScreenManager.AddScreen(new MorwirCutscene());
                }                    
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
            box = new JigLibX.Geometry.Box(Vector3.Transform(new Vector3(-24.88526f, -0.07025528f, -24.1344f), Matrix.CreateScale(Scale)),
                Matrix.CreateFromQuaternion(new Quaternion(0f, 0f, 0f, 0.9999999f)),
                Vector3.Transform(new Vector3(52.04298f, 121.3141f, 36.86612f), Matrix.CreateScale(Scale)));

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