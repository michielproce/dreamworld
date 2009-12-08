using System;
using System.Collections.Generic;
using DreamWorld.Entities;
using DreamWorld.Helpers.Debug;
using DreamWorld.InputManagement;
using DreamWorld.Levels;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Cameras
{
    class DebugCamera : Camera
    {        
        public Entity SelectedEntity { get; private set; }
        public string SelectedEntityName { get; private set; }
     
        public const float MaxPitch = MathHelper.PiOver2 * .99f; // Matrix.createLookAt gets confused with maxPitch > 90 degrees

        private Vector3 lookAt;
        
        private bool mouseLook = true;

        private SpriteBatch spriteBatch;        
        private Texture2D reticleTexture;
        private Vector2 reticlePosition;

        private float yaw;
        private float pitch;

        public override void Initialize()
        {
            Projection = Matrix.CreatePerspectiveFieldOfView(
               MathHelper.ToRadians(45.0f),
               device.Viewport.AspectRatio,
               1.0f,
               10000.0f);
            
            spriteBatch = new SpriteBatch(inputManager.Game.GraphicsDevice);                        
            reticleTexture = GameScreen.Instance.Content.Load<Texture2D>(@"Textures\Debug\Reticle");
            reticlePosition = new Vector2(inputManager.Game.GraphicsDevice.Viewport.Width / 2 - reticleTexture.Width / 2, 
                inputManager.Game.GraphicsDevice.Viewport.Height / 2 - reticleTexture.Height / 2);
            
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (inputManager.Debug.ToggleMouseLook)
            {
                mouseLook = !mouseLook;
                inputManager.Game.IsMouseVisible = !mouseLook;
                inputManager.Mouse.IgnoreReset = !mouseLook;
            }

            if (mouseLook)
            {                
                Move(inputManager.Debug.Movement);
                Rotate(inputManager.Debug.Rotation.X, inputManager.Debug.Rotation.Y);
                lookAt = Position + RotatedDirection(Vector3.Forward);
                View = Matrix.CreateLookAt(
                        Position,
                        lookAt,
                        Vector3.Up);     
            }

            if (inputManager.Debug.SelectEntity)
            {
                float distance;
                bool collisionDetected = false;
                Ray cameraRay = new Ray(Position, Direction);
                foreach (KeyValuePair<string, Entity> pair in GameScreen.Instance.Level.Entities)
                {
                    if (collisionDetected)
                        break;
                    if (pair.Value.IgnoreDebugHighlight)
                        continue;
                    foreach (ModelMesh mesh in pair.Value.Model.Meshes)
                    {
                        List<Vector3[]> triangles = TriangleFinder.find(mesh, pair.Value.World);
                        foreach (Vector3[] triangle in triangles)
                        {
                            if (Collision.RayTriangleIntersect(cameraRay, triangle, out distance))
                            {
                                SelectedEntityName = pair.Key;
                                SelectedEntity = pair.Value;
                                collisionDetected = true;
                            }
                        }
                    }
                }
                if (!collisionDetected)
                {
                    SelectedEntityName = null;
                    SelectedEntity = null;
                }
            }
           
            base.Update(gameTime);
        }

        private void Move(Vector3 direction)
        {
             Position += RotatedDirection(direction);
        }

        private void Rotate(float yaw, float pitch)
        {
            this.yaw += yaw;
            this.pitch += pitch;           

            if (this.pitch > MaxPitch)
                this.pitch = MaxPitch;
            if (this.pitch < -MaxPitch)
                this.pitch = -MaxPitch;
        }

        private Vector3 RotatedDirection(Vector3 direction)
        {            
            return Vector3.Transform(direction, Quaternion.CreateFromYawPitchRoll(yaw, pitch, 0));
        }

        public override Vector3 Direction
        {
            get
            {
                return Vector3.Normalize(lookAt - Position);
            }
        }

        public void DrawReticle()
        {
            spriteBatch.Begin();
            spriteBatch.Draw(reticleTexture, reticlePosition, Color.White);
            spriteBatch.End();
        }
    }
}
