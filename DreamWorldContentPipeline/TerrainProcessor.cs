using System.ComponentModel;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace DreamWorldContentPipeline
{
    [ContentProcessor(DisplayName = "Terrain Processor")]
    public class TerrainProcessor : ContentProcessor<Texture2DContent, ModelContent>
    {
        private float terrainScale = 30f;
        [DisplayName("Terrain Scale")]
        [DefaultValue(30f)]
        [Description("Scale of the the terrain geometry width and length.")]
        public float TerrainScale
        {
            get { return terrainScale; }
            set { terrainScale = value; }
        }

        private float terrainBumpiness = 640f;
        [DisplayName("Terrain Bumpiness")]
        [DefaultValue(640f)]
        [Description("Scale of the the terrain geometry height.")]
        public float TerrainBumpiness
        {
            get { return terrainBumpiness; }
            set { terrainBumpiness = value; }
        }

        private float texCoordScale = 0.1f;
        [DisplayName("Texture Coordinate Scale")]
        [DefaultValue(0.1f)]
        [Description("Terrain texture tiling density.")]
        public float TexCoordScale
        {
            get { return texCoordScale; }
            set { texCoordScale = value; }
        }

        private string terrainTextureFilename1 = "primary.bmp";
        [DisplayName("Terrain Texture #1")]
        [DefaultValue("primary.bmp")]
        [Description("The name of the primary terrain texture.")]
        public string TerrainTextureFilename1
        {
            get { return terrainTextureFilename1; }
            set { terrainTextureFilename1 = value; }
        }

        private float weightTexture1 = 0.1f;
        [DisplayName("Weight Texture #1")]
        [DefaultValue(0.5f)]
        [Description("The weight of the primary terrain texture.")]
        public float WeightTexture1
        {
            get { return weightTexture1; }
            set { weightTexture1 = value; }
        }

        private string terrainTextureFilename2 = "secondary.bmp";
        [DisplayName("Terrain Texture #2")]
        [DefaultValue("secondary.bmp")]
        [Description("The name of the secondary terrain texture.")]
        public string TerrainTextureFilename2
        {
            get { return terrainTextureFilename2; }
            set { terrainTextureFilename2 = value; }
        }

        private float weightTexture2 = 0.1f;
        [DisplayName("Weight Texture #2")]
        [DefaultValue(0.5f)]
        [Description("The weight of the secondary terrain texture.")]
        public float WeightTexture2
        {
            get { return weightTexture2; }
            set { weightTexture2 = value; }
        }

        public override ModelContent Process(Texture2DContent input,
                                             ContentProcessorContext context)
        {
            MeshBuilder builder = MeshBuilder.StartMesh("terrain");

            // Convert the input texture to float format, for ease of processing.
            input.ConvertBitmapType(typeof(PixelBitmapContent<float>));

            PixelBitmapContent<float> heightfield;
            heightfield = (PixelBitmapContent<float>)input.Mipmaps[0];

            // Create the terrain vertices.
            for (int y = 0; y < heightfield.Height; y++)
            {
                for (int x = 0; x < heightfield.Width; x++)
                {
                    Vector3 position;

                    // position the vertices so that the heightfield is centered around x=0,z=0
                    position.X = terrainScale * (x - ((heightfield.Width - 1) / 2.0f));
                    position.Z = terrainScale * (y - ((heightfield.Height - 1) / 2.0f));

                    position.Y = (heightfield.GetPixel(x, y) - 1) * terrainBumpiness;

                    builder.CreatePosition(position);
                }
            }

            EffectMaterialContent material = new EffectMaterialContent();

            string effectPath = Path.GetFullPath(@"Effects\Terrain.fx");
            material.Effect = new ExternalReference<EffectContent>(effectPath);

            string directory = Path.GetDirectoryName(input.Identity.SourceFilename);

            float weightTotal = weightTexture1 + weightTexture2;

            string texture1 = Path.Combine(directory, terrainTextureFilename1);            
            material.Textures.Add("Texture1", new ExternalReference<TextureContent>(texture1));
            material.OpaqueData.Add("WeightTexture1", weightTexture1 / weightTotal);

            string texture2 = Path.Combine(directory, terrainTextureFilename2);
            material.Textures.Add("Texture2", new ExternalReference<TextureContent>(texture2));
            material.OpaqueData.Add("WeightTexture2", weightTexture2 / weightTotal);

            builder.SetMaterial(material);

            // Create a vertex channel for holding texture coordinates.
            int texCoordId = builder.CreateVertexChannel<Vector2>(
                                            VertexChannelNames.TextureCoordinate(0));

            // Create the individual triangles that make up our terrain.
            for (int y = 0; y < heightfield.Height - 1; y++)
            {
                for (int x = 0; x < heightfield.Width - 1; x++)
                {
                    AddVertex(builder, texCoordId, heightfield.Width, x, y);
                    AddVertex(builder, texCoordId, heightfield.Width, x + 1, y);
                    AddVertex(builder, texCoordId, heightfield.Width, x + 1, y + 1);

                    AddVertex(builder, texCoordId, heightfield.Width, x, y);
                    AddVertex(builder, texCoordId, heightfield.Width, x + 1, y + 1);
                    AddVertex(builder, texCoordId, heightfield.Width, x, y + 1);
                }
            }

            // Chain to the ModelProcessor so it can convert the mesh we just generated.
            MeshContent terrainMesh = builder.FinishMesh();

            ModelContent model = context.Convert<MeshContent, ModelContent>(terrainMesh,
                                                              "ModelProcessor");

            // generate information about the height map, and attach it to the finished
            // model's tag.
            model.Tag = new HeightMapInfoContent(heightfield, terrainScale,
                terrainBumpiness);

            return model;
        }

        void AddVertex(MeshBuilder builder, int texCoordId, int w, int x, int y)
        {
            builder.SetVertexChannelData(texCoordId, new Vector2(x, y) * texCoordScale);

            builder.AddTriangleVertex(x + y * w);
        }
    }
}
