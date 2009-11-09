using System;
using Microsoft.Xna.Framework;

namespace DreamWorld.Cameras
{
    class DebugCamera : Camera
    {
        public const float MaxPitch = MathHelper.PiOver2 * .99f; // Matrix.createLookAt gets confused with maxPitch > 90 degrees

        private Vector3 position;
        private float yaw;
        private float pitch;

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
            // Handle Input                       
            Move(InputManager.Debug.Movement);
            Rotate(InputManager.Debug.Rotation.X, InputManager.Debug.Rotation.Y);

            View = Matrix.CreateLookAt(
                    position,
                    position + RotatedDirection(Vector3.Forward),
                    Vector3.Up);

            base.Update(gameTime);
        }

        private void Move(Vector3 direction)
        {
             position += RotatedDirection(direction);
        }

        private void Rotate(float yaw, float pitch)
        {
            this.yaw += yaw;
            this.pitch += pitch;           

            if (this.pitch > MaxPitch)
                this.pitch = MaxPitch;
            if (this.pitch < -MaxPitch)
                this.pitch = -MaxPitch;
        }

        private Vector3 RotatedDirection(Vector3 direction)
        {            
            return Vector3.Transform(direction, Quaternion.CreateFromYawPitchRoll(yaw, pitch, 0));
        }
    }
}
