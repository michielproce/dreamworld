using System;
using System.Collections.Generic;
using DreamWorldBase;
using Microsoft.Xna.Framework;

namespace DreamWorld.Entities
{
    public class Animation
    {
        private SkinningData _skinningData;
        private AnimationPlayer _animationPlayer;
        private Matrix[] _boneTransforms;

        public bool Loaded { get; private set; }
        public string InitialClip { get; set; }
        public float Speed { get; set; }
        public bool Paused { get; set; }
        public string CurrentClip { get; private set; }
        public TimeSpan CurrentTimeSpan { get; private set; }

        private readonly Dictionary<string, Matrix> _additionalBoneTransforms;
        
        public Animation()
        {       
            _additionalBoneTransforms = new Dictionary<string, Matrix>();
            Speed = 1;
        }

        public void Load(SkinningData skinningData)
        {
            _skinningData = skinningData;            
            
            _animationPlayer = new AnimationPlayer(skinningData);
            _boneTransforms = new Matrix[skinningData.BindPose.Count];

            if (InitialClip != null)
                StartClip(InitialClip);
            
            if(Paused)
                AdvanceAnimation(new TimeSpan(0));
            
            
            Loaded = true;
        }

        public void StartClip(string clip)
        {
            if (!clip.Equals(CurrentClip))
            {
                _animationPlayer.StartClip(_skinningData.AnimationClips[clip]);
                CurrentClip = clip;
            }
        }

        public void ResetClip()
        {
            AdvanceAnimation(TimeSpan.Zero);
        }

        public Matrix[] SkinTransforms
        {
            get
            {
                return _animationPlayer.GetSkinTransforms();
            }
        }

        public void Update(GameTime gameTime)
        {            
            if(!Loaded || Paused)
                return;

            TimeSpan ts = gameTime.ElapsedGameTime;
            ts = ts.Add(new TimeSpan((long) (ts.Ticks * Speed) - ts.Ticks));                        

            AdvanceAnimation(ts);
        }

        public void SetAdditionalBoneTransform(string bone, Matrix transform)
        {
            if (_additionalBoneTransforms.ContainsKey(bone))
                _additionalBoneTransforms[bone] = transform;
            else
                _additionalBoneTransforms.Add(bone, transform);
        }

        private void AdvanceAnimation(TimeSpan ts)
        {            
            _animationPlayer.UpdateBoneTransforms(ts, true);
            _animationPlayer.GetBoneTransforms().CopyTo(_boneTransforms, 0);
            foreach (KeyValuePair<string, Matrix> pair in _additionalBoneTransforms)
            {
                 int index = _skinningData.BoneIndices[pair.Key];
                _boneTransforms[index] = pair.Value * _boneTransforms[index];
            }
            _animationPlayer.UpdateWorldTransforms(Matrix.Identity, _boneTransforms);
            _animationPlayer.UpdateSkinTransforms();

            CurrentTimeSpan += ts;
        }

    }
}
