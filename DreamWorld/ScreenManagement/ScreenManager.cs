using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace DreamWorld.ScreenManagement
{
    public class ScreenManager : DrawableGameComponent
    {
        private List<Screen> screens;

        public ScreenManager(Game game) : base(game)
        {
            screens = new List<Screen>();
        }

        public void AddScreen(Screen screen)
        {
            screen.ScreenManager = this;
            screens.Add(screen);
        }

        public void RemoveScreen(Screen screen)
        {
            screens.Remove(screen);            
        }

        public override void Initialize()
        {
            foreach (Screen screen in screens)
                screen.Initialize();
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Screen screen in screens)
                screen.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Screen screen in screens)
                screen.Draw(gameTime);
            base.Update(gameTime);
        }
    }
}
