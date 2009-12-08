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
        
        public override Vector3 Direction { 
            get
            {
                return Vector3.Normalize(level.Player.Position - Position);
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
            Vector3 playerPosition = level.Player.Position + level.Player.CameraOffset;

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
            Position = playerPosition - 
                Vector3.Transform(
                    Vector3.Transform(
                        new Vector3(0, 0, -distance),
                        Matrix.CreateRotationX(verticalRotation)),
                    Matrix.CreateRotationY(level.Player.Rotation.Y + horizontalRotation));    

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
    }
}
