using System;
using System.Collections.Generic;
using DreamWorld.Cameras;
using DreamWorld.Entities;
using DreamWorld.InputManagement.Types;
using DreamWorld.ScreenManagement.Screens;
using DreamWorld.Util;
using JigLibX.Collision;
using JigLibX.Geometry;
using JigLibX.Physics;
using Microsoft.Xna.Framework;

namespace DreamWorld.Levels
{
    public abstract class PuzzleLevel : Level
    {
        protected internal float selectionRadius = 110;
        private int selectedGroup;

        protected PuzzleHUD hud;

        public override void Initialize()
        {            
            base.Initialize();
            hud = new PuzzleHUD(GameScreen.GraphicsDevice);
            hud.Load(GameScreen.Content);
        }

        public Group GetSelectedGroup()
        {
            int[] keys = new int[Groups.Count];
            Groups.Keys.CopyTo(keys, 0);
            Group group = Groups[keys[selectedGroup]];

            if (!group.AllowedRotations.Equals(Vector3.Zero) && group.IsInRange)
                return group;

            return null;
        }

        public void SetSelectedGroup(Group group)
        {
            int[] keys = new int[Groups.Count];
            Groups.Keys.CopyTo(keys, 0);
            foreach (int i in keys)
            {
                if (Groups[keys[i]] == group)
                {
                    selectedGroup = i;
                    return;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            PlayerInput input = Game.InputManager.Player;           
            
            hud.Cycle(input.CycleAxle);

            if(input.ApplyRotation)
                RotateGroup(hud.CurrentDirection);

            hud.Update(gameTime);

            SelectGroup();

            if(GameIsWon())
                VictoryEventHandler();

            base.Update(gameTime);
        }

        private void SelectGroup()
        {
            if (!(GameScreen.Camera is ThirdPersonCamera))
                return;

            ThirdPersonCamera camera = (ThirdPersonCamera)GameScreen.Camera;
            Vector3 direction = camera.Direction;
            direction.Normalize();
            direction *= selectionRadius*2;

            float dist;
            CollisionSkin skin;
            Vector3 pos, normal;

            CollisionSkinPredicate1 pred = new IgnoreSkinPredicate(Player.Skin);
            Segment seg = new Segment(camera.Position, direction);

            DreamWorldPhysicsSystem.CurrentPhysicsSystem.CollisionSystem.SegmentIntersect(out dist, out skin, out pos, out normal, seg, pred);

            if (skin == null) return;

            foreach (KeyValuePair<int, Group> group in Groups)
            {
                foreach (KeyValuePair<string, Entity> entity in group.Value.Entities)
                {
                    if (entity.Value.Skin != skin) continue;
                    if (!group.Value.AllowedRotations.Equals(Vector3.Zero) && group.Value.IsInRange)
                        SetSelectedGroup(group.Value);
                    return;
                }
            }
        }

        private void RotateGroup(Vector3 direction)
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

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            hud.Draw(gameTime);
        }
    }
}