using System.ComponentModel;
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace DreamWorldContentPipeline
{
    [ContentProcessor(DisplayName = "Default Model Processor")]
    public class DefaultModelProcessor :ModelProcessor
    {
        private string effectFile = "Default.fx";
        [DisplayName("Effect File")]
        [DefaultValue("Default.fx")]
        [Description("The effect file to apply to this model.")]
        public string EffectFile
        {
            get { return effectFile; }
            set { effectFile = value; }
        }

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
                    "DefaultModelProcessor only supports BasicMaterialContent, " +
                    "but input mesh uses {0}.", material.GetType()));
            }

            EffectMaterialContent effectMaterial = new EffectMaterialContent();

            // Store a reference to our skinned mesh effect.
            string effectPath = Path.GetFullPath(@"Effects\" + effectFile);

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