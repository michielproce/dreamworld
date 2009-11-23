using DreamWorld.InputManagement;
using DreamWorld.Levels;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Cameras
{
    public abstract class Camera
    {
        public Level Level { protected get; set; }
        public GraphicsDevice GraphicsDevice { protected get; set; }
        public InputManager InputManager { protected get; set; }
        
        public Matrix View { get; protected set; }
        public Matrix Projection { get; protected set; }
        public BoundingFrustum Frustrum { get; protected set; }
        public Vector3 Position { get; set; }

        public virtual void Initialize() { }
        public virtual void Update(GameTime gameTime) { }

        public virtual Vector3 CameraDirection { get; set; }
    }
}
