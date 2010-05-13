using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Rendering.Postprocessing
{
    public class EdgeDetection : PostProcessor
    {
        private readonly Effect _postprocessEffect;
        private readonly RenderTarget2D _sceneRenderTarget;
        private readonly RenderTarget2D _normalDepthRenderTarget;


        public EdgeDetection(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {

            _postprocessEffect = game.Content.Load<Effect>("Effects\\EdgeDetection");

            PresentationParameters pp = device.PresentationParameters;

            _sceneRenderTarget = new RenderTarget2D(device,
                                    pp.BackBufferWidth, pp.BackBufferHeight, 1,
                                    pp.BackBufferFormat, pp.MultiSampleType, pp.MultiSampleQuality);

            _normalDepthRenderTarget = new RenderTarget2D(device,
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
            device.SetRenderTarget(0, _normalDepthRenderTarget);
            device.Clear(Color.Black);
        }

        public void PrepareDrawDefault()
        {
            device.SetRenderTarget(0, _sceneRenderTarget);
            device.Clear(Color.White);
        }


        public override void Draw(GameTime gameTime)
        {
            device.SetRenderTarget(0, null);

            EffectParameterCollection parameters = _postprocessEffect.Parameters;

            Vector2 resolution = new Vector2(_sceneRenderTarget.Width,
                                             _sceneRenderTarget.Height);

            Texture2D normalDepthTexture = _normalDepthRenderTarget.GetTexture();

            parameters["ScreenResolution"].SetValue(resolution);
            parameters["NormalDepthTexture"].SetValue(normalDepthTexture);

            spriteBatch.Begin(SpriteBlendMode.None,
                              SpriteSortMode.Immediate,
                              SaveStateMode.None);

            _postprocessEffect.Begin();
            _postprocessEffect.CurrentTechnique.Passes[0].Begin();

            spriteBatch.Draw(_sceneRenderTarget.GetTexture(), Vector2.Zero, Color.White);

            spriteBatch.End();

            _postprocessEffect.CurrentTechnique.Passes[0].End();
            _postprocessEffect.End();
        }
    }
}