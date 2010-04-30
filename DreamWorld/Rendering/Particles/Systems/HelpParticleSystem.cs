using System;

namespace DreamWorld.Rendering.Particles.Systems
{
    class HelpParticleSystem : ParticleSystem
    {
        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "HelpSparkle";

            settings.MaxParticles = 30;

            settings.Duration = TimeSpan.FromSeconds(2.5f);

            settings.MinHorizontalVelocity = -1;
            settings.MaxHorizontalVelocity = 1;

            settings.MinVerticalVelocity = 1;
            settings.MaxVerticalVelocity = 2;

            settings.EndVelocity = .5f;

            settings.MinRotateSpeed = -1;
            settings.MaxRotateSpeed = 1;

            settings.MinStartSize = .2f;
            settings.MaxStartSize = .5f;

            settings.MinEndSize = 1.5f;
            settings.MaxEndSize = 1.9f;
        }
    }
}
