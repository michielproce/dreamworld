using System;
using DreamWorld.Cameras;
using DreamWorld.Entities;
using DreamWorld.Levels;
using Microsoft.Xna.Framework;

namespace DreamWorld.InputHandlers
{
    public abstract class InputHandler : GameComponent
    {
        protected Player player;               
        protected Camera camera;
        protected Level level;

        protected InputHandler(Game game) : base(game)
        {                     
        }

        public override void Initialize()
        {
            DreamWorldGame dwg = (DreamWorldGame)Game;
            player = dwg.Player;
            camera = dwg.CurrentCamera;
            level = dwg.CurrentLevel;               
        }

        public override void Update(GameTime gameTime)
        {
            if (!Game.IsActive)
                return;

            HandleInput(gameTime);
        }


        protected abstract void HandleInput(GameTime gameTime);
    }
}
