using DreamWorld.Entities;
using DreamWorld.ScreenManagement.Screens;
using JigLibX.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace DreamWorld.Audio
{
    public class SoundEffect3D
    {
        private SoundEffectInstance sfxInstance;
        private Entity entity;
        private AudioEmitter emitter;

        public float Boost { get; set;}

        public SoundEffect3D(Entity entity, SoundEffect sfxInstance) : this (entity, sfxInstance, true)
        {            
        }

        public SoundEffect3D(Entity entity, SoundEffect sfx, bool looped)
        {
            this.entity = entity;
            sfxInstance = sfx.CreateInstance();
            emitter = new AudioEmitter();
            sfxInstance.IsLooped = looped;
            Apply3D();
        }

        public void Update(GameTime gameTime)
        {
            if (!sfxInstance.IsDisposed)
                Apply3D();   
        }

        public void Play()
        {
            if(!sfxInstance.IsDisposed) 
                sfxInstance.Play();
        }

        public void Stop()
        {
            if (!sfxInstance.IsDisposed)
                sfxInstance.Stop();            
        }

        private void Apply3D()
        {            
            AudioListener listener = GameScreen.Instance.Camera.Listener;
            emitter.Position = Vector3.Lerp(entity.Body.Position, listener.Position, Boost);
            emitter.Forward = Vector3.Transform(Vector3.Forward, entity.Body.Orientation);
            sfxInstance.Apply3D(listener, emitter);
        }
    }
}
