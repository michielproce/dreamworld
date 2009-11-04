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
    }
}
