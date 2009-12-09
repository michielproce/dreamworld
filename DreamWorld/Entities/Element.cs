using Microsoft.Xna.Framework;

namespace DreamWorld.Entities
{
    public class Element : Entity
    {
        public Group Group { get; internal set; }

        protected override Matrix GenerateWorldMatrix()
        {
            return base.GenerateWorldMatrix() * Group.World;
        }
    }
}