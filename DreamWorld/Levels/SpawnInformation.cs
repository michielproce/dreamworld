using System;
using Microsoft.Xna.Framework;

namespace DreamWorld.Levels
{
    [Serializable]
    public class SpawnInformation
    {
        public string Name { get; set; }
        public string TypeName { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; } // Degrees!
        public Vector3 Scale { get; set; }
        public int Group { get; set; }

        public SpawnInformation()
        {
            Scale = Vector3.One;            
        }
    }
}
