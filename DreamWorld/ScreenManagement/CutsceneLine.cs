using System;
using Microsoft.Xna.Framework.Audio;

namespace DreamWorld.ScreenManagement
{
    public class CutsceneLine
    {
        public string Text { get; private set; }
        public SoundEffect Audio { get; private set; }
        public TimeSpan Delay { get; private set; }

        public CutsceneLine(SoundEffect audio, string text) : this(audio, text, TimeSpan.Zero)           
        {
        }

        public CutsceneLine(SoundEffect audio, string text, TimeSpan delay)
        {
            Audio = audio;
            Text = text;
            Delay = delay;
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
