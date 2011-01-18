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
    public class ChangeVisibilityEvent : Event
    {
        private bool _visibility;
        [DisplayName("Visibility"), Category("Event Data")]
        [Description("All objects in list will change their visibility property to this value.")]
        public bool visibility { get { return _visibility; } set { _visibility = value; } }

        public ChangeVisibilityEvent(Rectangle rectangle)
        {
            this.rectangle = rectangle;
            position = rectangle.Location.ToVector2();
            width = rectangle.Width;
            height = rectangle.Height;
            list = new List<LevelObject>();
            isActivated = true;
        }

        public override void AddLevelObject(LevelObject lo)
        {
            if (this.list != null)
            {
                if (!this.list.Contains(lo))
                    this.list.Add(lo);
            }
        }

        public override string getPrefix()
        {
            return "ChangeVisibilityEvent_";
        }

        public override LevelObject clone()
        {
            ChangeVisibilityEvent result = (ChangeVisibilityEvent)this.MemberwiseClone();
            result.mouseOn = false;
            return result;
        }

        public bool OnCollision(Fixture a, Fixture b, Contact contact)
        {
            if (b.isEvent)
            {
                if (isActivated)
                {
                    foreach (LevelObject lo in this.list)
                    {
                        lo.isVisible = this.visibility;
                    }

                    isActivated = false;
                }
                else
                {
                    foreach (LevelObject lo in this.list)
                    {
                        lo.isVisible = !lo.isVisible;
                    }

                    isActivated = true;
                }
            }

            return true;
        }

        public override void ToFixture()
        {
            fixture = FixtureManager.CreateRectangle(width, height, position, BodyType.Static, 1);
            fixture.OnCollision += this.OnCollision;
            fixture.IsSensor = true;
        }
    }
}
