using DreamWorld.InputManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Entities
{
    public class Player : Entity
    {
        private const float jumpStart = 2f;
        private const float jumpGravity = .05f;
        private float jumpVelocity;

        public readonly Vector3 CameraOffset = new Vector3(0,2,0);

        public InputManager InputManager { private get; set; }

        protected override void LoadContent()
        {
            Model = GameScreen.Content.Load<Model>(@"Models\Test\Ball");
            PutOnTerrain();
            
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Rotation += new Vector3(Rotation.X, InputManager.Player.HorizontalRotation, Rotation.Z);            
            Position += Vector3.Transform(InputManager.Player.Movement, Matrix.CreateRotationY(Rotation.Y));

            if (InputManager.Player.Jump && !IsJumping())            
                StartJumping();

            if (IsJumping())
            {
                Position += new Vector3(0, jumpVelocity, 0);
                jumpVelocity -= jumpGravity;
                if (IsOnTerrain())
                {
                    StopJumping();
                    PutOnTerrain();
                }
            }
            else
            {
                PutOnTerrain();
            }

            base.Update(gameTime);
        }

        private void StartJumping()
        {
            jumpVelocity = jumpStart;
        }

        private bool IsJumping()
        {
            return jumpVelocity != 0;
        }

        private void StopJumping()
        {
            jumpVelocity = 0;
        }

        private bool IsOnTerrain()
        {
            return Level.Terrain != null &&
                   Position.Y - Level.Terrain.HeightMapInfo.GetHeight(Position) <= 0;
        }
        
        private void PutOnTerrain()
        {
            if (Level.Terrain != null)
                Position = new Vector3(Position.X, Level.Terrain.HeightMapInfo.GetHeight(Position), Position.Z);
        }
    }
}
