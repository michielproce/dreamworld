using System;
using DreamWorld.Entities;
using DreamWorld.InputManagement.Types;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;

namespace DreamWorld.Levels
{
    public abstract class PuzzleLevel : Level
    {
        protected internal float selectionRadius = 110;
        private int selectedGroup;

        public Group GetSelectedGroup()
        {
            int[] keys = new int[Groups.Count];
            Groups.Keys.CopyTo(keys, 0);
            Group group = Groups[keys[selectedGroup]];

            if (!group.AllowedRotations.Equals(Vector3.Zero) && group.IsInRange)
                return group;

            return null;
        }

        public override void Update(GameTime gameTime)
        {
            PlayerInput input = Game.InputManager.Player;

            HandleGroupSelection(input.SelectGroup);
            HandleGroupRotation(input.RotateGroup);

            if(GameIsLost())
                LossEventHandler();

            if(GameIsWon())
                VictoryEventHandler();

            base.Update(gameTime);
        }

        private void HandleGroupSelection(int selection)
        {
            if(selection == 0)
                return;

            int[] keys = new int[Groups.Count];
            Groups.Keys.CopyTo(keys, 0);

            for (int tries = 0; tries < Groups.Count; tries++ )
            {
                // Keep looping untill we have found a group that is allowed to rotate
                selectedGroup += selection;
                if (selectedGroup >= keys.Length)
                {
                    selectedGroup -= keys.Length;
                }
                else if (selectedGroup < 0)
                {
                    selectedGroup += keys.Length;
                }

                if (!Groups[keys[selectedGroup]].AllowedRotations.Equals(Vector3.Zero) && Groups[keys[selectedGroup]].IsInRange)
                    break;
            }
        }

        private void HandleGroupRotation(Vector3 direction)
        {
            Group targetGroup = GetSelectedGroup();

            if (direction != Vector3.Zero && targetGroup != null)
            {
                if (direction.X == 0 && direction.Z == 0)
                {
                    direction.Normalize();
                    targetGroup.Rotate(direction * MathHelper.PiOver2);
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

        protected virtual bool GameIsWon()
        {
            return false;
        }

        protected virtual void VictoryEventHandler()
        {
            if(!GameScreen.IsExiting)
            {
                GameScreen.ScreenManager.AddScreen(new MainMenuScreen());
                GameScreen.ExitScreen();
            }
        }

        protected virtual bool GameIsLost()
        {
            return false;
        }

        protected virtual void LossEventHandler()
        {
            Player.Respawn();
        }
    }
}