using System;
using DreamWorld.Entities;
using DreamWorld.InputManagement.Types;
using Microsoft.Xna.Framework;

namespace DreamWorld.Levels
{
    public abstract class PuzzleLevel : Level
    {        
        private int SelectedGroup;
        protected Group[] Groups;

        public override void Initialize()
        {
            for (int i = 0; i < Groups.Length; i++)
            {
                AddEntity("Group"+i, Groups[i]);
            }
            base.Initialize();

            for (int i = 0; i < Groups.Length; i++)
            {
                Groups[i].ignoreCollisionSkins.Add(GameScreen.Level.Player.Skin);
            }
        }

        public override void Update(GameTime gameTime)
        {
            PlayerInput input = Game.InputManager.Player;

            HandleGroupSelection(input.SelectGroup);
            HandleGroupRotation(input.RotateGroup);

            base.Update(gameTime);
        }

        private void HandleGroupSelection(int selection)
        {
            SelectedGroup += selection;
            if (SelectedGroup >= Groups.Length)
            {
                SelectedGroup -= Groups.Length;
            }
            else if (SelectedGroup < 0)
            {
                SelectedGroup += Groups.Length;
            }
        }

        private void HandleGroupRotation(Vector3 direction)
        {
            if (direction != Vector3.Zero)
            {
                if (direction.X == 0 && direction.Z == 0)
                {
                    Groups[0].Rotate(direction);
                }
                else
                {
                    Vector3 cameraDirection = GameScreen.Camera.Direction;
                    float angle = MathHelper.ToDegrees((float) Math.Atan2(cameraDirection.X, cameraDirection.Z));
                    float rotation = 0;

                    if (angle > 45 && angle < 135)
                    {
                        rotation = -MathHelper.PiOver2;
                    }
                    else if (angle > -135 && angle < -45)
                    {
                        rotation = MathHelper.PiOver2;
                    }
                    else if (angle > -45 && angle < 45)
                    {
                        rotation = MathHelper.Pi;
                    }

                    Groups[SelectedGroup].Rotate(Vector3.Transform(direction, Matrix.CreateRotationY(rotation)));
                }
            }
        }
    }
}