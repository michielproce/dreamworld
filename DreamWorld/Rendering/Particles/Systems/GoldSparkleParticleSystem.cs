using System;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Rendering.Particles.Systems
{
    class GoldSparkleParticleSystem : ParticleSystem
    {
        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "GoldSparkle";

            settings.MaxParticles = 5;

            settings.Duration = TimeSpan.FromSeconds(1f);

            settings.MinHorizontalVelocity = -2;
            settings.MaxHorizontalVelocity = 2;

            settings.MinVerticalVelocity = -2;
            settings.MaxVerticalVelocity = 2;

            settings.EndVelocity = 1f;

            settings.MinRotateSpeed = -1;
            settings.MaxRotateSpeed = 1;

            settings.MinStartSize = .1f;
            settings.MaxStartSize = .5f;

            settings.MinEndSize = 6;
            settings.MaxEndSize = 7;
        }
    }
}
