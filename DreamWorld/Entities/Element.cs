using Microsoft.Xna.Framework;

namespace DreamWorld.Entities
{
    class Element : Entity
    {
        public Group Group { get; internal set; }

        protected override Matrix GenerateWorldMatrix()
        {
            return base.GenerateWorldMatrix() * Group.World;
        }
    }
}