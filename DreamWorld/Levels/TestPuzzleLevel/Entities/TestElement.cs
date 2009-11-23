using DreamWorld.Entities;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Levels.TestPuzzleLevel.Entities
{
    class TestElement : Element
    {
        protected override void LoadContent()
        {
            Model = Content.Load<Model>(@"Models\Global\Zeppelin\Zeppelin");

            base.LoadContent();
        }
    }
}
