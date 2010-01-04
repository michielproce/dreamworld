using System;
using DreamWorld.Entities;
using DreamWorld.InputManagement.Types;
using Microsoft.Xna.Framework;

namespace DreamWorld.Levels
{
    public abstract class PuzzleLevel : Level
    {        
        private int SelectedGroup;

        public override void Initialize()
        {
            base.Initialize();
            HandleGroupSelection(1);
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
            if(selection == 0)
                return;

            int[] keys = new int[Groups.Count];
            Groups.Keys.CopyTo(keys, 0);

            for (int tries = 0; tries < Groups.Count; tries++ )
            {   // Keep looping untill we have found a group that is allowed to rotate
                SelectedGroup += selection;
                if (SelectedGroup >= keys.Length)
                {
                    SelectedGroup -= keys.Length;
                }
                else if (SelectedGroup < 0)
                {
                    SelectedGroup += keys.Length;
                }

                if (!Groups[keys[SelectedGroup]].AllowedRotations.Equals(Vector3.Zero))
                {
                    break;   
                }
            }
        }

        private void HandleGroupRotation(Vector3 direction)
        {
            if (direction != Vector3.Zero)
            {
                int[] keys = new int[Groups.Count];
                Groups.Keys.CopyTo(keys, 0);
                Group targetGroup = Groups[keys[SelectedGroup]];

                if (direction.X == 0 && direction.Z == 0)
                {
                    targetGroup.Rotate(direction);
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

                    targetGroup.Rotate(Vector3.Transform(direction, Matrix.CreateRotationY(rotation)));
                }
            }
        }
    }
}