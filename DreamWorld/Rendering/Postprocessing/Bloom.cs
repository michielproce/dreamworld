using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Rendering.Postprocessing
{
    public class Bloom : PostProcessor
    {
        public float BloomThreshold { get; set; }
        public float BlurAmount { get; set; }
        public float BloomIntensity { get; set; }
        public float BaseIntensity { get; set; }
        public float BloomSaturation { get; set; }
        public float BaseSaturation { get; set; }

        private readonly Effect _bloomExtractEffect;
        private readonly Effect _bloomCombineEffect;
        private readonly Effect _gaussianBlurEffect;

        readonly ResolveTexture2D _resolveTarget;
        readonly RenderTarget2D _renderTarget1;
        readonly RenderTarget2D _renderTarget2;

        public Bloom(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {                  
            _bloomExtractEffect = game.Content.Load<Effect>(@"Effects\BloomExtract");
            _bloomCombineEffect = game.Content.Load<Effect>(@"Effects\BloomCombine");
            _gaussianBlurEffect = game.Content.Load<Effect>(@"Effects\GaussianBlur");

            PresentationParameters pp = device.PresentationParameters;

            int width = pp.BackBufferWidth;
            int height = pp.BackBufferHeight;            

            SurfaceFormat format = pp.BackBufferFormat;
            
            _resolveTarget = new ResolveTexture2D(device, width, height, 1,
                format);

            width /= 2;
            height /= 2;
            
            _renderTarget1 = new RenderTarget2D(device, width, height, 1,
                format, pp.MultiSampleType, pp.MultiSampleQuality);
            _renderTarget2 = new RenderTarget2D(device, width, height, 1,
                format, pp.MultiSampleType, pp.MultiSampleQuality);
        }

        public override void Draw(GameTime gameTime)
        {            
            device.ResolveBackBuffer(_resolveTarget);
            
            // Pass 1: Extract bloom
            _bloomExtractEffect.Parameters["BloomThreshold"].SetValue(BloomThreshold);
            DrawFullscreenQuad(_resolveTarget, _renderTarget1, _bloomExtractEffect);

            // Pass 2: Horizontal Blur
            SetBlurEffectParameters(1.0f / _renderTarget1.Width, 0);
            DrawFullscreenQuad(_renderTarget1.GetTexture(), _renderTarget2, _gaussianBlurEffect);

            // Pass 3: Vertical Blur
            SetBlurEffectParameters(0, 1.0f / _renderTarget1.Height);
            DrawFullscreenQuad(_renderTarget2.GetTexture(), _renderTarget1, _gaussianBlurEffect);
            
            // Pass 4: Combine
            device.SetRenderTarget(0, null);

            EffectParameterCollection parameters = _bloomCombineEffect.Parameters;

            parameters["BloomIntensity"].SetValue(BloomIntensity);
            parameters["BaseIntensity"].SetValue(BaseIntensity);
            parameters["BloomSaturation"].SetValue(BloomSaturation);
            parameters["BaseSaturation"].SetValue(BaseSaturation);

            device.Textures[1] = _resolveTarget;

            Viewport viewport = device.Viewport;

            DrawFullscreenQuad(_renderTarget1.GetTexture(),
                               viewport.Width, viewport.Height,
                               _bloomCombineEffect);
        }


        private void DrawFullscreenQuad(Texture2D texture, RenderTarget2D renderTarget, Effect effect)
        {
            device.SetRenderTarget(0, renderTarget);

            DrawFullscreenQuad(texture, renderTarget.Width, renderTarget.Height, effect);

            device.SetRenderTarget(0, null);
        }


        private void DrawFullscreenQuad(Texture2D texture, int width, int height, Effect effect)
        {
            spriteBatch.Begin(SpriteBlendMode.None, SpriteSortMode.Immediate, SaveStateMode.None);
            
            effect.Begin();
            effect.CurrentTechnique.Passes[0].Begin();
            
            spriteBatch.Draw(texture, new Rectangle(0, 0, width, height), Color.White);
            spriteBatch.End();
            
            effect.CurrentTechnique.Passes[0].End();
            effect.End();
        }

        
        private void SetBlurEffectParameters(float dx, float dy)
        {
            EffectParameter weightsParameter = _gaussianBlurEffect.Parameters["SampleWeights"];
            EffectParameter offsetsParameter = _gaussianBlurEffect.Parameters["SampleOffsets"];

            int sampleCount = weightsParameter.Elements.Count;

            float[] sampleWeights = new float[sampleCount];
            Vector2[] sampleOffsets = new Vector2[sampleCount];

            sampleWeights[0] = ComputeGaussian(0);
            sampleOffsets[0] = new Vector2(0);

            float totalWeights = sampleWeights[0];

            for (int i = 0; i < sampleCount / 2; i++)
            {
                float weight = ComputeGaussian(i + 1);

                sampleWeights[i * 2 + 1] = weight;
                sampleWeights[i * 2 + 2] = weight;

                totalWeights += weight * 2;

                float sampleOffset = i * 2 + 1.5f;

                Vector2 delta = new Vector2(dx, dy) * sampleOffset;

                sampleOffsets[i * 2 + 1] = delta;
                sampleOffsets[i * 2 + 2] = -delta;
            }

            for (int i = 0; i < sampleWeights.Length; i++)
            {
                sampleWeights[i] /= totalWeights;
            }

            weightsParameter.SetValue(sampleWeights);
            offsetsParameter.SetValue(sampleOffsets);
        }


        private float ComputeGaussian(float n)
        {
            float theta = BlurAmount;

            return (float)((1.0 / Math.Sqrt(2 * Math.PI * theta)) *
                           Math.Exp(-(n * n) / (2 * theta * theta)));
        }

    }
}
