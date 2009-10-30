using System;
using Microsoft.Xna.Framework;

namespace DreamWorld.Cameras
{
    class DebugCamera : Camera
    {
        public const float MoveSpeed = .5f;
        public const float RotateSpeed = MathHelper.Pi / 2000f;
        public const float MaxPitch = MathHelper.PiOver2 * .99f; // Matrix.createLookAt gets confused with maxPitch > 90 degrees

        private Vector3 position;
        private float yaw;
        private float pitch;

        public DebugCamera(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            Projection = Matrix.CreatePerspectiveFieldOfView(
               MathHelper.ToRadians(45.0f),
               Game.GraphicsDevice.Viewport.AspectRatio,
               1.0f,
               10000.0f);          
        }

        public override void Update(GameTime gameTime)
        {
            View = Matrix.CreateLookAt(
                    position,
                    position + RotatedDirection(Vector3.Forward),
                    Vector3.Up);
        }

        public void Move(Vector3 direction)
        {
             position += RotatedDirection(direction * MoveSpeed);
        }

        public void Rotate(float yaw, float pitch)
        {
            this.yaw -= yaw * RotateSpeed;
            this.pitch -= pitch * RotateSpeed;

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
