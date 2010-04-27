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
            settings.TextureName = "BlackSmoke";

            settings.MaxParticles = 250;

            settings.Duration = TimeSpan.FromSeconds(2);

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 10;

            settings.MinVerticalVelocity = 0;
            settings.MaxVerticalVelocity = 50;

            settings.EndVelocity = 0.25f;

            settings.MinRotateSpeed = -1;
            settings.MaxRotateSpeed = 1;

            settings.MinStartSize = 10;
            settings.MaxStartSize = 20;

            settings.MinEndSize = 50;
            settings.MaxEndSize = 100;

            settings.MaxColor = _color;
            settings.MinColor = _color;
        }
    }
}
