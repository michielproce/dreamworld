using System;
using System.Collections.Generic;
using JigLibX.Collision;
using Microsoft.Xna.Framework;

namespace DreamWorld.Entities
{
    public class Group : Entity
    {
        private List<Element> Elements { get; set; }
        internal List<CollisionSkin> ignoreCollisionSkins;

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

        public Quaternion RotationSinceLastUpdate { get; private set; }

        internal const float RotationSpeed = 0.05f;


        public Group()
        {
            Elements = new List<Element>();
            ignoreCollisionSkins = new List<CollisionSkin>();
            OriginalRotation = Quaternion.Identity;
            TargetRotation = Quaternion.Identity;
        }

        public override void Initialize()
        {

            foreach (Element element in Elements)
            {
                element.Initialize();

                if (element.Skin != null)
                    ignoreCollisionSkins.Add(element.Skin);
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {

        }

        public override void Update(GameTime gameTime)
        {
            Quaternion oldRotation = Rotation;

            if (IsRotating)
            {
                if (isColliding())
                {
                    Quaternion temp = OriginalRotation;
                    OriginalRotation = TargetRotation;
                    TargetRotation = temp;
                    AmountRotated = 1 - AmountRotated;
                }

                AmountRotated += RotationSpeed;

                if (AmountRotated >= 1)
                {
                    OriginalRotation = TargetRotation;
                    AmountRotated = 0;
                }
            }

            RotationSinceLastUpdate = Quaternion.Concatenate(Quaternion.Inverse(oldRotation), Rotation);

            foreach (Element element in Elements)
            {
                element.Update(gameTime);
            }

            base.Update(gameTime);
        }

        private bool isColliding()
        {
            foreach (Element element in Elements)
            {
                for (int i = 0; i <= element.Skin.Collisions.Count - 1; i++)
                {
                    if (!ignoreCollisionSkins.Contains(element.Skin.Collisions[i].SkinInfo.Skin0) || !ignoreCollisionSkins.Contains(element.Skin.Collisions[i].SkinInfo.Skin1))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override void Draw(GameTime gameTime, string technique)
        {
            foreach (Element element in Elements)
            {
                if(technique == "EdgeDetection")
                {
                    element.Draw(gameTime, !element.IgnoreEdgeDetection ? "NormalDepth" : "IgnoreNormalDepth");
                } 
                else
                {
                    element.Draw(gameTime, technique);
                }
            }
        }

        public void AddElement(Element element)
        {
            element.Group = this;
            Elements.Add(element);
            if (initialized && element.Skin != null)
                ignoreCollisionSkins.Add(element.Skin);
        }

        public void Rotate(Vector3 direction)
        {
            if(!IsRotating)
            {
                direction = Vector3.Normalize(direction) * MathHelper.PiOver2;

                if (IsRotating)
                {
                    OriginalRotation = Quaternion.Lerp(OriginalRotation, TargetRotation, AmountRotated);
                    AmountRotated = 0;
                }

                TargetRotation = Quaternion.Concatenate(
                    TargetRotation,
                    Quaternion.CreateFromYawPitchRoll(direction.Y, direction.X, direction.Z));
            }
        }
    }
}