using System;
using DreamWorld.Entities;
using DreamWorld.ScreenManagement.Screens.Cutscenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Levels.VillageLevel.Entities
{
    public class Morwir : Entity
    {
        public static bool ListInEditor = true;
        private bool hintVisible;

        public override void Initialize()
        {
            Animation.InitialClip = "Standing";
            Animation.Speed = 1.0f;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Model = GameScreen.Content.Load<Model>(@"Models\Village\Morwir");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            float dist = Vector3.Distance(Level.Player.Body.Position, Body.Position);
            if (dist < 50f)
            {
                GameScreen.TutorialText.SetText("Press the B button to interact with morwir");
                if(GameScreen.InputManager.Player.Interact)
                {                    
                    GameScreen.ExitScreen();
                    GameScreen.ScreenManager.AddScreen(new MorwirCutscene());
                }
                hintVisible = true;
            }
            else if(hintVisible)
            {
                GameScreen.TutorialText.Hide();    
            }
            base.Update(gameTime);
        }
    }
}
