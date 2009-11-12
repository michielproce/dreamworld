using System;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Entities
{
    public class Terrain : Entity
    {
        private string terrain;
        public HeightMapInfo HeightMapInfo { get; private set; }

        public Terrain(string terrain)
        {
            this.terrain = terrain;
        }

        protected override void LoadContent()
        {
            Model = Screen.Content.Load<Model>(@"Terrains\" + terrain);
            HeightMapInfo = Model.Tag as HeightMapInfo;
            if (HeightMapInfo == null)
            {
                throw new InvalidOperationException("The terrain model did not have a HeightMapInfo " +
                    "object attached. Are you sure you are using the Terrain Processor?");
            }
            base.LoadContent();
        }      
    }
}
