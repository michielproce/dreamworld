using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace DreamWorldContentPipeline
{

    public class HeightMapInfoContent
    {
        public float[,] Height
        {
            get { return height; }
        }
        float[,] height;

        public float TerrainScale
        {
            get { return terrainScale; }
        }
        private float terrainScale;

        public HeightMapInfoContent(PixelBitmapContent<float> bitmap,
            float terrainScale, float terrainBumpiness)
        {
            this.terrainScale = terrainScale;

            height = new float[bitmap.Width, bitmap.Height];
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    height[x, y] = (bitmap.GetPixel(x, y) - 1) * terrainBumpiness;
                }
            }
        }
    }

    [ContentTypeWriter]
    public class HeightMapInfoWriter : ContentTypeWriter<HeightMapInfoContent>
    {
        protected override void Write(ContentWriter output, HeightMapInfoContent value)
        {
            output.Write(value.TerrainScale);

            output.Write(value.Height.GetLength(0));
            output.Write(value.Height.GetLength(1));
            foreach (float height in value.Height)
            {
                output.Write(height);
            }
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return "DreamWorld.Entities.HeightMapInfo, " +
                "DreamWorld, Version=1.0.0.0, Culture=neutral";
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "DreamWorld.Entities.HeightMapInfoReader, " +
                "DreamWorld, Version=1.0.0.0, Culture=neutral";
        }
    }

}
