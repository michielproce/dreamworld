using System.Collections.Generic;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Helpers.Renderers
{
    public class DebugDrawer : DrawableGameComponent
    {
        BasicEffect basicEffect;
        List<VertexPositionColor> vertexData;

        private GameScreen GameScreen;

        public DebugDrawer(Game game, GameScreen gameScreen)
            : base(game)
        {
            GameScreen = gameScreen;
            vertexData = new List<VertexPositionColor>();
        }

        public override void Initialize()
        {
            base.Initialize();

            basicEffect = new BasicEffect(GraphicsDevice, null);
            GraphicsDevice.VertexDeclaration = new VertexDeclaration(GraphicsDevice, VertexPositionColor.VertexElements);
        }

        public override void Draw(GameTime gameTime)
        {
            if (vertexData.Count == 0) return;


            GraphicsDevice.RenderState.AlphaBlendEnable = true;
            GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;

            basicEffect.AmbientLightColor = Vector3.One;
            basicEffect.View = GameScreen.Camera.View;
            basicEffect.Projection = GameScreen.Camera.Projection;
            basicEffect.VertexColorEnabled = true;

            basicEffect.Begin();
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Begin();

                GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, vertexData.ToArray(), 0, vertexData.Count - 1);

                pass.End();
            }
            basicEffect.End();

            vertexData.Clear();

            base.Draw(gameTime);
        }

        public void DrawShape(VertexPositionColor[] shape)
        {
            if (vertexData.Count > 0)
            {
                vertexData.Add(new VertexPositionColor(vertexData[vertexData.Count - 1].Position, Color.TransparentWhite));
                vertexData.Add(new VertexPositionColor(shape[0].Position, Color.TransparentWhite));
            }

            foreach (VertexPositionColor vps in shape)
            {
                vertexData.Add(vps);
            }
        }
    }
}