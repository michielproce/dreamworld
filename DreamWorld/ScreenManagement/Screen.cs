using System;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

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
        public ContentManager Content { get; private set; }

        public LoadingScreen LoadingScreen { get; protected set; }

        public bool IsPopup { get; protected set; }

        public TimeSpan TransitionOnTime { get; set; }

        public TimeSpan TransitionOffTime { get; set; }

        public float TransitionPosition { get; protected set; }

        public byte TransitionAlpha
        {
            get { return (byte)(255 - TransitionPosition * 255); }
        }

        public ScreenState State { get; protected set; }

        public bool IsExiting { get; protected internal set; }

        public bool IsActive
        {
            get
            {
                return !OtherScreenHasFocus &&
                       (State == ScreenState.TransitionOn ||
                        State == ScreenState.Active);
            }
        }

        public bool CoveredByOtherScreen { get; set; }

        public bool OtherScreenHasFocus { get; set; }

        public ScreenManager ScreenManager { get; set; }

        public Screen ()
        {
            TransitionOffTime = TimeSpan.Zero;
            TransitionOnTime = TimeSpan.Zero;
            State = ScreenState.TransitionOn;
            TransitionPosition = 1;
        }

        public virtual void Initialize()
        {
            Content = new ContentManager(ScreenManager.Game.Services) { RootDirectory = "Content" };
            LoadContent();
        }

        protected virtual void LoadContent() { }

        public virtual void UnloadContent() { }


        public virtual void Update(GameTime gameTime)
        {

            if (IsExiting)
            {
                State = ScreenState.TransitionOff;

                if (!UpdateTransition(gameTime, TransitionOffTime, 1))
                {
                    ScreenManager.RemoveScreen(this);
                    HandleExit();
                }
            }
            else if (CoveredByOtherScreen)
            {
                if (UpdateTransition(gameTime, TransitionOffTime, 1))
                {
                    State = ScreenState.TransitionOff;
                }
                else
                {
                    State = ScreenState.Hidden;
                }
            }
            else
            {
                if (UpdateTransition(gameTime, TransitionOnTime, -1))
                {
                    State = ScreenState.TransitionOn;
                }
                else
                {
                    State = ScreenState.Active;
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

            TransitionPosition += transitionDelta * direction;

            // Did we reach the end of the transition?
            if (((direction < 0) && (TransitionPosition <= 0)) ||
                ((direction > 0) && (TransitionPosition >= 1)))
            {
                TransitionPosition = MathHelper.Clamp(TransitionPosition, 0, 1);
                return false;
            }

            return true;
        }

        public virtual void HandleInput() { }
        
        public virtual void HandleExit() { }

        public virtual void Draw(GameTime gameTime) { }


        public void ExitScreen()
        {            
            if (TransitionOffTime == TimeSpan.Zero)
            {
                ScreenManager.RemoveScreen(this);
                HandleExit();
            }
            else
            {
                IsExiting = true;
            }
        }
    }
}
