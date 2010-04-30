using System;
using System.Collections.Generic;
using DreamWorld.Audio;
using DreamWorld.Cameras;
using DreamWorld.Entities;
using DreamWorld.Levels.VillageLevel.Entities;
using DreamWorld.Rendering.Postprocessing;
using DreamWorld.ScreenManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace DreamWorld.Levels.VillageLevel
{
    public class VillageLevel : Level
    {
        public enum Stage
        {
            START,
            FINISHED_TUTORIAL,
            FINISHED_PUZZLE1,
        }

        public Stage CurrentStage { get; private set;}

        public float maximumWalkingHeight { get; private set; }

        public override string LevelInformationFileName
        {
            get { return "Village.xml"; }
        }

        public VillageLevel(Stage stage)
        {
            CurrentStage = stage;
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

            foreach (KeyValuePair<int, Group> group in Groups)
            {
                foreach (KeyValuePair<string, Entity> entity in group.Value.Entities)
                {

                    switch (CurrentStage)
                    {
                        case Stage.START:
                            if (entity.Value is Stable ||
                                entity.Value is CowDummy)
                                toRemove.Add(entity.Value);
                            break;

                        case Stage.FINISHED_TUTORIAL:
                            if (entity.Value is Stable ||
                                entity.Value is CowDummy)
                                toRemove.Add(entity.Value);                                                                                 
                            break;

                        case Stage.FINISHED_PUZZLE1:
                            if (entity.Value is StableTrashed)
                                toRemove.Add(entity.Value);

                            GameScreen.VoiceOver = new VoiceOver(GameScreen.Content.Load<SoundEffect>(@"Audio\Voice\52 done_better"), "Tubbles, you have done even better then I had expected. Just arrived and already solved the problem. You better take a look outside and see what change you made to the village.");
                            break;
                    }
                }
            }

            foreach (Entity entity in toRemove)
                entity.Group.RemoveEntity(entity.Name);
        }
    }
}
