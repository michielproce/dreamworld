using Microsoft.Xna.Framework.Audio;

namespace DreamWorld.Audio
{
    public class VoiceOver
    {
        public bool Started { get; set; }

        public SoundEffectInstance Audio { get; private set; }
        public string Text { get; private set; }

        public VoiceOver(SoundEffect audio, string text)
        {
            Audio = audio.CreateInstance();
            Text = text;
        }
    }
}
