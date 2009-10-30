using Microsoft.Xna.Framework;

namespace DreamWorld.Cameras
{
    public abstract class Camera : GameComponent
    {
        public Matrix View { get; protected set; }
        public Matrix Projection { get; protected set; }
        public BoundingFrustum Frustrum { get; protected set; }

        protected Camera(Game game) : base(game)
        {
        }
    }
}
