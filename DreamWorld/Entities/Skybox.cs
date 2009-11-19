using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Entities
{
    public class Skybox : Entity
    {
        private string skybox;

        public Skybox(string skybox)
        {
            this.skybox = skybox;
            IgnoreEdgeDetection = true;
        }

        protected override void LoadContent()
        {
            Model = Game.Content.Load<Model>(@"Models\Skyboxes\" + skybox);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Position = GameScreen.CurrentCamera.Position;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, string technique)
        {           
            Game.GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Clamp;
            Game.GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Clamp;
            Game.GraphicsDevice.RenderState.DepthBufferWriteEnable = false;
            base.Draw(gameTime, technique);
            Game.GraphicsDevice.RenderState.DepthBufferWriteEnable = true;
            Game.GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
            Game.GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;
        }
    }
}
