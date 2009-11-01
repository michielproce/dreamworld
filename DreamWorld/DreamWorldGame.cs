using System;
using System.Drawing;
using DreamWorld.Cameras;
using DreamWorld.Entities;
using DreamWorld.InputHandlers;
using DreamWorld.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color=Microsoft.Xna.Framework.Graphics.Color;

namespace DreamWorld
{
    public class DreamWorldGame : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }
        public InputHandler[] InputHandlers { get; private set; }
        public Player Player { get; private set; }
        public Camera CurrentCamera { get; set; }
        public Level CurrentLevel { get; set; }

        public Effect DefaultEffect { get; private set; }
        // Postprocessing
        public bool DrawNormalDepth { get; private set; }
        public Effect NormalDepthEffect { get; private set; }
        private Effect postprocessEffect;
        private RenderTarget2D sceneRenderTarget;
        private RenderTarget2D normalDepthRenderTarget;
        private SpriteBatch spriteBatch;
        
        public DreamWorldGame()
        {
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            InputHandlers = new InputHandler[3];
            InputHandlers[0] = new GamepadHandler(this);
            InputHandlers[1] = new KeyboardHandler(this);
            InputHandlers[2] = new MouseHandler(this);
            foreach (InputHandler inputHandler in InputHandlers)
            {
                Components.Add(inputHandler);   
            }

//            Player = new Player(this);
//            Components.Add(Player);

            CurrentCamera = new DebugCamera(this);
            Components.Add(CurrentCamera);

            CurrentLevel = new TestLevel(this);
            Components.Add(CurrentLevel);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            DefaultEffect = Content.Load<Effect>(@"Effects\Default");
            // Postprocessing
            NormalDepthEffect = Content.Load<Effect>(@"Effects\NormalDepth");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            postprocessEffect = Content.Load<Effect>("Effects\\Postprocess");

            PresentationParameters pp = GraphicsDevice.PresentationParameters;

            sceneRenderTarget = new RenderTarget2D(GraphicsDevice,
                pp.BackBufferWidth, pp.BackBufferHeight, 1,
                pp.BackBufferFormat, pp.MultiSampleType, pp.MultiSampleQuality);

            normalDepthRenderTarget = new RenderTarget2D(GraphicsDevice,
                pp.BackBufferWidth, pp.BackBufferHeight, 1,
                pp.BackBufferFormat, pp.MultiSampleType, pp.MultiSampleQuality);
            base.LoadContent();
        }

        protected override void Draw(GameTime gameTime)
        {
            RenderState renderState = GraphicsDevice.RenderState;
            renderState.AlphaBlendEnable = false;
            renderState.AlphaTestEnable = false;
            renderState.DepthBufferEnable = true;

            // Draw NormalDepth
            GraphicsDevice.SetRenderTarget(0, normalDepthRenderTarget);

            GraphicsDevice.Clear(Color.Black);
            DrawNormalDepth = true;
            base.Draw(gameTime);

            // Draw Scene
            GraphicsDevice.SetRenderTarget(0, sceneRenderTarget);

            GraphicsDevice.Clear(Color.White);
            DrawNormalDepth = false;
            base.Draw(gameTime);

            GraphicsDevice.SetRenderTarget(0, null);

            ApplyPostprocess();
        }

        private void ApplyPostprocess()
        {
            EffectParameterCollection parameters = postprocessEffect.Parameters;

            Vector2 resolution = new Vector2(sceneRenderTarget.Width,
                                                sceneRenderTarget.Height);

            Texture2D normalDepthTexture = normalDepthRenderTarget.GetTexture();

            parameters["EdgeWidth"].SetValue(1f);
            parameters["EdgeIntensity"].SetValue(1f);
            parameters["ScreenResolution"].SetValue(resolution);
            parameters["NormalDepthTexture"].SetValue(normalDepthTexture);

            postprocessEffect.CurrentTechnique =
                                   postprocessEffect.Techniques["EdgeDetect"];

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
