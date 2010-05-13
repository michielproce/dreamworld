using DreamWorld.Entities;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace DreamWorld.Audio
{
    public class SoundEffect3D
    {
        private readonly SoundEffectInstance _sfxInstance;
        private readonly Entity _entity;
        private readonly AudioEmitter _emitter;

        public float Boost { get; set;}

        public SoundEffect3D(Entity entity, SoundEffect sfxInstance) : this (entity, sfxInstance, true)
        {            
        }

        public SoundEffect3D(Entity entity, SoundEffect sfx, bool looped)
        {
            _entity = entity;
            _sfxInstance = sfx.CreateInstance();
            _emitter = new AudioEmitter();
            _sfxInstance.IsLooped = looped;
            Apply3D();
        }

        public void Update(GameTime gameTime)
        {
            if (!_sfxInstance.IsDisposed)
                Apply3D();   
        }

        public void Play()
        {
            if(!_sfxInstance.IsDisposed) 
                _sfxInstance.Play();
        }

        public void Stop()
        {
            if (!_sfxInstance.IsDisposed)
                _sfxInstance.Stop();            
        }

        private void Apply3D()
        {            
            AudioListener listener = GameScreen.Instance.Camera.Listener;
            _emitter.Position = Vector3.Lerp(_entity.Body.Position, listener.Position, Boost);
            _emitter.Forward = Vector3.Transform(Vector3.Forward, _entity.Body.Orientation);
            _sfxInstance.Apply3D(listener, _emitter);
        }
    }
}
