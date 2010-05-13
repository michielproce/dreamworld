using System.Collections.Generic;
using DreamWorld.Audio;
using DreamWorld.Cameras;
using DreamWorld.Entities;
using DreamWorld.Levels.VillageLevel.Entities;
using DreamWorld.Rendering.Postprocessing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace DreamWorld.Levels.VillageLevel
{
    public class VillageLevel : Level
    {
        public enum Stage
        {
            Start,
            FinishedTutorial,
            FinishedPuzzle1,
        }

        public Stage CurrentStage { get; private set;}

        public float MaximumWalkingHeight { get; private set; }

        public override string LevelInformationFileName
        {
            get { return "Village.xml"; }
        }

        public VillageLevel(Stage stage)
        {
            CurrentStage = stage;
        }

        protected override void InitBloom(ref Bloom bloom)
        {
            switch (CurrentStage)
            {
                case Stage.Start:
                case Stage.FinishedTutorial:
                    bloom.BloomThreshold = .3f;
                    bloom.BlurAmount = 4f;
                    bloom.BloomIntensity = 1f;
                    bloom.BaseIntensity = 1f;
                    bloom.BloomSaturation = .85f;
                    bloom.BaseSaturation = .85f;
                    break;

                case Stage.FinishedPuzzle1:
                    bloom.BloomThreshold = .3f;
                    bloom.BlurAmount = 4f;
                    bloom.BloomIntensity = 1f;
                    bloom.BaseIntensity = 1f;
                    bloom.BloomSaturation = 1.15f;
                    bloom.BaseSaturation = 1.15f;
                    break;
            }
        }

        public override void Initialize()
        {
            MaximumWalkingHeight = -525;
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
                        case Stage.Start:
                            if (entity.Value is Stable ||
                                entity.Value is CowDummy ||
                                "villageSignAfterTutorial".Equals(entity.Key) ||
                                "villageTubesAfterTutorial".Equals(entity.Key) ||
                                "villageSignAfterPuzzle1".Equals(entity.Key) ||
                                "villageTubesAfterPuzzle1".Equals(entity.Key))
                                toRemove.Add(entity.Value);
                            break;

                        case Stage.FinishedTutorial:
                            if (entity.Value is Stable ||
                                entity.Value is CowDummy || 
                                "villageSignAfterPuzzle1".Equals(entity.Key) ||
                                "villageTubesAfterPuzzle1".Equals(entity.Key))
                                toRemove.Add(entity.Value);                                                                                 
                            break;

                        case Stage.FinishedPuzzle1:
                            if (entity.Value is StableTrashed || 
                                "villageSign3".Equals(entity.Key) || 
                                "villageTubes3".Equals(entity.Key) || 
                                "villageSignAfterTutorial".Equals(entity.Key) ||
                                "villageTubesAfterTutorial".Equals(entity.Key))                              
                                toRemove.Add(entity.Value);

                            GameScreen.VoiceOver = new VoiceOver(GameScreen.Content.Load<SoundEffect>(@"Audio\Voice\52 done_better"), "Tubbles, you have done even better then I had expected. Just arrived and already solved the problem. You better take a look outside and see what change you made to the village.", .3f);
                            break;
                    }
                }
            }

            foreach (Entity entity in toRemove)
                entity.Group.RemoveEntity(entity.Name);

            // Move the player to it's location
            switch (CurrentStage)
            {
                case Stage.FinishedTutorial:
                    Player.Body.MoveTo(new Vector3(300, -550, 90), Matrix.Identity);
                    break;

                case Stage.FinishedPuzzle1:
                    Player.Body.MoveTo(new Vector3(250, -550, 275), Matrix.CreateRotationY(MathHelper.ToRadians(130)));
                    break;
            }

        }
    }
}
