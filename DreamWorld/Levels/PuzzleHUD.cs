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

        private GraphicsDevice device;
        private GameScreen gameScreen;
        private Model model;
        private Matrix world;
        private Matrix[] transforms;        
        private Viewport viewport;
        private Viewport originalViewport;
        
        private ModelMesh[] axles;
        private int currentAxle;
        private int currentDirection;

        private float rotation;

        public bool Hidden { get; set;}

        public PuzzleHUD(GameScreen gameScreen)
        {
            this.gameScreen = gameScreen;
            this.device =  gameScreen.GraphicsDevice;
            
            axles = new ModelMesh[3];
            currentAxle = XZ;
            currentDirection = CW;
        }

        public void Cycle(int direction)
        {
            if(direction == 0)
                return;

            if(direction != currentDirection)
            {
                currentDirection = direction;   
            } else
            {
                CycleAxle(direction);
            }
        }

        private void CycleAxle(int direction)
        {
            currentAxle += direction;            
            if(currentAxle > axles.Length - 1)            
                currentAxle = 0;
            if (currentAxle < 0)
                currentAxle = axles.Length - 1;            
            rotation = 0;            
        }

        public Vector3 CurrentDirection
        {
            get
            {
                Vector3 v = Vector3.Zero;
                
                if (currentAxle == XZ)
                    v = new Vector3(0, 1, 0);
                if (currentAxle == XY)
                    v = new Vector3(0, 0, 1);
                if (currentAxle == YZ)
                    v = new Vector3(1, 0, 0);

                v *= currentDirection;

                return v;
            }
        }

        public void Load(ContentManager content)
        {
            viewport = new Viewport();
            viewport.Height = (int)(height*device.Viewport.Height);
            viewport.Width = (int)(height*device.Viewport.Height);
            viewport.X = device.Viewport.TitleSafeArea.Right/2 - viewport.Width/2;
            viewport.Y = device.Viewport.TitleSafeArea.Bottom - viewport.Height;

            model = content.Load<Model>(@"Models\Puzzle\Axles");

            world = Matrix.CreateScale(new Vector3(.5f));

            transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            
            foreach (ModelMesh mesh in model.Meshes)
            {
                if(mesh.Name.EndsWith("XZ", true, null))
                    axles[XZ] = mesh;
                if (mesh.Name.EndsWith("XY", true, null))
                    axles[XY] = mesh;
                if (mesh.Name.EndsWith("YZ", true, null))
                    axles[YZ] = mesh;
                foreach (Effect effect in mesh.Effects)
                {
                    effect.Parameters["projection"].SetValue(Matrix.CreatePerspective(1,1,1,10));
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            rotation += MathHelper.TwoPi/500f * currentDirection;
        }

        public void Draw(GameTime gameTime)
        {            
            if(Hidden)
                return;

            originalViewport = device.Viewport;
            device.Viewport = viewport;
            device.RenderState.DepthBufferEnable = true;            
            
            Vector3 cameraPosition = -Vector3.Normalize(gameScreen.Camera.Direction);
            cameraPosition.Y += 0.35f;
            cameraPosition.Normalize();
            cameraPosition *= 2.4f;
            
            foreach (ModelMesh mesh in model.Meshes) {
                foreach (Effect effect in mesh.Effects)
                {
                    effect.Parameters["view"].SetValue(Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up));
                    
                    if(mesh.Equals(axles[currentAxle]))
                    {
                        effect.CurrentTechnique = effect.Techniques["Highlight"];                        
                        if(currentAxle == XZ)
                            effect.Parameters["world"].SetValue(transforms[mesh.ParentBone.Index] * world * Matrix.CreateRotationY(rotation));
                        if (currentAxle == XY)
                            effect.Parameters["world"].SetValue(transforms[mesh.ParentBone.Index] * world * Matrix.CreateRotationZ(rotation));
                        if (currentAxle == YZ)
                            effect.Parameters["world"].SetValue(transforms[mesh.ParentBone.Index] * world * Matrix.CreateRotationX(rotation));                        
                    } 
                    else
                    {
                        effect.CurrentTechnique = effect.Techniques["Default"];
                        effect.Parameters["world"].SetValue(transforms[mesh.ParentBone.Index] * world);
                    }
                }
                mesh.Draw();
            }

            device.Viewport = originalViewport;
            device.RenderState.DepthBufferEnable = false;
        }
    }
}
