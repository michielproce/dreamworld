using System.Collections.Generic;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Helpers.Renderers
{
    public class DebugDrawer : DrawableGameComponent
    {
        private BasicEffect _basicEffect;
        private readonly List<VertexPositionColor> _vertexData;

        private readonly GameScreen _gameScreen;

        public DebugDrawer(Game game, GameScreen gameScreen)
            : base(game)
        {
            _gameScreen = gameScreen;
            _vertexData = new List<VertexPositionColor>();
        }

        public override void Initialize()
        {
            base.Initialize();

            _basicEffect = new BasicEffect(GraphicsDevice, null);
            GraphicsDevice.VertexDeclaration = new VertexDeclaration(GraphicsDevice, VertexPositionColor.VertexElements);
        }

        public override void Draw(GameTime gameTime)
        {
            if (_vertexData.Count == 0) return;


            GraphicsDevice.RenderState.AlphaBlendEnable = true;
            GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;

            _basicEffect.AmbientLightColor = Vector3.One;
            _basicEffect.View = _gameScreen.Camera.View;
            _basicEffect.Projection = _gameScreen.Camera.Projection;
            _basicEffect.VertexColorEnabled = true;

            _basicEffect.Begin();
            foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Begin();

                GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, _vertexData.ToArray(), 0, _vertexData.Count - 1);

                pass.End();
            }
            _basicEffect.End();

            _vertexData.Clear();

            base.Draw(gameTime);
        }

        public void DrawShape(VertexPositionColor[] shape)
        {
            if (_vertexData.Count > 0)
            {
                _vertexData.Add(new VertexPositionColor(_vertexData[_vertexData.Count - 1].Position, Color.TransparentWhite));
                _vertexData.Add(new VertexPositionColor(shape[0].Position, Color.TransparentWhite));
            }

            foreach (VertexPositionColor vps in shape)
            {
                _vertexData.Add(vps);
            }
        }
    }
}