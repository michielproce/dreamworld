﻿using System;
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
        private bool mouseLook = true;

        public const float MaxPitch = MathHelper.PiOver2 * .99f; // Matrix.createLookAt gets confused with maxPitch > 90 degrees
        private Vector3 lookAt;       
        private float yaw;
        private float pitch;

        public EntityForm Form { get; private set;}

        public DebugCamera()
        {
            Form = new EntityForm(GameScreen.Instance.Level is PuzzleLevel);            
        }

        public override void Initialize()
        {            
            Projection = Matrix.CreatePerspectiveFieldOfView(
               MathHelper.ToRadians(45.0f),
               device.Viewport.AspectRatio,
               1.0f,
               10000.0f);
            
            spriteBatch = new SpriteBatch(inputManager.Game.GraphicsDevice);

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
                    foreach (Group group in GameScreen.Instance.Level.Groups.Values)
                    {
                        foreach (Entity entity in group.Entities.Values)
                        {
                            if (entity.SpawnInformation == null) // No spawninfo == special object
                                continue;

                            foreach (ModelMesh mesh in entity.Model.Meshes)
                            {
                                List<Vector3[]> triangles = TriangleFinder.find(mesh, entity.World);
                                foreach (Vector3[] triangle in triangles)
                                {
                                    if (Collision.RayTriangleIntersect(cameraRay, triangle, out distance))
                                    {
                                        if(distance < closestDistance)
                                        {
                                            selectedEntity = entity;
                                            collisionDetected = true;
                                            closestDistance = distance;
                                        }                                                                        
                                    }
                                }
                            }
                        }
                    }
                    if(collisionDetected)
                        ShowForm(selectedEntity);                                               
                }                
            }

            if(inputManager.Debug.NewEntity)
            {
                String name = "entity" + DateTime.Now.Ticks;
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

                Entity placeHolder = Entity.CreateFromSpawnInfo(spawn);
                placeHolder.Initialize();
                placeHolder.Spawn();
                level.LevelInformation.Spawns.Add(spawn);
                LevelInformation.Save(level.LevelInformation,
                                      level.LevelInformationFileName);
                ShowForm(placeHolder);
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

        private void ShowForm(Entity entity)
        {
            ToggleMouseLook(false);
            inputManager.DisableInput = true;
            if (Form.IsDisposed)
                Form = new EntityForm(GameScreen.Instance.Level is PuzzleLevel);
            Form.Entity = entity;
            Form.Level = GameScreen.Instance.Level;
            Form.DebugCamera = this;
            Form.UpdateForm();            
            Form.Show();
        }

        public void ToggleMouseLook(bool enabled)
        {            
            mouseLook = enabled;
            inputManager.Game.IsMouseVisible = !mouseLook;
            inputManager.Mouse.IgnoreReset = !mouseLook;
            inputManager.Mouse.ResetMouse();
            inputManager.DisableInput = false;
        }

        public override Vector3 Direction
        {
            get
            {
                return Vector3.Normalize(lookAt - Position);
            }
        }

        public void DisposeForm()
        {
            ToggleMouseLook(true);
            Form.Dispose();
        }        
    }
}
