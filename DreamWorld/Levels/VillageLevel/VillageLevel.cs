using System;
using System.Collections.Generic;
using DreamWorld.Entities;
using DreamWorld.Levels.VillageLevel.Entities;
using DreamWorld.Rendering.Postprocessing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace DreamWorld.Levels.VillageLevel
{
    public class VillageLevel : Level
    {
        private bool initialTutorialShown;

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

            Song ambient = GameScreen.Content.Load<Song>(@"Audio\Ambient\Village");
            MediaPlayer.Play(ambient);
            MediaPlayer.IsRepeating = true;

            base.Initialize();

            if (LevelsCompleted < 1)
            {
                List<Entity> toRemove = new List<Entity>();
                foreach (KeyValuePair<int, Group> group in Groups)
                    foreach (KeyValuePair<string, Entity> entity in group.Value.Entities)
                        if (entity.Value is Stable || entity.Value is CowDummy)
                            toRemove.Add(entity.Value);
                foreach (Entity entity in toRemove)
                    entity.Group.RemoveEntity(entity.Name);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!initialTutorialShown && GameScreen.TutorialText != null)
            {
                if (LevelsCompleted == 0)
                {
                    GameScreen.TutorialText.SetText(
                        "Welcome to DreamWorld.\nUse the arrow or WSAD keys to move around.\nUse your mouse to look around.\nPress space to jump.\nTry and find someone to talk to in the village.",
                        "Welcome to DreamWorld.\nUse the left joystick to move and the right joystick to look around.\nPress the A button to jump.\nTry and find someone to talk to in the village.");
                }
                else if (LevelsCompleted == 1)
                {
                    GameScreen.TutorialText.SetText(
                    "Congratulations, you have helped the village. There are now cows for milk and food.\nHowever, this is it for now. We have 7 more levels planned, so check floatingkoalagames.com for updates.",
                    "Congratulations, you have helped the village. There are now cows for milk and food.\nHowever, this is it for now. We have 7 more levels planned, so check floatingkoalagames.com for updates.",
                    gameTime.TotalGameTime + TimeSpan.FromSeconds(15));
                }

                initialTutorialShown = true;
            }
            base.Update(gameTime);
        }
    }
}
