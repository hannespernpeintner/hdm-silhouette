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

using Silhouette.Engine.Effects;

namespace Silhouette.GameMechs.Events
{
    [Serializable]
    public class ChangeEffectEvent : Event
    {
        private float _targetFactor;
        [DisplayName("TargetFactor"), Category("Event Data")]
        [Description("Affects the effect's strength, from 0 to 100%.")]
        public float TargetFactor { get { return _targetFactor; } set { _targetFactor = value; } }

        private float _startFactor;
        [DisplayName("StartFactor"), Category("Event Data")]
        [Description("StartFactor.")]
        private float StartFactor { get { return _startFactor; } set { _startFactor = value; } }

        private int _duration;
        [DisplayName("Duration"), Category("Event Data")]
        [Description("The duration of the fade in Miliseconds.")]
        public int Duration { get { return _duration; } set { _duration = value; } }

        private int _currentDuration;
        [DisplayName("Duration"), Category("Event Data")]
        [Description("The current duration of the fade in Miliseconds.")]
        private int CurrentDuration { get { return _currentDuration; } set { _currentDuration = value; } }

        private static int _updateInterval = 10;

        public ChangeEffectEvent(Rectangle rectangle)
        {
            this.rectangle = rectangle;
            position = rectangle.Location.ToVector2();
            width = rectangle.Width;
            height = rectangle.Height;
            list = new List<LevelObject>();
            TargetFactor = 0.5f;
            Duration = 1000;
            CurrentDuration = 0;
            _setFactor += SetFactor;
        }

        public override void AddLevelObject(LevelObject lo)
        {
            if (this.list != null)
            {
                if (!this.list.Contains(lo) && lo is EffectObject)
                {
                    this.list.Add(lo);
                    this.StartFactor = ((EffectObject)lo).Factor;
                }
            }
        }

        public override string getPrefix()
        {
            return "ChangeEffectEvent_";
        }

        public override LevelObject clone()
        {
            ChangeEffectEvent result = (ChangeEffectEvent)this.MemberwiseClone();
            result.mouseOn = false;
            return result;
        }

        public bool OnCollision(Fixture a, Fixture b, Contact contact)
        {
            if (b.isEvent)
            {

                foreach (EffectObject eo in this.list)
                {
                    new Timer(_updateInterval, _updateInterval, (int)(Duration / _updateInterval), _setFactor);
                }
            }

            return true;
        }

        private Timer.OnTimeout _setFactor;

        public void SetFactor()
        {
            foreach (EffectObject eo in this.list)
            {
                CurrentDuration += _updateInterval;
                eo.Factor += (Duration - CurrentDuration) * (TargetFactor - StartFactor);

            }
        }

        public override void ToFixture()
        {
            fixture = FixtureManager.CreateRectangle(width, height, position, BodyType.Static, 1);
            fixture.OnCollision += this.OnCollision;
            fixture.IsSensor = true;
        }
    }
}
