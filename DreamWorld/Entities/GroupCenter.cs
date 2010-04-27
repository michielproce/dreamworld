using DreamWorld.Rendering.Particles.Systems;
using Microsoft.Xna.Framework;

namespace DreamWorld.Entities
{
    public class GroupCenter : Entity
    {
        public static bool ListInEditor = true;
        private GroupCenterParticleSystem particleSystem;
        private int _frames;

        public override void Initialize()
        {
            particleSystem = new GroupCenterParticleSystem(Group.Color);
            Level.AddParticleSystem(Group.groupId + "_groupCenter", particleSystem);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Only load a model if a deriving class has added one
            if (Model != null)
            {
                base.LoadContent();
                return;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (_frames++ % 10 == 0)
                particleSystem.AddParticle(Body.Position, Vector3.Zero);
            base.Update(gameTime);
        }


    }
}
