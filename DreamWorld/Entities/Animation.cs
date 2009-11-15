using System;
using DreamWorldBase;
using Microsoft.Xna.Framework;

namespace DreamWorld.Entities
{
    public class Animation
    {
        private SkinningData skinningData;
        private AnimationPlayer animationPlayer;

        public bool Loaded { get; private set; }
        public string InitialClip { get; set; }
        public float Speed { get; set; }
        public bool Paused { get; set; }
        
        public Animation()
        {                                
        }

        public void Load(SkinningData skinningData)
        {
            this.skinningData = skinningData;
            animationPlayer = new AnimationPlayer(skinningData);
            if (InitialClip != null)
                StartClip(InitialClip);
            if(Paused)
                animationPlayer.Update(new TimeSpan(0L), true, Matrix.Identity);
            Loaded = true;
        }

        public void StartClip(string clip)
        {            
            animationPlayer.StartClip(skinningData.AnimationClips[clip]);
        }

        public Matrix[] SkinTransforms
        {
            get
            {
                return animationPlayer.GetSkinTransforms();
            }
        }

        public void AdvanceAnimation(GameTime gameTime)
        {            
            if(!Loaded || Paused)
                return;

            TimeSpan ts = gameTime.ElapsedGameTime;
            ts = ts.Add(new TimeSpan((long) (ts.Ticks * Speed) - ts.Ticks));
            animationPlayer.Update(ts, true, Matrix.Identity);
        }

    }
}
