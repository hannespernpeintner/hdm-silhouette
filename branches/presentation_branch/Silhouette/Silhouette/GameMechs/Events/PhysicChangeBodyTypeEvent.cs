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
    public class PhysicChangeBodyTypeEvent : Event
    {
        private BodyType _bodyType;
        [DisplayName("BodyType"), Category("Event Data")]
        [Description("All objects in list will change their BodyType property to this value.")]
        public BodyType bodyType { get { return _bodyType; } set { _bodyType = value; } }

        public PhysicChangeBodyTypeEvent(Rectangle rectangle)
        {
            this.rectangle = rectangle;
            position = rectangle.Location.ToVector2();
            width = rectangle.Width;
            height = rectangle.Height;
            list = new List<LevelObject>();
            isActivated = true;
        }

        public bool OnCollision(Fixture a, Fixture b, Contact contact)
        {
            if (isActivated)
            {
                foreach (LevelObject lo in this.list)
                {
                    if (lo is InteractiveObject)
                    {
                        InteractiveObject io = (InteractiveObject)lo;

                        io.bodyType = this.bodyType;

                        if(io.fixture != null)
                            io.fixture.Body.BodyType = this.bodyType;
                        if (io.fixtures != null)
                            io.fixtures[0].Body.BodyType = this.bodyType;

                    }

                    if (lo is CollisionObject)
                    {
                        CollisionObject fi = (CollisionObject)lo;
                        fi.bodyType = this.bodyType;
                        fi.fixture.Body.BodyType = this.bodyType;
                    }
                }

                isActivated = false;
                return true;
            }
            else
            {
                return false;
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
            return "ChangeBodyTypeEvent_";
        }

        public override LevelObject clone()
        {
            PhysicChangeBodyTypeEvent result = (PhysicChangeBodyTypeEvent)this.MemberwiseClone();
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
