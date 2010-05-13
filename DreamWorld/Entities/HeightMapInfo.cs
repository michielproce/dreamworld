using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace DreamWorld.Entities
{
    public class HeightMapInfo
    {
        private readonly float _terrainScale;
        private readonly float[,] _heights;
        private readonly Vector3 _heightmapPosition;
        private readonly float _heightmapWidth;
        private readonly float _heightmapHeight;

        public HeightMapInfo(float[,] heights, float terrainScale)
        {
            if (heights == null)
            {
                throw new ArgumentNullException("heights");
            }

            _terrainScale = terrainScale;
            _heights = heights;

            _heightmapWidth = (heights.GetLength(0) - 1) * terrainScale;
            _heightmapHeight = (heights.GetLength(1) - 1) * terrainScale;

            _heightmapPosition.X = -(heights.GetLength(0) - 1) * terrainScale / 2;
            _heightmapPosition.Z = -(heights.GetLength(1) - 1) * terrainScale / 2;
        }

        public bool IsOnHeightmap(Vector3 position)
        {
            Vector3 positionOnHeightmap = position - _heightmapPosition;

            return (positionOnHeightmap.X > 0 &&
                positionOnHeightmap.X < _heightmapWidth &&
                positionOnHeightmap.Z > 0 &&
                positionOnHeightmap.Z < _heightmapHeight);
        }

        public float GetHeight(Vector3 position)
        {
            Vector3 positionOnHeightmap = position - _heightmapPosition;

            int left = (int)positionOnHeightmap.X / (int)_terrainScale;
            int top = (int)positionOnHeightmap.Z / (int)_terrainScale;

            float xNormalized = (positionOnHeightmap.X % _terrainScale) / _terrainScale;
            float zNormalized = (positionOnHeightmap.Z % _terrainScale) / _terrainScale;

            float topHeight = MathHelper.Lerp(
                _heights[left, top],
                _heights[left + 1, top],
                xNormalized);

            float bottomHeight = MathHelper.Lerp(
                _heights[left, top + 1],
                _heights[left + 1, top + 1],
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
