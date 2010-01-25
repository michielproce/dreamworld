using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement
{
    public class CutsceneLine
    {
        public string Text { get; private set; }
        public SoundEffect Audio { get; private set; }
        public TimeSpan Delay { get; private set; }
        
        public Texture2D Texture { get; set; }

        
        public CutsceneLine(SoundEffect audio, string text) 
            : this(audio, text, TimeSpan.Zero, null)           
        {
        }

        public CutsceneLine(SoundEffect audio, string text, TimeSpan delay)
            : this(audio, text, delay, null)
        {
        }

        public CutsceneLine(SoundEffect audio, string text, Texture2D texture)
            : this(audio, text, TimeSpan.Zero, texture)
        {
        }

        public CutsceneLine(SoundEffect audio, string text, TimeSpan delay, Texture2D texture)
        {
            Audio = audio;
            Text = text;
            Delay = delay;
            Texture = texture;
        }

        public TimeSpan Duration
        {
            get
            {
                return Audio.Duration + Delay;
            }
        }
    }
}
