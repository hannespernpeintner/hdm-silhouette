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
using Silhouette;
using Silhouette.Engine;
using Silhouette.Engine.Manager;
using Silhouette.GameMechs;
using System.IO;
using System.ComponentModel;

//Physik-Engine Klassen
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics.Contacts;

namespace Silhouette.GameMechs.Events
{
    [Serializable]
    public class ChangeGlobalGravityEvent : Event
    {
        private Vector2 _targetForce;
        [DisplayName("Target force"), Category("Event Data")]
        [Description("The aimed global gravity force.")]
        public Vector2 TargetForce { get { return _targetForce; } set { _targetForce = value; } }

        private Vector2 _startForce;
        [DisplayName("Start force"), Category("Event Data")]
        [Description("The gravity at the time of the event start.")]
        private Vector2 StartForce { get { return _startForce; } set { _startForce = value; } }

        private float _duration;
        [DisplayName("Duration"), Category("Event Data")]
        [Description("The duration for the change in gravitation in ms.")]
        public float Duration { get { return _duration; } set { _duration = value; } }

        private float _stepAmountX;
        private float StepAmountX { get { return _stepAmountX; } set { _stepAmountX = value; } }
        private float _stepAmountY;
        private float StepAmountY { get { return _stepAmountY; } set { _stepAmountY = value; } }

        private Timer.OnTimeout _setXHandler;
        private Timer.OnTimeout SetXHandler
        {
            get { return _setXHandler; }
            set { _setXHandler = value; }
        }
        private Timer.OnTimeout _setYHandler;
        private Timer.OnTimeout SetYHandler
        {
            get { return _setYHandler; }
            set { _setYHandler = value; }
        }

        [NonSerialized]
        private Timer _timerX;
        private Timer TimerX
        {
            get { return _timerX; }
            set { _timerX = value; }
        }
        [NonSerialized]
        private Timer _timerY;
        private Timer TimerY
        {
            get { return _timerY; }
            set { _timerY = value; }
        }
        [NonSerialized]
        private bool _initialized;

        public ChangeGlobalGravityEvent(Rectangle rectangle)
        {
            this.rectangle = rectangle;
            position = rectangle.Location.ToVector2();
            width = rectangle.Width;
            height = rectangle.Height;
            list = new List<LevelObject>();
            isActivated = true;
            OnlyOnPlayerCollision = true;
            Duration = 1000f;
            SetXHandler += SetX;
            SetYHandler += SetY;
            _initialized = false;
        }
        private static int _timerIntervalMS = 10;
        public bool OnCollision(Fixture a, Fixture b, Contact contact)
        {
            if (isActivated && ((OnlyOnPlayerCollision && b.isPlayer) || !OnlyOnPlayerCollision))
            {
                if (!_initialized)
                {
                    StartForce = layer.level.Gravitation;
                    float totalAmountX = TargetForce.X - StartForce.X;
                    float totalAmountY = TargetForce.Y - StartForce.Y;
                    StepAmountX = totalAmountX * (_timerIntervalMS / Duration);
                    StepAmountY = totalAmountY * (_timerIntervalMS / Duration);
                    int stepsX = (int)(Duration / _timerIntervalMS);
                    int stepsY = (int)(Duration / _timerIntervalMS);

                    TimerX = new Timer(0, _timerIntervalMS, stepsX, SetXHandler);
                    TimerY = new Timer(0, _timerIntervalMS, stepsY, SetYHandler);
                    _initialized = true;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetX()
        {
            Console.WriteLine(TimerX.RepeatCount + " | " + TimerX.RepeatInterval + " GravityX: " + Level.Physics.Gravity.X);
            Level.Physics.Gravity.X += StepAmountX;
            if ((StartForce.X < TargetForce.X && Level.Physics.Gravity.X >= TargetForce.X)
                || (StartForce.X > TargetForce.X && Level.Physics.Gravity.X <= TargetForce.X))
            {
                Level.Physics.Gravity.X = TargetForce.X;
                TimerX.Active = false;
                _initialized = false;
            }
        }

        public void SetY()
        {
            Console.WriteLine(TimerY.RepeatCount + " | " + TimerY.RepeatInterval + " GravityY: " + Level.Physics.Gravity.Y);
            Level.Physics.Gravity.Y += StepAmountY;
            if ((StartForce.Y < TargetForce.Y && Level.Physics.Gravity.Y >= TargetForce.Y)
                || (StartForce.Y > TargetForce.Y && Level.Physics.Gravity.Y <= TargetForce.Y))
            {
                Level.Physics.Gravity.Y = TargetForce.Y;
                TimerX.Active = false;
                _initialized = false;
            }
        }

        public override void AddLevelObject(LevelObject lo)
        {
            if (this.list != null)
            {
                if(!this.list.Contains(lo) && (lo is InteractiveObject || lo is CollisionObject))
                    this.list.Add(lo);
            }
        }

        public override string getPrefix()
        {
            return "ChangeGlobalGravityEvent_";
        }

        public override LevelObject clone()
        {
            ChangeGlobalGravityEvent result = (ChangeGlobalGravityEvent)this.MemberwiseClone();
            result.mouseOn = false;
            return result;
        }

        public override void ToFixture()
        {
            fixture = FixtureManager.CreateRectangle(width, height, position, BodyType.Static, 1);
            fixture.OnCollision += this.OnCollision;
            fixture.IsSensor = true;
        }
    }
}
