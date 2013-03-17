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

namespace Silhouette.GameMechs.Events
{
    public enum Attribute
    {
        Rotation,
        Position,
        Scale
    }

    [Serializable]
    public abstract class Event : LevelObject
    {
        /* Sascha:
         * Zentrale Klasse der Events. Implementiert alle Editorfunktionen und stellt alle grundlegenden Funktionen für alle Events bereit.
        */

        public Microsoft.Xna.Framework.Rectangle rectangle;

        [NonSerialized]
        public Fixture fixture;

        public bool isActivated;

        private float _width;
        [DisplayName("Width"), Category("Collision Data")]
        [Description("The width of the rectangle.")]
        public float width { get { return _width; } set { _width = value; transformed(); } }
        private float _height;
        [DisplayName("Height"), Category("Collision Data")]
        [Description("The height of the rectangle.")]
        public float height { get { return _height; } set { _height = value; transformed(); } }

        private List<LevelObject> _list;
        [DisplayName("Object List"), Category("Event Data")]
        [Description("The list of Objects which are affected by the event.")]
        public List<LevelObject> list { get { return _list; } set { _list = value; } }

        private bool _onlyOnPlayerCollision;
        [DisplayName("OnlyOnPlayerCollision"), Category("Event Data")]
        [Description("True, if the event should only be triggered by collision with the Player.")]
        public bool OnlyOnPlayerCollision { get { return _onlyOnPlayerCollision; } set { _onlyOnPlayerCollision = value; } }

        public override void Initialise() { }
        public override void LoadContent() { ToFixture(); }
        public override void Update(GameTime gameTime) { }

        public override string getPrefix()
        {
            return "Event_";
        }

        public override bool canScale() { return true; }
        public override Vector2 getScale() { return new Vector2(width, height); }
        public override void setScale(Vector2 scale)
        {
            width = (float)Math.Round(scale.X);
            height = (float)Math.Round(scale.Y);
            transformed();
        }

        public override bool canRotate() { return false; }
        public override float getRotation() { return 0; }
        public override void setRotation(float rotate) { }

        public override LevelObject clone()
        {
            Event result = (Event)this.MemberwiseClone();
            result.mouseOn = false;
            return result;
        }

        public override void transformed()
        {
            rectangle.Location = position.ToPoint();
            rectangle.Width = (int)width;
            rectangle.Height = (int)height;
        }

        public override bool contains(Vector2 worldPosition)
        {
            return rectangle.Contains(new Microsoft.Xna.Framework.Point((int)worldPosition.X, (int)worldPosition.Y));
        }

        public override void drawSelectionFrame(SpriteBatch spriteBatch, Matrix matrix)
        {
            Primitives.Instance.drawBox(spriteBatch, this.rectangle, Color.Yellow, 2);

            Vector2[] poly = rectangle.ToPolygon();

            foreach (Vector2 p in poly)
            {
                Primitives.Instance.drawCircleFilled(spriteBatch, p, 4, Color.Yellow);
            }
        }

        public abstract void ToFixture();
        public abstract void AddLevelObject(LevelObject lo);
    }
}
