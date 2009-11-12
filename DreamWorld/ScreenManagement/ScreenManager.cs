using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using DreamWorld.InputManagement;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement
{

    public class ScreenManager : DrawableGameComponent
    {

        List<Screen> screens = new List<Screen>();

        public InputManager InputManager
        {
            get { return inputManager; }
        }
        InputManager inputManager;

        SpriteBatch spriteBatch;
        SpriteFont font;
        Texture2D blankTexture;

        bool isInitialized;

        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        public SpriteFont Font
        {
            get { return font; }
        }

        public ScreenManager(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            DreamWorldGame dreamWorldGame = (DreamWorldGame)Game;
            inputManager = dreamWorldGame.InputManager;

            foreach (Screen screen in screens)
            {
                screen.Initialize();
            }

            isInitialized = true;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ContentManager content = Game.Content;
            font = content.Load<SpriteFont>(@"Fonts/Test/menufont");
            blankTexture = content.Load<Texture2D>(@"Textures/blank");
        }

        protected override void UnloadContent()
        {
            foreach (Screen screen in screens)
            {
                screen.UnloadContent();
            }
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

                if (screen.ScreenState == ScreenState.TransitionOn || screen.ScreenState == ScreenState.Active)
                {
                    otherScreenHasFocus = true;

                    if (!screen.IsPopup)
                        coveredByOtherScreen = true;
                }
            }
        }

        private void TraceScreens()
        {
            List<string> screenNames = new List<string>();

            foreach (Screen screen in screens)
                screenNames.Add(screen.GetType().Name + " " + screen.ScreenState);

            Trace.WriteLine(string.Join(", ", screenNames.ToArray()));
        }

        public override void Draw(GameTime gameTime)
        {
            DreamWorldGame dreamWorldGame = (DreamWorldGame)Game;
            dreamWorldGame.GraphicsDeviceManager.GraphicsDevice.Clear(Color.Black);

            foreach (Screen screen in screens)
            {
                if (screen.ScreenState == ScreenState.Hidden)
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
                if (isInitialized)
                {
                    Thread loadingThread = new Thread(screen.Initialize);
                    loadingThread.Start();
                }
            }
            else if (isInitialized)
            {
                screen.Initialize();
            }

        }

        public void RemoveScreen(Screen screen)
        {
            if (isInitialized)
            {
                screen.UnloadContent();
            }

            screens.Remove(screen);
        }

        public Screen[] GetScreens()
        {
            return screens.ToArray();
        }

        public void FadeBackBufferToBlack(int alpha)
        {
            Viewport viewport = GraphicsDevice.Viewport;

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);

            spriteBatch.Draw(blankTexture,
                             new Rectangle(0, 0, viewport.Width, viewport.Height),
                             new Color(0, 0, 0, (byte)alpha));

            spriteBatch.End();
        }

    }
}
