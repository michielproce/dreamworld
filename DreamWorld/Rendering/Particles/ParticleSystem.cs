using System;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Rendering.Particles
{
    public abstract class ParticleSystem
    {
        public ParticleSettings Settings { get; private set; }
        
        private readonly GameScreen _gameScreen;
        private readonly ContentManager _content;
        private readonly GraphicsDevice _device;
        

        private static Random random = new Random();               

        private Effect _particleEffect;
        private EffectParameter _effectViewParameter;
        private EffectParameter _effectProjectionParameter;
        private EffectParameter _effectViewportHeightParameter;
        private EffectParameter _effectTimeParameter;

        private ParticleVertex[] _particles;
        private DynamicVertexBuffer _vertexBuffer;        
        private VertexDeclaration _vertexDeclaration;

        private int _firstActiveParticle;
        private int _firstNewParticle;
        private int _firstFreeParticle;
        private int _firstRetiredParticle;

        private float _currentTime;
        private int _drawCounter;

        protected ParticleSystem()
        {
            _gameScreen = GameScreen.Instance;
            _content = _gameScreen.Content;
            _device = _gameScreen.GraphicsDevice;
        }

        protected abstract void InitializeSettings(ParticleSettings settings);

        public void Initialize()
        {
            Settings = new ParticleSettings();
            InitializeSettings(Settings);
            _particles = new ParticleVertex[Settings.MaxParticles];
            LoadContent();    
        }

        private void LoadContent()
        {
            _particleEffect = _content.Load<Effect>(@"Effects\Particle").Clone(_device);

            EffectParameterCollection parameters = _particleEffect.Parameters;

            // Look up shortcuts for parameters that change every frame.
            _effectViewParameter = parameters["View"];
            _effectProjectionParameter = parameters["Projection"];
            _effectViewportHeightParameter = parameters["ViewportHeight"];
            _effectTimeParameter = parameters["CurrentTime"];

            // Set the values of parameters that do not change.
            parameters["Duration"].SetValue((float)Settings.Duration.TotalSeconds);
            parameters["DurationRandomness"].SetValue(Settings.DurationRandomness);
            parameters["Gravity"].SetValue(Settings.Gravity);
            parameters["EndVelocity"].SetValue(Settings.EndVelocity);
            parameters["MinColor"].SetValue(Settings.MinColor.ToVector4());
            parameters["MaxColor"].SetValue(Settings.MaxColor.ToVector4());

            parameters["RotateSpeed"].SetValue(
                new Vector2(Settings.MinRotateSpeed, Settings.MaxRotateSpeed));

            parameters["StartSize"].SetValue(
                new Vector2(Settings.MinStartSize, Settings.MaxStartSize));

            parameters["EndSize"].SetValue(
                new Vector2(Settings.MinEndSize, Settings.MaxEndSize));

            // Load the particle texture, and set it onto the effect.
            Texture2D texture = _content.Load<Texture2D>(@"Textures\Particles\" + Settings.TextureName);

            parameters["Texture"].SetValue(texture);

            // Choose the appropriate effect technique. If these particles will never
            // rotate, we can use a simpler pixel shader that requires less GPU power.
            string techniqueName;

            if ((Settings.MinRotateSpeed == 0) && (Settings.MaxRotateSpeed == 0))
                techniqueName = "NonRotatingParticles";
            else
                techniqueName = "RotatingParticles";

            _particleEffect.CurrentTechnique = _particleEffect.Techniques[techniqueName];


            _vertexDeclaration = new VertexDeclaration(_device,
                                                      ParticleVertex.VertexElements);
            
            int size = ParticleVertex.SizeInBytes * _particles.Length;

            _vertexBuffer = new DynamicVertexBuffer(_device, size,
                                                   BufferUsage.WriteOnly |
                                                   BufferUsage.Points);
        }

        public void Update(GameTime gameTime)
        {
            _currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Retire Active Particles
            float particleDuration = (float)Settings.Duration.TotalSeconds;

            while (_firstActiveParticle != _firstNewParticle)
            {
                // Is this particle old enough to retire?
                float particleAge = _currentTime - _particles[_firstActiveParticle].Time;

                if (particleAge < particleDuration)
                    break;

                // Remember the time at which we retired this particle.
                _particles[_firstActiveParticle].Time = _drawCounter;

                // Move the particle from the active to the retired queue.
                _firstActiveParticle++;

                if (_firstActiveParticle >= _particles.Length)
                    _firstActiveParticle = 0;
            }

            RetireActiveParticles();
            FreeRetiredParticles();

            if (_firstActiveParticle == _firstFreeParticle)
                _currentTime = 0;

            if (_firstRetiredParticle == _firstActiveParticle)
                _drawCounter = 0;
        }


        private void RetireActiveParticles()
        {
            float particleDuration = (float)Settings.Duration.TotalSeconds;

            while (_firstActiveParticle != _firstNewParticle)
            {
                // Is this particle old enough to retire?
                float particleAge = _currentTime - _particles[_firstActiveParticle].Time;

                if (particleAge < particleDuration)
                    break;

                // Remember the time at which we retired this particle.
                _particles[_firstActiveParticle].Time = _drawCounter;

                // Move the particle from the active to the retired queue.
                _firstActiveParticle++;

                if (_firstActiveParticle >= _particles.Length)
                    _firstActiveParticle = 0;
            }
        }


        private void FreeRetiredParticles()
        {
            while (_firstRetiredParticle != _firstActiveParticle)
            {
                // Has this particle been unused long enough that
                // the GPU is sure to be finished with it?
                int age = _drawCounter - (int)_particles[_firstRetiredParticle].Time;

                // The GPU is never supposed to get more than 2 frames behind the CPU.
                // We add 1 to that, just to be safe in case of buggy drivers that
                // might bend the rules and let the GPU get further behind.
                if (age < 3)
                    break;

                // Move the particle from the retired to the free queue.
                _firstRetiredParticle++;

                if (_firstRetiredParticle >= _particles.Length)
                    _firstRetiredParticle = 0;
            }
        }


        public void Draw(GameTime gameTime)
        {
            _effectViewParameter.SetValue(_gameScreen.Camera.View);
            _effectProjectionParameter.SetValue(_gameScreen.Camera.Projection);

            GraphicsDevice device = _device;

            // Restore the vertex buffer contents if the graphics device was lost.
            if (_vertexBuffer.IsContentLost)
            {
                _vertexBuffer.SetData(_particles);
            }

            // If there are any particles waiting in the newly added queue,
            // we'd better upload them to the GPU ready for drawing.
            if (_firstNewParticle != _firstFreeParticle)
            {
                AddNewParticlesToVertexBuffer();
            }

            // If there are any active particles, draw them now!
            if (_firstActiveParticle != _firstFreeParticle)
            {
                SetParticleRenderStates(device.RenderState);

                // Set an effect parameter describing the viewport size. This is needed
                // to convert particle sizes into screen space point sprite sizes.
                _effectViewportHeightParameter.SetValue(device.Viewport.Height);

                // Set an effect parameter describing the current time. All the vertex
                // shader particle animation is keyed off this value.
                _effectTimeParameter.SetValue(_currentTime);

                // Set the particle vertex buffer and vertex declaration.
                device.Vertices[0].SetSource(_vertexBuffer, 0,
                                             ParticleVertex.SizeInBytes);

                device.VertexDeclaration = _vertexDeclaration;

                // Activate the particle effect.
                _particleEffect.Begin();

                foreach (EffectPass pass in _particleEffect.CurrentTechnique.Passes)
                {
                    pass.Begin();

                    if (_firstActiveParticle < _firstFreeParticle)
                    {
                        // If the active particles are all in one consecutive range,
                        // we can draw them all in a single call.
                        device.DrawPrimitives(PrimitiveType.PointList,
                                              _firstActiveParticle,
                                              _firstFreeParticle - _firstActiveParticle);
                    }
                    else
                    {
                        // If the active particle range wraps past the end of the queue
                        // back to the start, we must split them over two draw calls.
                        device.DrawPrimitives(PrimitiveType.PointList,
                                              _firstActiveParticle,
                                              _particles.Length - _firstActiveParticle);

                        if (_firstFreeParticle > 0)
                        {
                            device.DrawPrimitives(PrimitiveType.PointList,
                                                  0,
                                                  _firstFreeParticle);
                        }
                    }

                    pass.End();
                }

                _particleEffect.End();

                // Reset a couple of the more unusual renderstates that we changed,
                // so as not to mess up any other subsequent drawing.
                device.RenderState.PointSpriteEnable = false;
                device.RenderState.DepthBufferWriteEnable = true;
            }

            _drawCounter++;
        }


        private void AddNewParticlesToVertexBuffer()
        {
            const int stride = ParticleVertex.SizeInBytes;

            if (_firstNewParticle < _firstFreeParticle)
            {
                // If the new particles are all in one consecutive range,
                // we can upload them all in a single call.
                _vertexBuffer.SetData(_firstNewParticle * stride, _particles,
                                     _firstNewParticle,
                                     _firstFreeParticle - _firstNewParticle,
                                     stride, SetDataOptions.NoOverwrite);
            }
            else
            {
                // If the new particle range wraps past the end of the queue
                // back to the start, we must split them over two upload calls.
                _vertexBuffer.SetData(_firstNewParticle * stride, _particles,
                                     _firstNewParticle,
                                     _particles.Length - _firstNewParticle,
                                     stride, SetDataOptions.NoOverwrite);

                if (_firstFreeParticle > 0)
                {
                    _vertexBuffer.SetData(0, _particles,
                                         0, _firstFreeParticle,
                                         stride, SetDataOptions.NoOverwrite);
                }
            }

            // Move the particles we just uploaded from the new to the active queue.
            _firstNewParticle = _firstFreeParticle;
        }

       
        private void SetParticleRenderStates(RenderState renderState)
        {
            // Enable point sprites.
            renderState.PointSpriteEnable = true;
            renderState.PointSizeMax = 256;

            // Set the alpha blend mode.
            renderState.AlphaBlendEnable = true;
            renderState.AlphaBlendOperation = BlendFunction.Add;
            renderState.SourceBlend = Settings.SourceBlend;
            renderState.DestinationBlend = Settings.DestinationBlend;

            // Set the alpha test mode.
            renderState.AlphaTestEnable = true;
            renderState.AlphaFunction = CompareFunction.Greater;
            renderState.ReferenceAlpha = 0;

            // Enable the depth buffer (so particles will not be visible through
            // solid objects like the ground plane), but disable depth writes
            // (so particles will not obscure other particles).
            renderState.DepthBufferEnable = true;
            renderState.DepthBufferWriteEnable = false;
        }

        public void AddParticle(Vector3 position, Vector3 velocity)
        {
            // Figure out where in the circular queue to allocate the new particle.
            int nextFreeParticle = _firstFreeParticle + 1;

            if (nextFreeParticle >= _particles.Length)
                nextFreeParticle = 0;

            // If there are no free particles, we just have to give up.
            if (nextFreeParticle == _firstRetiredParticle)
                return;

            // Adjust the input velocity based on how much
            // this particle system wants to be affected by it.
            velocity *= Settings.EmitterVelocitySensitivity;

            // Add in some random amount of horizontal velocity.
            float horizontalVelocity = MathHelper.Lerp(Settings.MinHorizontalVelocity,
                                                       Settings.MaxHorizontalVelocity,
                                                       (float)random.NextDouble());

            double horizontalAngle = random.NextDouble() * MathHelper.TwoPi;

            velocity.X += horizontalVelocity * (float)Math.Cos(horizontalAngle);
            velocity.Z += horizontalVelocity * (float)Math.Sin(horizontalAngle);

            // Add in some random amount of vertical velocity.
            velocity.Y += MathHelper.Lerp(Settings.MinVerticalVelocity,
                                          Settings.MaxVerticalVelocity,
                                          (float)random.NextDouble());

            // Choose four random control values. These will be used by the vertex
            // shader to give each particle a different size, rotation, and color.
            Color randomValues = new Color((byte)random.Next(255),
                                           (byte)random.Next(255),
                                           (byte)random.Next(255),
                                           (byte)random.Next(255));

            // Fill in the particle vertex structure.
            _particles[_firstFreeParticle].Position = position;
            _particles[_firstFreeParticle].Velocity = velocity;
            _particles[_firstFreeParticle].Random = randomValues;
            _particles[_firstFreeParticle].Time = _currentTime;

            _firstFreeParticle = nextFreeParticle;
        }
    }
}
