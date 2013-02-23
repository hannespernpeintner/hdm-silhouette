using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Silhouette.Engine;
using Silhouette.GameMechs;

namespace Silhouette.Engine
{
    public class Timer : LevelObject
    {
        public enum TimerType
        { 
            CountDown
        }

        private TimerType _type;

        public TimerType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private int _startInMiliSeconds;
        public int StartInMiliSeconds
        {
            get { return _startInMiliSeconds; }
            set { _startInMiliSeconds = value; }
        }

        private int _miliSecondsLeft;
        public int MiliSecondsLeft
        {
            get { return _miliSecondsLeft; }
            set { _miliSecondsLeft = value; }
        }

        public delegate void OnTimeout();
        OnTimeout _handler;
        public OnTimeout Handler
        {
          get { return _handler; }
          set { _handler = value; }
        }

        private bool _active;
        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        public Timer(int MiliSeconds, OnTimeout PassedDelegate)
               :this(TimerType.CountDown, 0, MiliSeconds, PassedDelegate)
        {
        }

        // Caution, the explicit start time has to be passed
        public Timer(TimerType type, int startInMiliSeconds, int MiliSeconds, OnTimeout PassedDelegate)
        {
            // nix mit Verarschen hier
            if (startInMiliSeconds < 0)
            {
                startInMiliSeconds = -startInMiliSeconds;
            }
            if (MiliSeconds < 0)
            {
                MiliSeconds = -MiliSeconds;
            }

            Type = type;
            StartInMiliSeconds = startInMiliSeconds;
            MiliSecondsLeft = MiliSeconds;
            Handler += PassedDelegate;
            Active = true;
        }

        public override void Update(GameTime gameTime)
        {

            if(!Active)
            {
                return;
            }

            int dt = gameTime.ElapsedGameTime.Milliseconds;

            if (StartInMiliSeconds > 0)
            {
                StartInMiliSeconds -= dt;
            }
            else if (MiliSecondsLeft > 0)
            {
                StartInMiliSeconds = 0;
                MiliSecondsLeft -= dt;
            }
            else if (MiliSecondsLeft <= 0)
            {
                MiliSecondsLeft = 0;
                DoTimerAction();
            }
        }

        public override void Initialise()
        {
        }

        public override void LoadContent()
        {
        }

        private void DoTimerAction()
        {
            if (Type == TimerType.CountDown)
            {
                try
                {
                    Active = false;
                    Handler();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Timer wasn't able to execute its delegate.");
                }
            }
        }
    }
}
