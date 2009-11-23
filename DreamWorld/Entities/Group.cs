using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace DreamWorld.Entities
{
    class Group : Entity
    {
        private List<Element> Elements { get; set; }

        private Quaternion OriginalRotation { get; set; }
        private Quaternion TargetRotation { get; set; }
        private float AmountRotated;

        private const float RotationSpeed = 0.05f;

        public bool IsRotating
        {
            get
            {
                return OriginalRotation != TargetRotation;
            }
        }

        public Group()
        {
            Elements = new List<Element>();
            OriginalRotation = Quaternion.Identity;
            TargetRotation = Quaternion.Identity;
        }

        public override void Initialize()
        {
            foreach (Element element in Elements)
            {
                element.Initialize();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (IsRotating)
            {
                AmountRotated += RotationSpeed;

                if (AmountRotated >= 1)
                {
                    OriginalRotation = TargetRotation;
                    AmountRotated = 0;
                }
            }

            foreach (Element element in Elements)
            {
                element.Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override Matrix GenerateWorldMatrix()
        {
            Matrix rotation;
            if (IsRotating)
                rotation = Matrix.CreateFromQuaternion(Quaternion.Lerp(OriginalRotation, TargetRotation, AmountRotated));
            else
                rotation = Matrix.CreateFromQuaternion(OriginalRotation);

            return rotation * Matrix.CreateTranslation(Position) * Matrix.CreateScale(Scale);
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
        }

        public void Rotate(Vector3 direction)
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