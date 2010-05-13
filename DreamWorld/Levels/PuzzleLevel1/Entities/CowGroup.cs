using System.Collections.Generic;
using DreamWorld.Entities;
using Microsoft.Xna.Framework;

namespace DreamWorld.Levels.PuzzleLevel1.Entities
{
    class CowGroup : Group
    {
        public override bool IsColliding
        {
            get
            {
                if(IsRotating)
                    return false;

                return base.IsColliding;
            }
        }

        protected override bool IsRotationAllowed(Vector3 direction)
        {
            // Don't rotate if a cow is falling down
            if (Center is Cow && ((Cow)Center).IsFalling)
                return false;

            foreach (KeyValuePair<string, Entity> pair in Entities)
            {
                if (Center is Cow && ((Cow)pair.Value).IsFalling)
                    return false;
            }
            return base.IsRotationAllowed(direction);
        }
    }
}
