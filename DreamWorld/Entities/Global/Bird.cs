using System;
using Microsoft.Xna.Framework;

namespace DreamWorld.Entities.Global
{
    public abstract class Bird : Entity
    {   
        private enum State
        {
            Flying, Gliding
        }

        private State _state;
        private TimeSpan _lastStateSwitch;
        private static readonly Random random = new Random();
        private float? _initialHeight;
        private float _prevRotation;
        

        protected abstract float Speed { get; }
        protected abstract float MaxRotation { get; }

        public override void Initialize()
        {
            _state = State.Flying;

            if (Level.Terrain != null && Level.Terrain.HeightMapInfo.IsOnHeightmap(SpawnInformation.Position))            
                _initialHeight = SpawnInformation.Position.Y - Level.Terrain.HeightMapInfo.GetHeight(SpawnInformation.Position);            
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            Vector3 target = SpawnInformation.Position;
            Vector3 position = Body.Position;            
            TimeSpan timeInState = gameTime.TotalGameTime - _lastStateSwitch;


            // Make the bird face the target.
            float rot = (float)Math.Atan2(target.Z - position.Z, target.X - position.X) * -1 + MathHelper.PiOver2;
            if (rot < 0)
                rot += MathHelper.TwoPi;

            // Make sure the bird doesn't get a seizure.            
            rot = MathHelper.Clamp(rot, _prevRotation - MaxRotation, _prevRotation + MaxRotation);
            _prevRotation = rot;

            // Apply the rotation
            Body.Orientation = Matrix.CreateRotationY(rot);

            // Move forward along it's rotation
            Vector3 forward = Vector3.Transform(new Vector3(0, 0, Speed), Body.Orientation);
            Body.Position += forward;

            // Keep it on the same height on the heightmap
            if (_initialHeight.HasValue && Level.Terrain.HeightMapInfo.IsOnHeightmap(position))
                Body.Position = new Vector3(Body.Position.X, Level.Terrain.HeightMapInfo.GetHeight(position) + (float)_initialHeight, Body.Position.Z);

            switch (_state)
            {
                case State.Flying:
                    // Animation is playing.
                    Animation.Paused = false;                    
                    
                    // We switch the state when:
                    // - We've been flying for at least 5 seconds
                    // - The wings of the bird are in a nice gliding position
                    // - The dice likes us                    
                    if (timeInState.Seconds >= 5 && Math.Abs(Animation.CurrentTimeSpan.Milliseconds - 750f) < 30 && random.NextDouble() < .075)
                    {
                        _state = State.Gliding;
                        _lastStateSwitch = gameTime.TotalGameTime;
                    }
                    break;
                case State.Gliding:
                    Animation.Paused = true;

                    // Switch after 3 seconds of gliding
                    if (timeInState.Seconds >= 3)
                    {                        
                        _state = State.Flying;
                        _lastStateSwitch = gameTime.TotalGameTime;
                    }
                    break;                    
            }

   
            base.Update(gameTime);
        }

    }
}
