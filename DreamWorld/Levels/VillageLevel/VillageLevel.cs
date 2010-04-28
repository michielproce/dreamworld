using System;
using System.Collections.Generic;
using DreamWorld.Cameras;
using DreamWorld.Entities;
using DreamWorld.Levels.VillageLevel.Entities;
using DreamWorld.Rendering.Postprocessing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace DreamWorld.Levels.VillageLevel
{
    public class VillageLevel : Level
    {
        public int LevelsCompleted;

        public float maximumWalkingHeight { get; private set; }

        public override string LevelInformationFileName
        {
            get { return "Village.xml"; }
        }

        public override void InitBloom(ref Bloom bloom)
        {
            bloom.BloomThreshold = .3f;
            bloom.BlurAmount = 4f;
            bloom.BloomIntensity = 1f;
            bloom.BaseIntensity = 1f;
            bloom.BloomSaturation = 1f;
            bloom.BaseSaturation = 1f;
        }

        public override void Initialize()
        {
            maximumWalkingHeight = -525;
            Skybox = new Skybox("Village") { Name = "Skybox" };
            Terrain = new Terrain("Village") { Name = "Terrain" };
            
            MediaPlayer.Play(GameScreen.Content.Load<Song>(@"Audio\Music\Village"));

            base.Initialize();

            ThirdPersonCamera cam = GameScreen.Camera as ThirdPersonCamera;
            if (cam != null)
                cam.VerticalRotation = MathHelper.ToRadians(5);     

            List<Entity> toRemove = new List<Entity>();

            if (LevelsCompleted < 1)
            {
                foreach (KeyValuePair<int, Group> group in Groups)
                    foreach (KeyValuePair<string, Entity> entity in group.Value.Entities)
                        if (entity.Value is Stable || entity.Value is CowDummy)
                            toRemove.Add(entity.Value);
            } 
            else
            {
                foreach (KeyValuePair<int, Group> group in Groups)
                    foreach (KeyValuePair<string, Entity> entity in group.Value.Entities)
                        if (entity.Value is StableTrashed)
                            toRemove.Add(entity.Value);
            }

            foreach (Entity entity in toRemove)
                entity.Group.RemoveEntity(entity.Name);
        }
    }
}
