using System.IO;
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

        protected override MaterialContent ConvertMaterial(MaterialContent material,
                                                        ContentProcessorContext context)
        {
            BasicMaterialContent basicMaterial = material as BasicMaterialContent;

            if (basicMaterial == null)
            {
                throw new InvalidContentException(string.Format(
                    "SkinnedModelProcessor only supports BasicMaterialContent, " +
                    "but input mesh uses {0}.", material.GetType()));
            }

            EffectMaterialContent effectMaterial = new EffectMaterialContent();

            // Store a reference to our skinned mesh effect.
            string effectPath = Path.GetFullPath(@"Effects\Default.fx");

            effectMaterial.Effect = new ExternalReference<EffectContent>(effectPath);

            // Copy texture settings from the input
            // BasicMaterialContent over to our new material.
            if (basicMaterial.Texture != null)
            {
                effectMaterial.OpaqueData.Add("TextureEnabled", true);
                effectMaterial.Textures.Add("Texture", basicMaterial.Texture);
            }
            else
            {
                effectMaterial.OpaqueData.Add("TextureEnabled", false);
                effectMaterial.OpaqueData.Add("DiffuseColor", basicMaterial.DiffuseColor);
            }

            // Chain to the base ModelProcessor converter.)
            return base.ConvertMaterial(effectMaterial, context);
        }
    }
}