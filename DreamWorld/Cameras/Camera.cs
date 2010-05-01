using DreamWorld.InputManagement;
using DreamWorld.Levels;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Cameras
{
    public abstract class Camera
    {
        protected Level level;
        protected GraphicsDevice device;
        protected InputManager inputManager;
        
        public AudioListener Listener { get; private set; }


        protected Camera()
        {
            GameScreen gameScreen = GameScreen.Instance;
            level = gameScreen.Level;
            device = gameScreen.GraphicsDevice;
            inputManager = gameScreen.InputManager;
            Listener = new AudioListener();
        }


        public Matrix View { get; protected set; }
        public Matrix Projection { get; protected set; }
        public BoundingFrustum Frustrum { get; protected set; }
        public Vector3 Position { get; set; }


        public virtual void Initialize() { }


        public virtual void Update(GameTime gameTime)
        {
            Listener.Position = Position;
            Listener.Forward = Direction;
        }


        public abstract Vector3 Direction { get; }
    }
}
