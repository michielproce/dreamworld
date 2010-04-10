using System;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement
{
    public class CutsceneLine
    {
        public string Text { get; private set; }
        public string Audio { get; private set; }
        public TimeSpan Delay { get; private set; }
        
        public Texture2D Texture { get; set; }

        
        public CutsceneLine(string audio, string text) 
            : this(audio, text, TimeSpan.Zero, null)           
        {
        }

        public CutsceneLine(string audio, string text, TimeSpan delay)
            : this(audio, text, delay, null)
        {
        }

        public CutsceneLine(string audio, string text, Texture2D texture)
            : this(audio, text, TimeSpan.Zero, texture)
        {
        }

        public CutsceneLine(string audio, string text, TimeSpan delay, Texture2D texture)
        {
            Audio = audio;
            Text = text;
            Delay = delay;
            Texture = texture;
        }        
    }
}
