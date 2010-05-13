using DreamWorld.Levels.PuzzleLevel1.Entities;
using DreamWorld.Rendering.Particles.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Entities
{
    public class GroupCenter : Entity
    {
        public static bool ListInEditor = true;
        private GroupCenterParticleSystem _particleSystem;
        private int _frames;
        private bool _hideModel;
        

        public override void Initialize()
        {
            _particleSystem = new GroupCenterParticleSystem(Group.Color);
            Level.AddParticleSystem(Group.GroupId + "_groupCenter", _particleSystem);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Only load model if a deriving class has none. This is necessary for the level editor.
            if (Model == null)
            {
                Model = GameScreen.Content.Load<Model>(@"Models\Puzzle\GroupCenter");
                _hideModel = true;
            }

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            int framesPerParticle = this is Cow && Group.IsRotating ? 5 : 15;

            if (_frames++ % framesPerParticle == 0)
                _particleSystem.AddParticle(Body.Position, Vector3.Zero);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, string technique)
        {
            if(!_hideModel)
                base.Draw(gameTime, technique);
        }
    }
}
