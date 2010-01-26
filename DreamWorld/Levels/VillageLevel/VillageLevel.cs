using System;
using DreamWorld.Entities;
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
                    "Welcome to DreamWorld. Use the left joystick to move, the right joystick to look around, A button to jump and B button to interact.",
                    gameTime.TotalGameTime + TimeSpan.FromSeconds(10));
                initialTutorialShown = true;
            }
            base.Update(gameTime);
        }
    }
}
