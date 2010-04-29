using System.Collections.Generic;
using DreamWorld.Entities;
using DreamWorld.Levels;
using JigLibX.Collision;
using Microsoft.Xna.Framework;

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

    class IgnoreSkinPredicate : CollisionSkinPredicate1
    {
        readonly CollisionSkin _ignore;

        public IgnoreSkinPredicate(CollisionSkin skin)
        {
            _ignore = skin;
        }

        public override bool ConsiderSkin(CollisionSkin skin0)
        {
            if (skin0.Owner != null && skin0 != _ignore)
                return true;
            return false;
        }
    }

    class IsSelectablePredicate : CollisionSkinPredicate1
    {
        readonly PuzzleLevel _level;

        public IsSelectablePredicate(PuzzleLevel level)
        {
            _level = level;
        }

        public override bool ConsiderSkin(CollisionSkin skin)
        {
            if (skin.Owner == null)
                return false;

            foreach (KeyValuePair<int, Group> group in _level.Groups)
            {
                if (group.Value.AllowedRotations.Equals(Vector3.Zero) || !group.Value.IsInRange)
                    continue;

                foreach (KeyValuePair<string, Entity> entity in group.Value.Entities)
                {
                    if (entity.Value.Skin == skin)
                        return true;
                }
            }

            return false;
        }
    }
}
