using Microsoft.Xna.Framework.Audio;

namespace DreamWorld.Audio
{
    public class VoiceOver
    {
        public bool Started { get; set; }

        public SoundEffectInstance Audio { get; private set; }
        public string Text { get; private set; }

        public VoiceOver(SoundEffect audio, string text) : this(audio, text, 1.0f)
        {            
        }

        public VoiceOver(SoundEffect audio, string text, float volume)
        {
            Audio = audio.CreateInstance();
            Audio.Volume = volume;
            Text = text;
        }
    }
}
