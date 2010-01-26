﻿using JigLibX.Collision;

namespace DreamWorld.Util
{
    class DefaultSkinPredicate : CollisionSkinPredicate1
    {
        public override bool ConsiderSkin(CollisionSkin skin0)
        {
            if (skin0.Owner != null)
                return true;
            return false;
        }
    }

    class ImmovableSkinPredicate : CollisionSkinPredicate1
    {
        public override bool ConsiderSkin(CollisionSkin skin0)
        {
            if (skin0.Owner != null && skin0.Owner.Immovable)
                return true;
            return false;
        }
    }
}
