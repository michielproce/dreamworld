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
        protected internal float SelectionRadius = 110;

        private int _selectedGroup;
        protected PuzzleHUD _hud;

        public override void Initialize()
        {            
            base.Initialize();
            _hud = new PuzzleHUD(GameScreen);
            _hud.Load(GameScreen.Content);
        }

        public Group GetSelectedGroup()
        {
            int[] keys = new int[Groups.Count];
            Groups.Keys.CopyTo(keys, 0);
            Group group = Groups[keys[_selectedGroup]];

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
                    _selectedGroup = i;
                    return;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            PlayerInput input = Game.InputManager.Player;           
            
            _hud.Cycle(input.CycleAxle);

            if(input.ApplyRotation)
                RotateGroup(_hud.CurrentDirection);

            _hud.Update(gameTime);

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
            direction *= SelectionRadius*2;

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
            if (direction == Vector3.Zero) return;

            Group targetGroup = GetSelectedGroup();
            if (targetGroup == null) return;

            targetGroup.Rotate(direction);
            return;
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
            _hud.Draw(gameTime);
        }
    }
}