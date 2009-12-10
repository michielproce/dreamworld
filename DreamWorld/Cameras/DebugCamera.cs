using System;
using System.Collections.Generic;
using DreamWorld.Entities;
using DreamWorld.Entities.Global;
using DreamWorld.Helpers.Debug;
using DreamWorld.Interface.Debug.Forms;
using DreamWorld.Levels;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Cameras
{
    public class DebugCamera : Camera
    {        

        private SpriteBatch spriteBatch;
        private Texture2D reticleTexture;
        private Vector2 reticlePosition;
        private bool mouseLook = true;

        public const float MaxPitch = MathHelper.PiOver2 * .99f; // Matrix.createLookAt gets confused with maxPitch > 90 degrees
        private Vector3 lookAt;       
        private float yaw;
        private float pitch;

        public EntityForm Form { get; private set;}

        public DebugCamera()
        {
            Form = new EntityForm();            
        }

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
            // Look around with the mouse
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
                if (mouseLook)
                {
                    float distance;
                    float closestDistance = float.MaxValue;
                    bool collisionDetected = false;
                    Entity selectedEntity = null;
                    Ray cameraRay = new Ray(Position, Direction);
                    foreach (KeyValuePair<string, Entity> pair in GameScreen.Instance.Level.Entities)
                    {                       
                        if (pair.Value.SpawnInformation == null) // No spawninfo == special object
                            continue;
                        foreach (ModelMesh mesh in pair.Value.Model.Meshes)
                        {
                            List<Vector3[]> triangles = TriangleFinder.find(mesh, pair.Value.World);
                            foreach (Vector3[] triangle in triangles)
                            {
                                if (Collision.RayTriangleIntersect(cameraRay, triangle, out distance))
                                {
                                    if(distance < closestDistance)
                                    {
                                        selectedEntity = pair.Value;
                                        collisionDetected = true;
                                        closestDistance = distance;
                                    }                                                                        
                                }
                            }
                        }
                    }
                    if(collisionDetected)
                        ShowForm(selectedEntity);                                               
                }
                else
                {
                    // Only accept click within the viewport
                    // TODO: One problem remains: if the form is over the viewport mouselook will be toggled.
                    Point mPos = inputManager.Mouse.Position;
                    Viewport vp = inputManager.Game.GraphicsDevice.Viewport;
                    if (mPos.X >= 0 && mPos.Y >= 0 && mPos.X < vp.Width && mPos.Y < vp.Height)
                    {
                        Form.Hide();
                        ToggleMouseLook(true);
                    }
                    
                }
            }

            if(inputManager.Debug.NewEntity)
            {
                String name = "entity" + level.Entities.Count;
                Vector3 spawnPosition = Position + RotatedDirection(new Vector3(0, 0, -50));                
                spawnPosition.X = (float)Math.Round(spawnPosition.X);
                spawnPosition.Y = (float)Math.Round(spawnPosition.Y);
                spawnPosition.Z = (float)Math.Round(spawnPosition.Z);
                SpawnInformation spawn = new SpawnInformation
                     {
                         Name = name,
                         TypeName =
                             typeof (PlaceHolder).Namespace + "." + typeof (PlaceHolder).Name,
                         Position = spawnPosition
                     };

                PlaceHolder placeHolder = new PlaceHolder {
                    SpawnInformation = spawn
                };
                level.AddEntity(name, placeHolder);
                level.LevelInformation.Spawns.Add(spawn);
                LevelInformation.Save(level.LevelInformation,
                                      level.LevelInformationFileName);
                ShowForm(placeHolder);
            }

            // Update the form                        
            Form.UpdateEntity();                
             

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

        private void ShowForm(Entity entity)
        {
            if (Form.IsDisposed)
                Form = new EntityForm();
            Form.Entity = entity;
            Form.Level = GameScreen.Instance.Level;
            Form.DebugCamera = this;
            Form.UpdateForm();            
            Form.Show();
            ToggleMouseLook(false);
        }

        public void ToggleMouseLook(bool enabled)
        {            
            mouseLook = enabled;
            inputManager.Game.IsMouseVisible = !mouseLook;
            inputManager.Mouse.IgnoreReset = !mouseLook;
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

        public void DisposeForm()
        {
            ToggleMouseLook(true);
            Form.Dispose();
        }
    }
}
