﻿using System.Collections.Generic;
using System.Threading;
using DreamWorld.InputManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement
{
    public class ScreenManager : DrawableGameComponent
    {
        private List<Screen> screens = new List<Screen>();

        public InputManager InputManager { get; private set; }

        public SpriteBatch SpriteBatch { get; private set; }
        public SpriteFont Font { get; private set; }
        public Texture2D BlankTexture { get; private set; }
        public Rectangle FullscreenDestination { get; private set; }
        private bool Initialized;

        public ScreenManager(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            InputManager = ((DreamWorldGame)Game).InputManager;

            foreach (Screen screen in screens)
                screen.Initialize();
            FullscreenDestination = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            Initialized = true;
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            ContentManager content = Game.Content;
            Font = content.Load<SpriteFont>(@"Fonts/default");
            Font.Spacing *= 0.9f;
            BlankTexture = content.Load<Texture2D>(@"Textures/blank");
        }

        protected override void UnloadContent()
        {
            foreach (Screen screen in screens)
                screen.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            for (int i = screens.Count; i > 0; i--)
            {
                Screen screen = screens[i - 1];

                screen.OtherScreenHasFocus = otherScreenHasFocus;
                screen.CoveredByOtherScreen = coveredByOtherScreen;
                screen.Update(gameTime);

                if (screen.State == ScreenState.TransitionOn || screen.State == ScreenState.Active)
                {
                    otherScreenHasFocus = true;

                    if (!screen.IsPopup)
                        coveredByOtherScreen = true;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            DreamWorldGame dreamWorldGame = (DreamWorldGame)Game;
            dreamWorldGame.GraphicsDeviceManager.GraphicsDevice.Clear(Color.Black);

            foreach (Screen screen in screens)
            {
                if (screen.State == ScreenState.Hidden)
                    continue;

                screen.Draw(gameTime);
            }
        }

        public void AddScreen(Screen screen)
        {
            screen.ScreenManager = this;
            screen.IsExiting = false;

            screens.Add(screen);

            if (screen.LoadingScreen != null)
            {
                AddScreen(screen.LoadingScreen);
                if (Initialized)
                {
                    Thread loadingThread = new Thread(screen.Initialize);
                    loadingThread.Start();
                }
            }
            else if (Initialized)
            {
                screen.Initialize();
            }

        }

        public void RemoveScreen(Screen screen)
        {
            if (Initialized)
                screen.UnloadContent();

            screens.Remove(screen);
        }

        public void FadeBackBufferToBlack(int alpha)
        {
            Viewport viewport = GraphicsDevice.Viewport;

            SpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);

            SpriteBatch.Draw(BlankTexture,
                             new Rectangle(0, 0, viewport.Width, viewport.Height),
                             new Color(0, 0, 0, (byte)alpha));

            SpriteBatch.End();
        }

    }
}
