using System;
using Microsoft.Xna.Framework;

namespace DreamWorld.Cameras
{
    class ThirdPersonCamera : Camera
    {
        private const float camFloorDistance = 1f;

        private const float minDistance = 3f;
        private const float maxDistance = 30f;
        private float distance = 10f;

        private const float minVerticalRotation = -MathHelper.PiOver2 * .99f;
        private const float maxVerticalRotation = -minVerticalRotation;
        private float verticalRotation = -MathHelper.PiOver4;
        

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

            verticalRotation -= InputManager.Player.VerticalRotation;
            if (verticalRotation < minVerticalRotation)
                verticalRotation = minVerticalRotation;
            if (verticalRotation > maxVerticalRotation)
                verticalRotation = maxVerticalRotation;
            
            Vector3 cameraPosition = playerPosition - 
                Vector3.Transform(
                    Vector3.Transform(
                        new Vector3(0, 0, -distance),
                        Matrix.CreateRotationX(verticalRotation)),
                    Matrix.CreateRotationY(Level.Player.Rotation.Y));    

            if(Level.Terrain != null)
            {
                float cameraHeight = Level.Terrain.HeightMapInfo.GetHeight(cameraPosition) + camFloorDistance;
                if (cameraPosition.Y < cameraHeight)                
                    cameraPosition.Y = cameraHeight;
                
            }

            View = Matrix.CreateLookAt(
                    cameraPosition,
                    playerPosition,
                    Vector3.Up);

            base.Update(gameTime);
        }
    }
}
