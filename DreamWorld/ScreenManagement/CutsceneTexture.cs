using System;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement
{
    public class CutsceneTexture
    {
        public Texture2D Texture { get; private set; }
        public TimeSpan Duration { get; private set; }

        public CutsceneTexture(Texture2D texture, TimeSpan duration) 
        {
            Texture = texture;
            Duration = duration;
        }
    }
}
