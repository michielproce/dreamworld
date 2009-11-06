using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace DreamWorldContentPipeline
{
    [ContentProcessor(DisplayName = "DreamWorld Model Processor")]
    public class DreamWorldModelProcessor :ModelProcessor
    {
        public override ModelContent Process(NodeContent input,
                                     ContentProcessorContext context)
        {
            ModelContent model = base.Process(input, context);

            return model;
        }
    }
}