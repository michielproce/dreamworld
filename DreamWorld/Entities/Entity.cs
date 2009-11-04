using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Entities
{
    public abstract class Entity
    {       
        public Vector3 Position { get; protected set; }
        public Quaternion Rotation { get; protected set; }
        public Vector3 Scale { get; protected set; }
        public Model Model { get; protected set; }
        public Effect Effect { get; protected set; }
        public BoundingSphere[] BoundingSpheres { get; protected set; }
        public Matrix World { get; private set; }

        protected Entity()
        {
        }
    }
}
