using System;

namespace DreamWorld.Rendering.Particles.Systems
{
    public class HouseSmokeParticleSystem : ParticleSystem
    {
        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "BlackSmoke";

            settings.MaxParticles = 150;

            settings.Duration = TimeSpan.FromSeconds(4);

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 2;

            settings.MinVerticalVelocity = 0;
            settings.MaxVerticalVelocity = 20;

            settings.EndVelocity = 0.005f;

            settings.MinRotateSpeed = -1;
            settings.MaxRotateSpeed = 1;

            settings.MinStartSize = 5;
            settings.MaxStartSize = 10;

            settings.MinEndSize = 50;
            settings.MaxEndSize = 80;
        }
    }
}
