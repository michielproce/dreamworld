using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace DreamWorldContentPipeline
{
    [ContentProcessor(DisplayName = "Default Model Processor")]
    public class DefaultModelProcessor :ModelProcessor
    {
        public override ModelContent Process(NodeContent input,
                                     ContentProcessorContext context)
        {
            ModelContent model = base.Process(input, context);

            return model;
        }
    }
}