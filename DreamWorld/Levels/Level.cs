using System.Collections.Generic;
using DreamWorld.Entities;

namespace DreamWorld.Levels
{
    public abstract class Level
    {
        public List<Entity> Entities { get; protected set; }

        protected Level()
        {
            Entities = new List<Entity>();
        }        
    }
}
