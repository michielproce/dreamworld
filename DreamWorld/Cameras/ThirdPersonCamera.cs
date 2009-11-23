using System;
using Microsoft.Xna.Framework;

namespace DreamWorld.Cameras
{
    class ThirdPersonCamera : Camera
    {
        private const float minCameraHeight = 2f;
        private bool camOnFloor;

        private const float minDistance = 3f;
        private const float maxDistance = 100f;
        private float distance = 10f;

        private const float minVerticalRotation = -MathHelper.PiOver2 * .99f;
        private const float maxVerticalRotation = -minVerticalRotation;
        private float verticalRotation = -MathHelper.ToRadians(0);

        private float horizontalRotation;
        
        public override Vector3 CameraDirection { 
            get
            {
                return Vector3.Normalize(Level.Player.Position - Position);
            }
        }

        public override void Initialize()
        {
            Projection = Matrix.CreatePerspectiveFieldOfView(
               MathHelper.ToRadians(45.0f),
               GraphicsDevice.Viewport.AspectRatio,
               1.0f,
               10000.0f);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            Vector3 playerPosition = Level.Player.Position + Level.Player.CameraOffset;

            distance -= InputManager.Player.Zoom;
            if (distance < minDistance)
                distance = minDistance;
            if (distance > maxDistance)
                distance = maxDistance;

            float verticalRotationInput = InputManager.Player.VerticalRotation;
            if (camOnFloor && verticalRotationInput < 0)
                verticalRotationInput = 0;
            verticalRotation -= verticalRotationInput;
            if (verticalRotation < minVerticalRotation)
                verticalRotation = minVerticalRotation;
            if (verticalRotation > maxVerticalRotation)
                verticalRotation = maxVerticalRotation;

            if (InputManager.Player.ResetHorizontalRotation)
                horizontalRotation = 0;
            horizontalRotation += InputManager.Player.HorizontalRotation;
            Position = playerPosition - 
                Vector3.Transform(
                    Vector3.Transform(
                        new Vector3(0, 0, -distance),
                        Matrix.CreateRotationX(verticalRotation)),
                    Matrix.CreateRotationY(Level.Player.Rotation.Y + horizontalRotation));    

            if(Level.Terrain != null)
            {
                float cameraHeight = Level.Terrain.HeightMapInfo.GetHeight(Position) + minCameraHeight;
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
    }
}
