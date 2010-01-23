using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DreamWorld.Entities;

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
    }
}
