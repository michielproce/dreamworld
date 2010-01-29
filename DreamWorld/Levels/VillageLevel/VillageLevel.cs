using System;
using DreamWorld.Entities;
using DreamWorld.Rendering.Postprocessing;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace DreamWorld.Levels.VillageLevel
{
    public class VillageLevel : Level
    {
        private bool initialTutorialShown;

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
        }

        public override void Update(GameTime gameTime)
        {
            if (!initialTutorialShown && GameScreen.TutorialText != null)
            {
                GameScreen.TutorialText.SetText(
                    "Welcome to DreamWorld.\nUse the arrow or WSAD keys to move around.\nUse your mouse to look around.\nPress space to jump.\nTry and find someone to talk to in the village.",
                    "Welcome to DreamWorld.\nUse the left joystick to move and the right joystick to look around.\nPress the A button to jump.\nTry and find someone to talk to in the village.");
                initialTutorialShown = true;
            }
            base.Update(gameTime);
        }
    }
}
