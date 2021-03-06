﻿using System;
using System.Collections.Generic;
using DreamWorld.Levels;
using DreamWorld.ScreenManagement.Screens;
using JigLibX.Collision;
using JigLibX.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Entities
{
    public class Group
    {
        private readonly GameScreen _gameScreen;

        public int GroupId;

        public Dictionary<string, Entity> Entities { get; private set; }
        public GroupCenter Center;
        public Vector3 AllowedRotations;
        private readonly List<CollisionSkin> _ignoredCollisionSkins;

        private float _amountRotated;
        public Color Color;
        private Quaternion OriginalRotation { get; set; }
        private Quaternion TargetRotation { get; set; }

        private Quaternion Rotation
        {
            get
            {
                return IsRotating ? Quaternion.Lerp(OriginalRotation, TargetRotation, _amountRotated) : OriginalRotation;
            }
        }

        public bool IsRotating
        {
            get { return OriginalRotation != TargetRotation; }
        }

        public virtual bool IsColliding
        {
            get
            {
                foreach (Entity entity in Entities.Values)
                {
                    foreach (CollisionInfo collision in entity.Skin.Collisions)
                    {
                        if ((!_ignoredCollisionSkins.Contains(collision.SkinInfo.Skin0) &&
                             collision.SkinInfo.Skin0.Owner.Immovable) ||
                            (!_ignoredCollisionSkins.Contains(collision.SkinInfo.Skin1) &&
                             collision.SkinInfo.Skin1.Owner.Immovable))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public bool IsInRange
        {
            get
            {
                if (!(_gameScreen.Level is PuzzleLevel))
                    return false;
                return Center != null &&
                       Vector3.Distance(Center.Body.Position, _gameScreen.Level.Player.Body.Position) <
                       PuzzleLevel.SelectionRadius;
            }
        }

        private const float RotationSpeed = 0.025f;

        public Group()
        {
            _gameScreen = GameScreen.Instance;
            Entities = new Dictionary<string, Entity>();
            _ignoredCollisionSkins = new List<CollisionSkin>();
            OriginalRotation = Quaternion.Identity;
            TargetRotation = Quaternion.Identity;
            AllowedRotations = Vector3.One;
            Color = Color.White;
        }

        public void Initialize()
        {
            foreach (Entity entity in Entities.Values)
            {
                entity.Initialize();

                if (entity.Skin != null && !_ignoredCollisionSkins.Contains(entity.Skin))
                    _ignoredCollisionSkins.Add(entity.Skin);
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

                _amountRotated += RotationSpeed;

                if (_amountRotated >= 1)
                {
                    OriginalRotation = TargetRotation;
                    _amountRotated = 0;
                }

                Quaternion rotationSinceLastUpdate = Quaternion.Concatenate(Quaternion.Inverse(oldRotation), Rotation);

                if (Center != null && rotationSinceLastUpdate != Quaternion.Identity)
                {
                    foreach (Entity entity in Entities.Values)
                    {
                        RotateBody(entity.Body, rotationSinceLastUpdate, Center.Body.Position, true);

                        // Rotate bodies that collide with this entity and are movable
                        for (int i = 0; i <= entity.Skin.Collisions.Count - 1; i++)
                        {
                            CollisionSkin skin = entity.Skin == entity.Skin.Collisions[i].SkinInfo.Skin0 ? entity.Skin.Collisions[i].SkinInfo.Skin1 : entity.Skin.Collisions[i].SkinInfo.Skin0;

                            if (!_ignoredCollisionSkins.Contains(skin) && !skin.Owner.Immovable)
                                RotateBody(skin.Owner, rotationSinceLastUpdate, Center.Body.Position, false);
                        }
                    }
                }
            }

            foreach (Entity entity in Entities.Values)
            {
                entity.Update(gameTime);
            }
        }

        private static void RotateBody(Body body, Quaternion rotation, Vector3 origin, bool changeOrientation)
        {
            // Move the group's origin to the world's origin
            body.Position -= origin;

            // Rotate the entity (around the world's origin)
            if (changeOrientation)
                body.Orientation *= Matrix.CreateFromQuaternion(rotation);
            body.Position = Vector3.Transform(body.Position, rotation);

            // Move the entity's origin back
            body.Position += origin;
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
                _ignoredCollisionSkins.Add(entity.Skin);
        }

        /// <summary>
        /// This function will be called by Entity
        /// Simply set Entity.Group to null to remove it
        /// </summary>
        public void RemoveEntity(string name)
        {
            if (!Entities.ContainsKey(name))
                throw new InvalidOperationException("Entity " + name + " doesn't exist");
            if (_ignoredCollisionSkins.Contains(Entities[name].Skin))
                _ignoredCollisionSkins.Remove(Entities[name].Skin);
            Entities[name].DisablePhysics();
            Entities.Remove(name);
        }

        /// <summary>
        /// Returns true if this group is allowed to rotate in this direction
        /// </summary>
        protected virtual bool IsRotationAllowed(Vector3 direction)
        {
            return ((direction.X == 0 || AllowedRotations.X != 0) && (direction.Y == 0 || AllowedRotations.Y != 0)) &&
                   (direction.Z == 0 || AllowedRotations.Z != 0);
        }

        public void Rotate(Vector3 direction)
        {
            // Clean up direction
            direction.X = (direction.X < 0.1 && direction.X > -0.1 ? 0 : direction.X);
            direction.Y = (direction.Y < 0.1 && direction.Y > -0.1 ? 0 : direction.Y);
            direction.Z = (direction.Z < 0.1 && direction.Z > -0.1 ? 0 : direction.Z);
            direction = Vector3.Normalize(direction)*MathHelper.PiOver2;

            if (IsRotating || !IsRotationAllowed(direction))
                return;

//            TODO (?): multiple rotations at the same time
//            if (IsRotating)
//            {
//                OriginalRotation = Quaternion.Lerp(OriginalRotation, TargetRotation, AmountRotated);
//                AmountRotated = 0;
//            }

            TargetRotation = Quaternion.Concatenate(
                TargetRotation,
                Quaternion.CreateFromYawPitchRoll(direction.Y, direction.X, direction.Z));
        }

        private void CancelRotation()
        {
            Quaternion temp = OriginalRotation;
            OriginalRotation = TargetRotation;
            TargetRotation = temp;
            _amountRotated = 1 - _amountRotated;
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