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
using Silhouette.Engine.Manager;

namespace Silhouette.Engine
{
    public class Timer : LevelObject
    {
        public enum TimerType
        { 
            CountDown,
            Repeated
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

        public static void PrintSomething()
        {
            Console.WriteLine("Test");
        }

        private bool _active;
        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        private int _repeatInterval;
        public int RepeatInterval
        {
            get { return _repeatInterval; }
            set { _repeatInterval = value; }
        }

        private int _repeatCount;
        public int RepeatCount
        {
            get { return _repeatCount; }
            set { _repeatCount = value; }
        }

        // Simplest timer ever: starts right now, executes delegate after timed out.
        public Timer(int MiliSeconds, OnTimeout PassedDelegate)
            : this(TimerType.CountDown, 0, MiliSeconds, 0, 0, PassedDelegate)
        {
        }

        // Pass a negative value for and endless timer.
        public Timer(int MiliSeconds, int repeatInterval, int repeatCount, OnTimeout PassedDelegate)
            : this(TimerType.Repeated, 0, MiliSeconds, repeatCount, repeatInterval, PassedDelegate)
        {
        }

        public Timer(TimerType type, int startInMiliSeconds, int MiliSeconds, int repeatCount, int repeatInterval, OnTimeout PassedDelegate)
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
            RepeatInterval = repeatInterval;
            RepeatCount = repeatCount;
            Active = true;
            TimerManager.Timers.Add(this);
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
            else if (Type == TimerType.Repeated)
            {
                try
                {
                    if (RepeatCount == 0)
                    {
                        Active = false;
                        return;
                    }

                    Handler();
                    RepeatCount--;
                    MiliSecondsLeft = RepeatInterval;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Timer wasn't able to execute its delegate.");
                }
            }
        }
    }
}
