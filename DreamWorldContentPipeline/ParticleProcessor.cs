using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace DreamWorldContentPipeline
{
    [ContentProcessor(DisplayName = "Particle Processor")]
    public class ParticleProcessor : TextureProcessor
    {
        public override TextureContent Process(TextureContent input, ContentProcessorContext context)
        {
            GenerateMipmaps = true;
            TextureFormat = TextureProcessorOutputFormat.DxtCompressed;
            return base.Process(input, context);
        }
    }
}