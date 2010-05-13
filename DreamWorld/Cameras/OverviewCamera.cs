using DreamWorld.Entities;
using Microsoft.Xna.Framework;

namespace DreamWorld.Cameras
{
    class OverviewCamera : Camera
    {
        public ThirdPersonCamera OldCamera;
        public Player Player;

        public Vector3 TargetPosition;
        public Vector3 TargetLookat;

        private Vector3 _startPosition;
        private Vector3 _startLookat;

        public bool IsExitting;
        private const float TransitionSpeed = 0.015f;
        public float Transition;

        public override Vector3 Direction
        {
            get
            {
                return Vector3.Normalize(Vector3.SmoothStep(_startLookat, TargetLookat, Transition) - Position);
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
            OldCamera.Update(gameTime);
            _startPosition = OldCamera.Position;
            _startLookat = Player.Body.Position + Player.CameraOffset;

            Position = Vector3.SmoothStep(_startPosition, TargetPosition, Transition);

            View = Matrix.CreateLookAt(
                    Position,
                    Vector3.SmoothStep(_startLookat, TargetLookat, Transition),
                    Vector3.Up);

            base.Update(gameTime);
        }

        private void UpdateTransition()
        {
            if (IsExitting)
            {
                Transition -= TransitionSpeed;
                if (Transition < 0)
                    Transition = 0;
            }
            else
            {
                Transition += TransitionSpeed;
                if (Transition > 1)
                    Transition = 1;
            }
        }
    }
}
