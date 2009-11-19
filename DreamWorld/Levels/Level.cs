using System;
using System.Collections.Generic;
using DreamWorld.Entities;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Levels
{
    public abstract class Level
    {
        public DreamWorldGame Game { protected get; set; }
        public GameScreen GameScreen { get; set; }
        public Terrain Terrain { get; protected set; }
        public Skybox Skybox { get; protected set; }

        public Player Player { get; private set; }
        
        private List<Entity> entities;

        // Postprocessing
        public static bool DrawNormalDepth { get; private set; }
        public static BasicEffect BasicEffect { get; private set; }
        public static Effect NormalDepthEffect { get; private set; }
        private Effect postprocessEffect;
        private RenderTarget2D sceneRenderTarget;
        private RenderTarget2D normalDepthRenderTarget;
        private SpriteBatch spriteBatch;

        protected Level()
        {
            entities = new List<Entity>();
            
        }


        protected void AddEntity(Entity entity)
        {
            entity.Level = this;
            entity.GameScreen = GameScreen;
            entity.Game = GameScreen.ScreenManager.Game as DreamWorldGame;
            entities.Add(entity);
        }


        public virtual void Initialize()
        {
            Player = new Player
                         {
                             InputManager = ((DreamWorldGame) GameScreen.ScreenManager.Game).InputManager
                         };
            AddEntity(Player);
            foreach (Entity entity in entities)
                entity.Initialize();

           // postprocessing
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            postprocessEffect = Game.Content.Load<Effect>("Effects\\Postprocess");

            PresentationParameters pp = Game.GraphicsDevice.PresentationParameters;

            sceneRenderTarget = new RenderTarget2D(GameScreen.ScreenManager.Game.GraphicsDevice,
                pp.BackBufferWidth, pp.BackBufferHeight, 1,
                pp.BackBufferFormat, pp.MultiSampleType, pp.MultiSampleQuality);

            normalDepthRenderTarget = new RenderTarget2D(Game.GraphicsDevice,
                pp.BackBufferWidth, pp.BackBufferHeight, 1,
                pp.BackBufferFormat, pp.MultiSampleType, pp.MultiSampleQuality);
        }





        public virtual void Update(GameTime gameTime)
        {
            foreach (Entity entity in entities)
                entity.Update(gameTime);
        }


        public virtual void Draw(GameTime gameTime)
        {
            RenderState renderState = Game.GraphicsDevice.RenderState;
            renderState.AlphaBlendEnable = false;
            renderState.AlphaTestEnable = false;
            renderState.DepthBufferEnable = true;

            // NormalDepth
            Game.GraphicsDevice.SetRenderTarget(0, normalDepthRenderTarget);
            Game.GraphicsDevice.Clear(Color.Black);
            foreach (Entity entity in entities)
            {
                if(!entity.IgnoreEdgeDetection)
                    entity.Draw(gameTime, "NormalDepth");
            }
            // Default
            Game.GraphicsDevice.SetRenderTarget(0, sceneRenderTarget);
            Game.GraphicsDevice.Clear(Color.CornflowerBlue);
            foreach (Entity entity in entities)
                entity.Draw(gameTime, "Default");
            
            Game.GraphicsDevice.SetRenderTarget(0, null);

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
