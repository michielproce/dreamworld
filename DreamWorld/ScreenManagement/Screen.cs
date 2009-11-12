using System;
using DreamWorld.InputManagement;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;

namespace DreamWorld.ScreenManagement
{

    public enum ScreenState
    {
        TransitionOn,
        Active,
        TransitionOff,
        Hidden,
    }

    public abstract class Screen
    {

        public LoadingScreen LoadingScreen
        {
            get { return loadingScreen; }
            protected set { loadingScreen = value; }
        }

        LoadingScreen loadingScreen;


        public bool IsPopup
        {
            get { return isPopup; }
            protected set { isPopup = value; }
        }

        bool isPopup;


        public TimeSpan TransitionOnTime
        {
            get { return transitionOnTime; }
            protected set { transitionOnTime = value; }
        }

        TimeSpan transitionOnTime = TimeSpan.Zero;


        public TimeSpan TransitionOffTime
        {
            get { return transitionOffTime; }
            protected set { transitionOffTime = value; }
        }

        TimeSpan transitionOffTime = TimeSpan.Zero;


        public float TransitionPosition
        {
            get { return transitionPosition; }
            protected set { transitionPosition = value; }
        }

        float transitionPosition = 1;


        public byte TransitionAlpha
        {
            get { return (byte)(255 - TransitionPosition * 255); }
        }


        public ScreenState ScreenState
        {
            get { return screenState; }
            protected set { screenState = value; }
        }

        ScreenState screenState = ScreenState.TransitionOn;


        public bool IsExiting
        {
            get { return isExiting; }
            protected internal set { isExiting = value; }
        }

        bool isExiting;


        public bool IsActive
        {
            get
            {
                return !otherScreenHasFocus &&
                       (screenState == ScreenState.TransitionOn ||
                        screenState == ScreenState.Active);
            }
        }

        public bool CoveredByOtherScreen { get; set; }

        public bool OtherScreenHasFocus
        {
            get { return otherScreenHasFocus; }
            set { otherScreenHasFocus = value; }
        }

        bool otherScreenHasFocus;


        public ScreenManager ScreenManager
        {
            get { return screenManager; }
            internal set { screenManager = value; }
        }

        ScreenManager screenManager;


        public virtual void Initialize()
        {
            LoadContent();
        }

        protected virtual void LoadContent() { }

        public virtual void UnloadContent() { }


        public virtual void Update(GameTime gameTime)
        {

            if (isExiting)
            {
                screenState = ScreenState.TransitionOff;

                if (!UpdateTransition(gameTime, transitionOffTime, 1))
                {
                    ScreenManager.RemoveScreen(this);
                }
            }
            else if (CoveredByOtherScreen)
            {
                if (UpdateTransition(gameTime, transitionOffTime, 1))
                {
                    screenState = ScreenState.TransitionOff;
                }
                else
                {
                    screenState = ScreenState.Hidden;
                }
            }
            else
            {
                if (UpdateTransition(gameTime, transitionOnTime, -1))
                {
                    screenState = ScreenState.TransitionOn;
                }
                else
                {
                    screenState = ScreenState.Active;
                }
            }
            
            if (!OtherScreenHasFocus)
            {
                HandleInput();
            }
        }

        private bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
        {
            float transitionDelta;

            if (time == TimeSpan.Zero)
                transitionDelta = 1;
            else
                transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds /
                                          time.TotalMilliseconds);

            transitionPosition += transitionDelta * direction;

            // Did we reach the end of the transition?
            if (((direction < 0) && (transitionPosition <= 0)) ||
                ((direction > 0) && (transitionPosition >= 1)))
            {
                transitionPosition = MathHelper.Clamp(transitionPosition, 0, 1);
                return false;
            }

            return true;
        }

        public virtual void HandleInput() { }

        public virtual void Draw(GameTime gameTime) { }


        public void ExitScreen()
        {
            if (TransitionOffTime == TimeSpan.Zero)
            {
                ScreenManager.RemoveScreen(this);
            }
            else
            {
                isExiting = true;
            }
        }

    }
}
