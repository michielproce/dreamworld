using System;
using System.Collections.Generic;
using DreamWorld.ScreenManagement.Screens;
using DreamWorld.Util;
using JigLibX.Collision;
using JigLibX.Geometry;
using JigLibX.Physics;
using Microsoft.Xna.Framework;

namespace DreamWorld.Cameras
{
    class ThirdPersonCamera : Camera
    {
        private const float minCameraHeight = 2f;
        private bool camOnFloor;

        private const float minDistance = 5f;
        private const float maxDistance = 100f;
        private float distance = 30f;

        private const float minVerticalRotation = -MathHelper.PiOver2 * .99f;
        private const float maxVerticalRotation = -minVerticalRotation;
        private float verticalRotation = MathHelper.ToRadians(5);

        private float horizontalRotation;
        
        public override Vector3 Direction { 
            get
            {
                return Vector3.Normalize(level.Player.Body.Position + level.Player.CameraOffset - Position);
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
            Vector3 playerPosition = level.Player.Body.Position + level.Player.CameraOffset;
            Vector3 playerDirection = Vector3.Transform(Vector3.Forward, level.Player.Body.Orientation);
            float playerRotation = MathHelper.Pi + (float)Math.Atan2(playerDirection.X, playerDirection.Z);

            distance -= inputManager.Player.Zoom;
            if (distance < minDistance)
                distance = minDistance;
            if (distance > maxDistance)
                distance = maxDistance;

            float verticalRotationInput = inputManager.Player.VerticalRotation;
            if (camOnFloor && verticalRotationInput < 0)
                verticalRotationInput = 0;
            verticalRotation -= verticalRotationInput;
            if (verticalRotation < minVerticalRotation)
                verticalRotation = minVerticalRotation;
            if (verticalRotation > maxVerticalRotation)
                verticalRotation = maxVerticalRotation;

            if (inputManager.Player.ResetHorizontalRotation)
                horizontalRotation = 0;
            horizontalRotation += inputManager.Player.HorizontalRotation;

            Position = playerPosition + 
                Vector3.Transform(
                    Vector3.Transform(
                        new Vector3(0, 0, distance),
                        Matrix.CreateRotationX(verticalRotation)),
                    Matrix.CreateRotationY(playerRotation + horizontalRotation));

            if(level.Terrain != null)
            {
                float cameraHeight = level.Terrain.HeightMapInfo.GetHeight(Position) + minCameraHeight;
                if (Position.Y < cameraHeight)
                {
                    camOnFloor = true;
                    Position = new Vector3(Position.X, cameraHeight, Position.Z);
                }
                else
                {
                    camOnFloor = false;
                }
            }

            View = Matrix.CreateLookAt(
                    Position,
                    playerPosition,
                    Vector3.Up);

            base.Update(gameTime);
        }

        /// <summary>
        /// Get null or the collisionskin that is colliding with the camera.
        /// Since we are using a physics-system, there can only be one skin at a position.
        /// </summary>
        public CollisionSkin GetCollidingSkin()
        {
            float dist;
            CollisionSkin skin;
            Vector3 pos, normal;

            CollisionSkinPredicate1 pred = new DefaultSkinPredicate();
            Segment seg = new Segment(Position, Vector3.Zero);

            DreamWorldPhysicsSystem.CurrentPhysicsSystem.CollisionSystem.SegmentIntersect(out dist, out skin, out pos, out normal, seg, pred);

            return skin;
        }
    }
}
