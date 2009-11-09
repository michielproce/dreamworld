using System;
using DreamWorld.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Entities
{
    class Terrain : Entity
    {
        private string terrain;
        protected HeightMapInfo HeightMapInfo { get; set; }

        public Terrain(string terrain)
        {
            this.terrain = terrain;
        }

        protected override void LoadContent()
        {
            Model = Game.Content.Load<Model>(@"Terrains\" + terrain);
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
