using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Levels
{
    public class PuzzleHUD
    {
        public const int XZ = 0;
        public const int XY = 1;
        public const int YZ = 2;
        public const int CW = 1;
        public const int CCW = -1;

        private const float height = .25f; // Height relative to the game height

        private readonly GraphicsDevice _device;
        private readonly GameScreen _gameScreen;
        private Model _model;
        private Matrix _world;
        private Matrix[] _transforms;        
        private Viewport _viewport;
        private Viewport _originalViewport;
        
        private readonly ModelMesh[] _axles;
        private int _currentAxle;
        private int _currentDirection;

        private float _rotation;

        public bool Hidden { get; set;}

        public PuzzleHUD(GameScreen gameScreen)
        {
            _gameScreen = gameScreen;
            _device =  gameScreen.GraphicsDevice;
            
            _axles = new ModelMesh[3];
            _currentAxle = XZ;
            _currentDirection = CW;
        }

        public void Cycle(int direction)
        {
            if(direction == 0)
                return;

            if(direction != _currentDirection)
            {
                _currentDirection = direction;   
            } else
            {
                CycleAxle(direction);
            }
        }

        private void CycleAxle(int direction)
        {
            _currentAxle += direction;            
            if(_currentAxle > _axles.Length - 1)            
                _currentAxle = 0;
            if (_currentAxle < 0)
                _currentAxle = _axles.Length - 1;            
            _rotation = 0;            
        }

        public Vector3 CurrentDirection
        {
            get
            {
                Vector3 v = Vector3.Zero;
                
                if (_currentAxle == XZ)
                    v = new Vector3(0, 1, 0);
                if (_currentAxle == XY)
                    v = new Vector3(0, 0, 1);
                if (_currentAxle == YZ)
                    v = new Vector3(1, 0, 0);

                v *= _currentDirection;

                return v;
            }
        }

        public void Load(ContentManager content)
        {
            _viewport = new Viewport();
            _viewport.Height = (int)(height*_device.Viewport.Height);
            _viewport.Width = (int)(height*_device.Viewport.Height);
            _viewport.X = _device.Viewport.TitleSafeArea.Right/2 - _viewport.Width/2;
            _viewport.Y = _device.Viewport.TitleSafeArea.Bottom - _viewport.Height;

            _model = content.Load<Model>(@"Models\Puzzle\Axles");

            _world = Matrix.CreateScale(new Vector3(.5f));

            _transforms = new Matrix[_model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(_transforms);

            
            foreach (ModelMesh mesh in _model.Meshes)
            {
                if(mesh.Name.EndsWith("XZ", true, null))
                    _axles[XZ] = mesh;
                if (mesh.Name.EndsWith("XY", true, null))
                    _axles[XY] = mesh;
                if (mesh.Name.EndsWith("YZ", true, null))
                    _axles[YZ] = mesh;
                foreach (Effect effect in mesh.Effects)
                {
                    effect.Parameters["projection"].SetValue(Matrix.CreatePerspective(1,1,1,10));
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            _rotation += MathHelper.TwoPi/500f * _currentDirection;
        }

        public void Draw(GameTime gameTime)
        {            
            if(Hidden)
                return;

            _originalViewport = _device.Viewport;
            _device.Viewport = _viewport;
            _device.RenderState.DepthBufferEnable = true;            
            
            Vector3 cameraPosition = -Vector3.Normalize(_gameScreen.Camera.Direction);
            cameraPosition.Y += 0.35f;
            cameraPosition.Normalize();
            cameraPosition *= 2.4f;
            
            foreach (ModelMesh mesh in _model.Meshes) {
                foreach (Effect effect in mesh.Effects)
                {
                    effect.Parameters["view"].SetValue(Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up));
                    
                    if(mesh.Equals(_axles[_currentAxle]))
                    {
                        effect.CurrentTechnique = effect.Techniques["Highlight"];                        
                        if(_currentAxle == XZ)
                            effect.Parameters["world"].SetValue(_transforms[mesh.ParentBone.Index] * _world * Matrix.CreateRotationY(_rotation));
                        if (_currentAxle == XY)
                            effect.Parameters["world"].SetValue(_transforms[mesh.ParentBone.Index] * _world * Matrix.CreateRotationZ(_rotation));
                        if (_currentAxle == YZ)
                            effect.Parameters["world"].SetValue(_transforms[mesh.ParentBone.Index] * _world * Matrix.CreateRotationX(_rotation));                        
                    } 
                    else
                    {
                        effect.CurrentTechnique = effect.Techniques["Default"];
                        effect.Parameters["world"].SetValue(_transforms[mesh.ParentBone.Index] * _world);
                    }
                }
                mesh.Draw();
            }

            _device.Viewport = _originalViewport;
            _device.RenderState.DepthBufferEnable = false;
        }
    }
}
