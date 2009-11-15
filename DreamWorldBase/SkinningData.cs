using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace DreamWorldBase
{

    public class SkinningData
    {
        public SkinningData(Dictionary<string, AnimationClip> animationClips,
                            List<Matrix> bindPose, List<Matrix> inverseBindPose,
                            List<int> skeletonHierarchy,
                            Dictionary<string, int> boneIndices)
        {
            AnimationClips = animationClips;
            BindPose = bindPose;
            InverseBindPose = inverseBindPose;
            SkeletonHierarchy = skeletonHierarchy;
            BoneIndices = boneIndices;
        }

        private SkinningData()
        {
        } 


        [ContentSerializer]
        public Dictionary<string, AnimationClip> AnimationClips { get; private set; }


        [ContentSerializer]
        public List<Matrix> BindPose { get; private set; }


        [ContentSerializer]
        public List<Matrix> InverseBindPose { get; private set; }


        [ContentSerializer]
        public List<int> SkeletonHierarchy { get; private set; }


        [ContentSerializer]
        public Dictionary<string, int> BoneIndices { get; private set; }
    }
}
