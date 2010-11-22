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

//Physik-Engine Klassen
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;

namespace Silhouette.GameMechs
{
    [Serializable]
    public class RectangleFixtureItem : LevelObject
    {
        public Microsoft.Xna.Framework.Rectangle rectangle;

        [NonSerialized]
        public Texture2D texture;
        [NonSerialized]
        public Fixture fixture;
        public Vector2 position;
        public float width;
        public float height;

        public RectangleFixtureItem(Microsoft.Xna.Framework.Rectangle rectangle)
        {
            this.rectangle = rectangle;
            position = rectangle.Location.ToVector2();
            width = rectangle.Width;
            height = rectangle.Height;
        }

        public override void Initialise() { }
        public override void LoadContent() { ToFixture(); }
        public override void Update(GameTime gameTime) { }

        public override string getPrefix()
        {
            return "RectangleFixture_";
        }

        public override bool contains(Vector2 worldPosition)
        {
            return rectangle.Contains(new Microsoft.Xna.Framework.Point((int)worldPosition.X, (int)worldPosition.Y));
        }

        public void ToFixture()
        {
            fixture = FixtureManager.CreateRectangle(width, height, position, BodyType.Static, 1);
        }
    }

    [Serializable]
    public class CircleFixtureItem : LevelObject
    {
        public float radius;
        [NonSerialized]
        public Fixture fixture;

        public CircleFixtureItem(Vector2 position, float radius)
        {
            this.position = position;
            this.radius = radius;
        }

        public override void Initialise() { }
        public override void LoadContent() { ToFixture(); }
        public override void Update(GameTime gameTime) { }

        public override string getPrefix()
        {
            return "CircleFixture_";
        }

        public override bool contains(Vector2 worldPosition)
        {
            return (worldPosition - position).Length() <= radius;
        }

        public void ToFixture()
        {
            fixture = FixtureManager.CreateCircle(radius, position, BodyType.Static, 1);
        }
    }
}
