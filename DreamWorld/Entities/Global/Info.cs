using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Entities.Global
{
    // TODO: change the classname to fit the model's name
    class Info : Entity
    {
        public static bool ListInEditor = true;
        private bool hintVisible;

        private float range = 50f;
        public string pcText = "";
        public string xboxText = "";

        protected override void LoadContent()
        {
            Model = GameScreen.Content.Load<Model>(@"Models\Test\Test");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            float dist = Vector3.Distance(Level.Player.Body.Position, Body.Position);
            if (dist < range)
            {
                GameScreen.TutorialText.SetText(pcText, xboxText);
                hintVisible = true;
            }
            else if (hintVisible)
            {
                GameScreen.TutorialText.Hide();
                hintVisible = false;
            }
            base.Update(gameTime);
        }
    }
}
