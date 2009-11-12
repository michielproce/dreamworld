using DreamWorld.InputManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Entities
{
    public class Player : Entity
    {
        public readonly Vector3 CameraOffset = new Vector3(0,0,0);

        public InputManager InputManager { private get; set; }

        protected override void LoadContent()
        {
            Model = Screen.Content.Load<Model>(@"Models\Test\Ball");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            // Handle Input            
            Rotation += new Vector3(Rotation.X, InputManager.Player.HorizontalRotation, Rotation.Z);            
            Position += Vector3.Transform(InputManager.Player.Movement, Matrix.CreateRotationY(Rotation.Y));
            

            // Put it on the floor
            if(Level.Terrain != null)
            {
                Position = new Vector3(Position.X, Level.Terrain.HeightMapInfo.GetHeight(Position), Position.Z);               
            }

            base.Update(gameTime);
        }

        private void move(Vector3 direction)
        {
            
        }

        private void rotate(Vector3 rotation)
        {
            
        }
    }
}
