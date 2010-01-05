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

        public override void Initialize()
        {
            base.Initialize();

            Body.DisableBody();
        }

        protected override void LoadContent()
        {
            Model = GameScreen.Content.Load<Model>(@"Models\Skyboxes\" + skybox);     
            base.LoadContent();
        }

        protected override Matrix GenerateWorldMatrix()
        {
            return Matrix.CreateTranslation(Body.Position);
        }

        public override void Update(GameTime gameTime)
        {
            Body.Position = GameScreen.Camera.Position;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, string technique)
        {           
            GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Clamp;
            GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Clamp;
            GraphicsDevice.RenderState.DepthBufferWriteEnable = false;
            base.Draw(gameTime, technique);
            GraphicsDevice.RenderState.DepthBufferWriteEnable = true;
            GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
            GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;
        }
    }
}
