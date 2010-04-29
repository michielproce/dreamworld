using System;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Rendering.Particles.Systems
{
    class GroupCenterParticleSystem : ParticleSystem
    {
        private Color _color;

        public GroupCenterParticleSystem(Color color)
        {
            _color = color;
        }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "Flare";

            settings.MaxParticles = 20;

            settings.Duration = TimeSpan.FromSeconds(1.5f);

            settings.MinHorizontalVelocity = -1;
            settings.MaxHorizontalVelocity = 1;

            settings.MinVerticalVelocity = -1;
            settings.MaxVerticalVelocity = 2;

            settings.EndVelocity = .75f;

            settings.MinRotateSpeed = -1;
            settings.MaxRotateSpeed = 1;

            settings.MinStartSize = 1;
            settings.MaxStartSize = 3;

            settings.MinEndSize = 12;
            settings.MaxEndSize = 16;

            settings.MaxColor = _color;
            settings.MinColor = _color;
        }
    }
}
