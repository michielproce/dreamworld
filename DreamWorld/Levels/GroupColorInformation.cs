using System;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Levels
{
    [Serializable]
    public class GroupColorInformation 
    {
        public int ID { get; set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public GroupColorInformation()
        {
            R = 255;
            G = 255;
            B = 255;
        }

        public Color GetColor()
        {
            return new Color(R, G, B);
        }
    }
}
