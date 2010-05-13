using System;
using DreamWorld.Util;
using JigLibX.Collision;
using JigLibX.Geometry;
using JigLibX.Physics;
using Microsoft.Xna.Framework;

namespace DreamWorld.Cameras
{
    class ThirdPersonCamera : Camera
    {
        private const float MinCameraHeight = 2f;
        private bool _camOnFloor;

        private const float MinDistance = 5f;
        private const float MaxDistance = 100f;
        private float _distance = 35f;

        private const float MinVerticalRotation = -MathHelper.PiOver2 * .99f;
        private const float MaxVerticalRotation = -MinVerticalRotation;        

        private float _horizontalRotation;

        public float VerticalRotation { get; set; }

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

            _distance -= inputManager.Player.Zoom;
            if (_distance < MinDistance)
                _distance = MinDistance;
            if (_distance > MaxDistance)
                _distance = MaxDistance;

            float verticalRotationInput = inputManager.Player.VerticalRotation;
            if (_camOnFloor && verticalRotationInput < 0)
                verticalRotationInput = 0;
            VerticalRotation -= verticalRotationInput;
            if (VerticalRotation < MinVerticalRotation)
                VerticalRotation = MinVerticalRotation;
            if (VerticalRotation > MaxVerticalRotation)
                VerticalRotation = MaxVerticalRotation;

            if (inputManager.Player.ResetHorizontalRotation)
                _horizontalRotation = 0;
            _horizontalRotation += inputManager.Player.HorizontalRotation;

            Position = playerPosition + 
                Vector3.Transform(
                    Vector3.Transform(
                        new Vector3(0, 0, _distance),
                        Matrix.CreateRotationX(VerticalRotation)),
                    Matrix.CreateRotationY(playerRotation + _horizontalRotation));

            if(level.Terrain != null)
            {
                float cameraHeight = level.Terrain.HeightMapInfo.GetHeight(Position) + MinCameraHeight;
                if (Position.Y < cameraHeight)
                {
                    _camOnFloor = true;
                    Position = new Vector3(Position.X, cameraHeight, Position.Z);
                }
                else
                {
                    _camOnFloor = false;
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
