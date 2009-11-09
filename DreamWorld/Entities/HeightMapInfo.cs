using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace DreamWorld.Entities
{
    public class HeightMapInfo
    {
        private float terrainScale;
        private float[,] heights;
        private Vector3 heightmapPosition;
        private float heightmapWidth;
        private float heightmapHeight;

        public HeightMapInfo(float[,] heights, float terrainScale)
        {
            if (heights == null)
            {
                throw new ArgumentNullException("heights");
            }

            this.terrainScale = terrainScale;
            this.heights = heights;

            heightmapWidth = (heights.GetLength(0) - 1) * terrainScale;
            heightmapHeight = (heights.GetLength(1) - 1) * terrainScale;

            heightmapPosition.X = -(heights.GetLength(0) - 1) / 2 * terrainScale;
            heightmapPosition.Z = -(heights.GetLength(1) - 1) / 2 * terrainScale;
        }

        public bool IsOnHeightmap(Vector3 position)
        {
            Vector3 positionOnHeightmap = position - heightmapPosition;

            return (positionOnHeightmap.X > 0 &&
                positionOnHeightmap.X < heightmapWidth &&
                positionOnHeightmap.Z > 0 &&
                positionOnHeightmap.Z < heightmapHeight);
        }

        public float GetHeight(Vector3 position)
        {
            Vector3 positionOnHeightmap = position - heightmapPosition;

            int left, top;
            left = (int)positionOnHeightmap.X / (int)terrainScale;
            top = (int)positionOnHeightmap.Z / (int)terrainScale;

            float xNormalized = (positionOnHeightmap.X % terrainScale) / terrainScale;
            float zNormalized = (positionOnHeightmap.Z % terrainScale) / terrainScale;

            float topHeight = MathHelper.Lerp(
                heights[left, top],
                heights[left + 1, top],
                xNormalized);

            float bottomHeight = MathHelper.Lerp(
                heights[left, top + 1],
                heights[left + 1, top + 1],
                xNormalized);

            return MathHelper.Lerp(topHeight, bottomHeight, zNormalized);
        }
    }

    public class HeightMapInfoReader : ContentTypeReader<HeightMapInfo>
    {
        protected override HeightMapInfo Read(ContentReader input,
            HeightMapInfo existingInstance)
        {
            float terrainScale = input.ReadSingle();
            int width = input.ReadInt32();
            int height = input.ReadInt32();
            float[,] heights = new float[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    heights[x, z] = input.ReadSingle();
                }
            }
            return new HeightMapInfo(heights, terrainScale);
        }
    }
}
