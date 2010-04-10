using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Entities
{
    public class Terrain : Entity
    {
        public const int MAX_SHADOWS = 50; // Same in terrain.fx

        private string terrain;
        
        private Entity[] shadowed;
        private int numberOfShadows;

        public HeightMapInfo HeightMapInfo { get; private set; }

        public Terrain(string terrain)
        {
            this.terrain = terrain;
            shadowed = new Entity[MAX_SHADOWS];
            IgnoreEdgeDetection = true;
        }

        protected override void LoadContent()
        {
            Model = GameScreen.Content.Load<Model>(@"Terrains\" + terrain);
            HeightMapInfo = Model.Tag as HeightMapInfo;
            if (HeightMapInfo == null)
            {
                throw new InvalidOperationException("The terrain model did not have a HeightMapInfo " +
                    "object attached. Are you sure you are using the Terrain Processor?");
            }
            base.LoadContent();
        }

        public void AddShadowedEntity(Entity entity)
        {
            shadowed[numberOfShadows++] = entity;
        }

        public override void Draw(GameTime gameTime, string technique)
        {
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    effect.Parameters["NumberOfShadows"].SetValue(numberOfShadows);
                    for (int i = 0; i < numberOfShadows; i++)
                    {
                        Entity entity = shadowed[i];
                        effect.Parameters["ShadowPositions"].Elements[i].SetValue(new Vector2(entity.Body.Position.X, entity.Body.Position.Z));
                        effect.Parameters["ShadowRadii"].Elements[i].SetValue(5);
                    }
                }
            }
            base.Draw(gameTime, technique);
        }
    }
}
