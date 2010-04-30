using System;
using System.Collections.Generic;
using DreamWorld.Entities;
using DreamWorld.ScreenManagement.Screens;
using DreamWorld.Util;
using JigLibX.Collision;
using JigLibX.Geometry;
using JigLibX.Physics;
using Microsoft.Xna.Framework;

namespace DreamWorld.Cameras
{
    class OverviewCamera : Camera
    {
        public ThirdPersonCamera oldCamera;
        public Player player;

        public Vector3 targetPosition;
        public Vector3 targetLookat;

        private Vector3 startPosition;
        private Vector3 startLookat;

        public bool isExitting;
        public float transitionSpeed = 0.015f;
        public float transition;

        public override Vector3 Direction
        {
            get
            {
                return Vector3.Normalize(Vector3.SmoothStep(startLookat, targetLookat, transition) - Position);
            }
        }

        public override void Initialize()
        {
            Projection = Matrix.CreatePerspectiveFieldOfView(
               MathHelper.ToRadians(45.0f),
               device.Viewport.AspectRatio,
               1.0f,
               10000.0f);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            UpdateTransition();

            // Update the start vectors in case the player has moved.
            oldCamera.Update(gameTime);
            startPosition = oldCamera.Position;
            startLookat = player.Body.Position + player.CameraOffset;

            Position = Vector3.SmoothStep(startPosition, targetPosition, transition);

            View = Matrix.CreateLookAt(
                    Position,
                    Vector3.SmoothStep(startLookat, targetLookat, transition),
                    Vector3.Up);

            base.Update(gameTime);
        }

        private void UpdateTransition()
        {
            if (isExitting)
            {
                transition -= transitionSpeed;
                if (transition < 0)
                    transition = 0;
            }
            else
            {
                transition += transitionSpeed;
                if (transition > 1)
                    transition = 1;
            }
        }
    }
}
