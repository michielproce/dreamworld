using System;
using Microsoft.Xna.Framework;

namespace DreamWorld.Cameras
{
    class DebugCamera : Camera
    {
        public const float MaxPitch = MathHelper.PiOver2 * .99f; // Matrix.createLookAt gets confused with maxPitch > 90 degrees

        private Vector3 lookAt;

        private float yaw;
        private float pitch;

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
            // Handle Input                       
            Move(inputManager.Debug.Movement);
            Rotate(inputManager.Debug.Rotation.X, inputManager.Debug.Rotation.Y);

            lookAt = Position + RotatedDirection(Vector3.Forward);
            View = Matrix.CreateLookAt(
                    Position,
                    lookAt,
                    Vector3.Up);

            base.Update(gameTime);
        }

        private void Move(Vector3 direction)
        {
             Position += RotatedDirection(direction);
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

        public override Vector3 Direction
        {
            get
            {
                return Vector3.Normalize(lookAt - Position);
            }
        }
    }
}
