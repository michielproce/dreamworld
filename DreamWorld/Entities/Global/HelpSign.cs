using System;
using DreamWorld.Interface.Help;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Entities.Global
{
    public class HelpSign : Entity
    {
        public static bool ListInEditor = true;
        protected override void LoadContent()
        {
            Model = GameScreen.Content.Load<Model>(@"Models\Global\HelpSign");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            // If we're close to this entity, and we have a help item, set this as the active helper.
            if (Vector3.Distance(Level.Player.Body.Position, Body.Position) <= Help.HELP_DISTANCE
                && Help.Instance.HasHelp(this))
            {
                GameScreen.HelpSystem.Helper = this;
            }
            base.Update(gameTime);
        }
    }
}
