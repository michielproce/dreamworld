using System;
using DreamWorld.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Postprocessing
{
    public class EdgeDetection : PostProcessor
    {
        private Effect postprocessEffect;
        private RenderTarget2D sceneRenderTarget;
        private RenderTarget2D normalDepthRenderTarget;


        public EdgeDetection(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {

            postprocessEffect = game.Content.Load<Effect>("Effects\\EdgeDetection");

            PresentationParameters pp = device.PresentationParameters;

            sceneRenderTarget = new RenderTarget2D(device,
                                    pp.BackBufferWidth, pp.BackBufferHeight, 1,
                                    pp.BackBufferFormat, pp.MultiSampleType, pp.MultiSampleQuality);

            normalDepthRenderTarget = new RenderTarget2D(device,
                                    pp.BackBufferWidth, pp.BackBufferHeight, 1,
                                    pp.BackBufferFormat, pp.MultiSampleType, pp.MultiSampleQuality);

            
        }

        public void PrepareDraw()
        {
            RenderState renderState = device.RenderState;
            renderState.AlphaBlendEnable = false;
            renderState.AlphaTestEnable = false;
            renderState.DepthBufferEnable = true;
        }

        public void PrepareDrawNormalDepth()
        {
            device.SetRenderTarget(0, normalDepthRenderTarget);
            device.Clear(Color.Black);
        }

        public void PrepareDrawDefault()
        {
            device.SetRenderTarget(0, sceneRenderTarget);
            device.Clear(Color.CornflowerBlue);
        }


        public override void Draw(GameTime gameTime)
        {
            device.SetRenderTarget(0, null);

            EffectParameterCollection parameters = postprocessEffect.Parameters;

            Vector2 resolution = new Vector2(sceneRenderTarget.Width,
                                             sceneRenderTarget.Height);

            Texture2D normalDepthTexture = normalDepthRenderTarget.GetTexture();

            parameters["ScreenResolution"].SetValue(resolution);
            parameters["NormalDepthTexture"].SetValue(normalDepthTexture);

            spriteBatch.Begin(SpriteBlendMode.None,
                              SpriteSortMode.Immediate,
                              SaveStateMode.None);

            postprocessEffect.Begin();
            postprocessEffect.CurrentTechnique.Passes[0].Begin();

            spriteBatch.Draw(sceneRenderTarget.GetTexture(), Vector2.Zero, Color.White);

            spriteBatch.End();

            postprocessEffect.CurrentTechnique.Passes[0].End();
            postprocessEffect.End();
        }
    }
}