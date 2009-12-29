using System;
using System.Collections.Generic;
using DreamWorld.ScreenManagement.Screens;
using JigLibX.Collision;
using Microsoft.Xna.Framework;

namespace DreamWorld.Entities
{
    public class Group 
    {
        private GameScreen GameScreen;

        public Dictionary<string, Entity> Entities { get; private set; }
        public GroupCenter Center;
        public Vector3 AllowedRotations;
        internal List<CollisionSkin> IgnoreCollisionSkins;

        private float AmountRotated;
        private Quaternion OriginalRotation { get; set; }
        private Quaternion TargetRotation { get; set; }
        private Quaternion Rotation
        {
            get
            {
                if (IsRotating)
                    return Quaternion.Lerp(OriginalRotation, TargetRotation, AmountRotated);
                return OriginalRotation;
            }
        }

        public bool IsRotating
        {
            get
            {
                return OriginalRotation != TargetRotation;
            }
        }

        public bool IsColliding
        {
            get
            {
                foreach (Entity entity in Entities.Values)
                {
                    for (int i = 0; i <= entity.Skin.Collisions.Count - 1; i++)
                    {
                        if (!IgnoreCollisionSkins.Contains(entity.Skin.Collisions[i].SkinInfo.Skin0) || !IgnoreCollisionSkins.Contains(entity.Skin.Collisions[i].SkinInfo.Skin1))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public Quaternion RotationSinceLastUpdate { get; private set; }

        private const float RotationSpeed = 0.05f;

        public Group()
        {
            GameScreen = GameScreen.Instance;
            Entities = new Dictionary<string, Entity>();
            IgnoreCollisionSkins = new List<CollisionSkin> {GameScreen.Level.Player.Skin};
            OriginalRotation = Quaternion.Identity;
            TargetRotation = Quaternion.Identity;
            AllowedRotations = Vector3.One;
        }

        public void Initialize()
        {
            foreach (Entity entity in Entities.Values)
            {
                entity.Initialize();

                if (entity.Skin != null && !IgnoreCollisionSkins.Contains(entity.Skin))
                    IgnoreCollisionSkins.Add(entity.Skin);
            }
        }

        public void Update(GameTime gameTime)
        {
            Quaternion oldRotation = Rotation;

            if (IsRotating)
            {
                if (IsColliding)
                {
                    CancelRotation();
                }

                AmountRotated += RotationSpeed;

                if (AmountRotated >= 1)
                {
                    OriginalRotation = TargetRotation;
                    AmountRotated = 0;
                }
            }

            RotationSinceLastUpdate = Quaternion.Concatenate(Quaternion.Inverse(oldRotation), Rotation);

            foreach (Entity entity in Entities.Values)
            {
                entity.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, string technique)
        {
            foreach (Entity entity in Entities.Values)
            {
                if(technique == "EdgeDetection")
                {
                    entity.Draw(gameTime, !entity.IgnoreEdgeDetection ? "NormalDepth" : "IgnoreNormalDepth");
                } 
                else
                {
                    entity.Draw(gameTime, technique);
                }
            }
        }

        /// <summary>
        /// This function will be called by Entity
        /// Simply set Entity.Group to change the group.
        /// </summary>
        public void AddEntity(string name, Entity entity)
        {
            if (Entities.ContainsKey(name))
                throw new InvalidOperationException("Entity " + name + " already exists");

            Entities.Add(name, entity);

            if (entity.Skin != null)
                IgnoreCollisionSkins.Add(entity.Skin);
        }

        /// <summary>
        /// This function will be called by Entity
        /// Simply set Entity.Group to null to remove it
        /// </summary>
        public void RemoveEntity(string name)
        {
            if (!Entities.ContainsKey(name))
                throw new InvalidOperationException("Entity " + name + " doesn't exist");
            Entities[name].DisablePhysics();
            Entities.Remove(name);
        }

        public void Rotate(Vector3 direction)
        {
            if (IsRotating) 
                return;

            direction = Vector3.Normalize(direction) * MathHelper.PiOver2;

//            TODO: multiple rotations at the same time
//            if (IsRotating)
//            {
//                OriginalRotation = Quaternion.Lerp(OriginalRotation, TargetRotation, AmountRotated);
//                AmountRotated = 0;
//            }

            TargetRotation = Quaternion.Concatenate(
                TargetRotation,
                Quaternion.CreateFromYawPitchRoll(direction.Y, direction.X, direction.Z));
        }

        public void CancelRotation()
        {
            Quaternion temp = OriginalRotation;
            OriginalRotation = TargetRotation;
            TargetRotation = temp;
            AmountRotated = 1 - AmountRotated;
        }

        public bool EntityNameExists(string name)
        {
            return Entities.ContainsKey(name);
        }

        public Entity FindEntity(string name)
        {
            if (!EntityNameExists(name))
                throw new InvalidOperationException("Entity " + name + " doesn't exist");
            return Entities[name];
        }
    }
}