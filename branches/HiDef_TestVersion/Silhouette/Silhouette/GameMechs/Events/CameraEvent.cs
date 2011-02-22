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
    public class CameraEvent : Event
    {
        private Attribute _attributeType;
        [DisplayName("Attribute"), Category("Event Data")]
        [Description("The attribute you want to change.")]
        public Attribute attributeType { get { return _attributeType; } set { _attributeType = value; } }

        private int _endValue;
        [DisplayName("Iterations"), Category("Event Data")]
        [Description("Defines how often the step value is applied.")]
        public int endValue { get { return _endValue; } set { _endValue = value; } }

        private float _step;
        [DisplayName("Step (Rotation/Scale)"), Category("Event Data")]
        [Description("The steps in which it changes. Caution: Use only if attribute is rotation or scale!")]
        public float step { get { return _step; } set { _step = value; } }

        private Vector2 _stepV;
        [DisplayName("Step (Position)"), Category("Event Data")]
        [Description("The steps in which it changes. Caution: Use only if attribute is position!")]
        public Vector2 stepV { get { return _stepV; } set { _stepV = value; } }

        [Browsable(false)]
        private bool isUpdate;
        [NonSerialized]
        int counter;

        public CameraEvent(Rectangle rectangle)
        {
            this.rectangle = rectangle;
            position = rectangle.Location.ToVector2();
            width = rectangle.Width;
            height = rectangle.Height;
            isActivated = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (isActivated)
            {
                if (isUpdate)
                {
                    switch (attributeType)
                    {
                        case Attribute.Rotation:
                            Camera.Rotation += this.step;
                            break;
                        case Attribute.Position:
                            Camera.Position += this.stepV;
                            break;
                        case Attribute.Scale:
                            Camera.Scale += this.step;
                            break;
                    }

                    counter--;

                    if (counter <= 0)
                    {
                        isUpdate = false;
                        isActivated = false;
                    }
                }
            }
            else
            {
                if (isUpdate)
                {
                    switch (attributeType)
                    {
                        case Attribute.Rotation:
                            Camera.Rotation -= this.step;
                            break;
                        case Attribute.Position:
                            Camera.Position -= this.stepV;
                            break;
                        case Attribute.Scale:
                            Camera.Scale -= this.step;
                            break;
                    }

                    counter--;

                    if (counter <= 0)
                    {
                        isUpdate = false;
                        isActivated = true;
                    }
                }
            }
        }

        public override void AddLevelObject(LevelObject lo) { }

        public override string getPrefix()
        {
            return "CameraEvent_";
        }

        public override LevelObject clone()
        {
            CameraEvent result = (CameraEvent)this.MemberwiseClone();
            result.mouseOn = false;
            return result;
        }

        public override void ToFixture()
        {
            fixture = FixtureManager.CreateRectangle(width, height, position, BodyType.Static, 1);
            fixture.OnCollision += this.OnCollision;
            fixture.IsSensor = true;
        }

        public bool OnCollision(Fixture a, Fixture b, Contact contact)
        {
            if (b.isEvent)
            {
                counter = endValue;
                isUpdate = true;
            }

            return true;
        }
    }
}
