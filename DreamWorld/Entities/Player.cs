using System;
using DreamWorld.Cameras;
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

        public readonly Vector3 CameraOffset = new Vector3(0,12,0);

        public InputManager InputManager { private get; set; }

        protected override void LoadContent()
        {
            Model = GameScreen.Content.Load<Model>(@"Models\Test\dude");
            Animation.InitialClip = "Take 001";
            Animation.Paused = true;
            Animation.Speed = 1.8f;
            
            Scale = new Vector3(.2f);           
            PutOnTerrain();
            
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {            
#if(DEBUG)
            if(GameScreen.CurrentCamera is DebugCamera)
                return;
#endif   
            
            Rotation += new Vector3(Rotation.X, InputManager.Player.Rotation, Rotation.Z);  
          
            Vector3 movement = Vector3.Transform(InputManager.Player.Movement, Matrix.CreateRotationY(Rotation.Y));
            Position += movement;
            if(movement.Length() != 0)
                Animation.Paused = false;
            else
                Animation.Paused = true;

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
