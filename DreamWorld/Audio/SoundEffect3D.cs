using System;
using DreamWorld.Entities;
using DreamWorld.Helpers.Renderers;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace DreamWorld.Audio
{
    public class SoundEffect3D
    {
        private string soundEffect;                

        public AudioEmitter Emitter { get; private set; }
        public SoundEffectInstance Sound { get; private set; }

        public float DistanceFactor { get; set; }

        public SoundEffect3D(string soundEffect)
        {
            this.soundEffect = soundEffect;
            DistanceFactor = 10;
        }


        public void Initialize()
        {
            Emitter = new AudioEmitter();
            LoadContent();
        }


        protected void LoadContent()
        {
            Sound = GameScreen.Instance.Content.Load<SoundEffect>(@"Audio\Effects\" + soundEffect).CreateInstance();            
            Apply3D();
            Sound.IsLooped = true;
            Sound.Play();
        }        

        public void Update(GameTime gameTime)
        {            
            Apply3D();    
        }

        private void Apply3D()
        {
            AudioListener listener = GameScreen.Instance.Camera.Listener;

            Emitter.Position = Vector3.Normalize(Emitter.Position) * DistanceFactor;
            listener.Position = Vector3.Normalize(listener.Position) * DistanceFactor;

            Sound.Apply3D(listener, Emitter);          
        }
    }
}
