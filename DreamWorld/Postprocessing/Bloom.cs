using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Postprocessing
{
    public class Bloom : PostProcessor
    {
        private const float bloomThreshold  = .5f;
        private const float blurAmount      = 4f;
        private const float bloomIntensity  = 1f;
        private const float baseIntensity   = 1f;
        private const float bloomSaturation = 1f;
        private const float baseSaturation  = 1f;

        Effect bloomExtractEffect;
        Effect bloomCombineEffect;
        Effect gaussianBlurEffect;

        ResolveTexture2D resolveTarget;
        RenderTarget2D renderTarget1;
        RenderTarget2D renderTarget2;

        public Bloom(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {            
            bloomExtractEffect = game.Content.Load<Effect>(@"Effects\BloomExtract");
            bloomCombineEffect = game.Content.Load<Effect>(@"Effects\BloomCombine");
            gaussianBlurEffect = game.Content.Load<Effect>(@"Effects\GaussianBlur");

            PresentationParameters pp = device.PresentationParameters;

            int width = pp.BackBufferWidth;
            int height = pp.BackBufferHeight;

            SurfaceFormat format = pp.BackBufferFormat;

            resolveTarget = new ResolveTexture2D(device, width, height, 1,
                format);

            width /= 2;
            height /= 2;

            renderTarget1 = new RenderTarget2D(device, width, height, 1,
                format);
            renderTarget2 = new RenderTarget2D(device, width, height, 1,
                format);
        }

        public override void Draw(GameTime gameTime)
        {            
            device.ResolveBackBuffer(resolveTarget);
            
            // Pass 1: Extract bloom
            bloomExtractEffect.Parameters["BloomThreshold"].SetValue(bloomThreshold);
            DrawFullscreenQuad(resolveTarget, renderTarget1, bloomExtractEffect);

            // Pass 2: Horizontal Blur
            SetBlurEffectParameters(1.0f / (float)renderTarget1.Width, 0);
            DrawFullscreenQuad(renderTarget1.GetTexture(), renderTarget2, gaussianBlurEffect);

            // Pass 3: Vertical Blur
            SetBlurEffectParameters(0, 1.0f / (float)renderTarget1.Height);
            DrawFullscreenQuad(renderTarget2.GetTexture(), renderTarget1, gaussianBlurEffect);
            
            // Pass 4: Combine
            device.SetRenderTarget(0, null);

            EffectParameterCollection parameters = bloomCombineEffect.Parameters;

            parameters["BloomIntensity"].SetValue(bloomIntensity);
            parameters["BaseIntensity"].SetValue(baseIntensity);
            parameters["BloomSaturation"].SetValue(bloomSaturation);
            parameters["BaseSaturation"].SetValue(baseSaturation);

            device.Textures[1] = resolveTarget;

            Viewport viewport = device.Viewport;

            DrawFullscreenQuad(renderTarget1.GetTexture(),
                               viewport.Width, viewport.Height,
                               bloomCombineEffect);
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
            EffectParameter weightsParameter, offsetsParameter;

            weightsParameter = gaussianBlurEffect.Parameters["SampleWeights"];
            offsetsParameter = gaussianBlurEffect.Parameters["SampleOffsets"];

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
            float theta = blurAmount;

            return (float)((1.0 / Math.Sqrt(2 * Math.PI * theta)) *
                           Math.Exp(-(n * n) / (2 * theta * theta)));
        }

    }
}
