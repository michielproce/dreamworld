using Microsoft.Xna.Framework.Content;

namespace DreamWorldBase
{
    public class SkinnedSphere
    {
        public string BoneName;
        public float Radius;

        [ContentSerializer(Optional = true)]
        public float Offset;
    }
}
