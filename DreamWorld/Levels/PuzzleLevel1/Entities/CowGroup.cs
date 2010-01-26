using System;
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

        public override bool IsRotationAllowed(Vector3 direction)
        {
            // Don't rotate if a cow is falling down
            if (Center is Cow && ((Cow)Center).velocity != 0)
                return false;

            foreach (KeyValuePair<string, Entity> pair in Entities)
            {
                if (Center is Cow && ((Cow)pair.Value).velocity != 0)
                    return false;
            }
            return base.IsRotationAllowed(direction);
        }
    }
}
