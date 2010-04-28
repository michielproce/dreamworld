using System;
using DreamWorld.Rendering.Particles.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Entities
{
    public class GroupCenter : Entity
    {
        public static bool ListInEditor = true;
        private GroupCenterParticleSystem particleSystem;
        private int frames;
        private bool hideModel;
        

        public override void Initialize()
        {
            particleSystem = new GroupCenterParticleSystem(Group.Color);
            Level.AddParticleSystem(Group.groupId + "_groupCenter", particleSystem);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Only load model if a deriving class has none. This is necessary for the level editor.
            if (Model == null)
            {
                Model = GameScreen.Content.Load<Model>(@"Models\Puzzle\GroupCenter");
                hideModel = true;
            }

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (frames++ % 15 == 0)
                particleSystem.AddParticle(Body.Position, Vector3.Zero);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, string technique)
        {
            if(!hideModel)
                base.Draw(gameTime, technique);
        }
    }
}
