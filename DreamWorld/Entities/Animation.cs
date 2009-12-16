using System;
using System.Collections.Generic;
using DreamWorldBase;
using Microsoft.Xna.Framework;

namespace DreamWorld.Entities
{
    public class Animation
    {
        private SkinningData skinningData;
        private AnimationPlayer animationPlayer;
        private Matrix[] boneTransforms;

        public bool Loaded { get; private set; }
        public string InitialClip { get; set; }
        public float Speed { get; set; }
        public bool Paused { get; set; }
        public string CurrentClip { get; private set; }

        private Dictionary<string, Matrix> additionalBoneTransforms;
        
        public Animation()
        {       
            additionalBoneTransforms = new Dictionary<string, Matrix>();             
        }

        public void Load(SkinningData skinningData)
        {
            this.skinningData = skinningData;            
            
            animationPlayer = new AnimationPlayer(skinningData);
            boneTransforms = new Matrix[skinningData.BindPose.Count];

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
                animationPlayer.StartClip(skinningData.AnimationClips[clip]);
                CurrentClip = clip;
            }
        }

        public Matrix[] SkinTransforms
        {
            get
            {
                return animationPlayer.GetSkinTransforms();
            }
        }

        public void Update(GameTime gameTime)
        {            
            if(!Loaded || Paused)
                return;

            TimeSpan ts = gameTime.ElapsedGameTime;
            ts = ts.Add(new TimeSpan((long) (ts.Ticks * Speed) - ts.Ticks));            
            
            // TODO: This if-statement is a hack for the less-then-one-second bug in kwxport
            if ("Run".Equals(CurrentClip) && animationPlayer.CurrentTime.Milliseconds > 750)
                    AdvanceAnimation(new TimeSpan(0, 0, 0, 0, 250));

            AdvanceAnimation(ts);
        }

        public void SetAdditionalBoneTransform(string bone, Matrix transform)
        {
            if (additionalBoneTransforms.ContainsKey(bone))
                additionalBoneTransforms[bone] = transform;
            else
                additionalBoneTransforms.Add(bone, transform);
        }

        private void AdvanceAnimation(TimeSpan ts)
        {
            animationPlayer.UpdateBoneTransforms(ts, true);
            animationPlayer.GetBoneTransforms().CopyTo(boneTransforms, 0);
            foreach (KeyValuePair<string, Matrix> pair in additionalBoneTransforms)
            {
                 int index = skinningData.BoneIndices[pair.Key];
                boneTransforms[index] = pair.Value * boneTransforms[index];
            }
            animationPlayer.UpdateWorldTransforms(Matrix.Identity, boneTransforms);
            animationPlayer.UpdateSkinTransforms();
        }

    }
}
