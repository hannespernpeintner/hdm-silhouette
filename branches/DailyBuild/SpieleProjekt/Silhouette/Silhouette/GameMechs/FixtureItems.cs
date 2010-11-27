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

        public override void drawSelectionFrame()
        {
            throw new NotImplementedException();
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

        public override void drawSelectionFrame()
        {
            throw new NotImplementedException();
        }

        public void ToFixture()
        {
            fixture = FixtureManager.CreateCircle(radius, position, BodyType.Static, 1);
        }
    }

    [Serializable]
    public class PathFixtureItem : LevelObject
    {
        private bool _isPolygon;
        [DisplayName("Polygon"), Category("Path Data")]
        [Description("Defines wether or not the path should be treated like a polygon. If the value is true the start and end of the path will be connected.")]
        public bool isPolygon { get { return _isPolygon; } set { _isPolygon = value; } }

        private int _lineWidth;
        [DisplayName("Line Width"), Category("Path Data")]
        [Description("The line width of this path. Can be used for rendering.")]
        public int lineWidth { get { return _lineWidth; } set { _lineWidth = value; } }

        public Vector2[] LocalPoints;
        public Vector2[] WorldPoints;


        public override void Initialise() { }
        public override void LoadContent() { ToFixture(); }
        public override void Update(GameTime gameTime) { }

        public PathFixtureItem(Vector2[] Points)
        {
            position = Points[0];
            WorldPoints = Points;
            LocalPoints = (Vector2[])Points.Clone();
            for (int i = 0; i < LocalPoints.Length; i++) LocalPoints[i] -= position;
            lineWidth = 4;
        }

        public override bool contains(Vector2 worldPosition)
        {
            for (int i = 1; i < WorldPoints.Length; i++)
            {
                if (worldPosition.DistanceToLineSegment(WorldPoints[i], WorldPoints[i - 1]) <= lineWidth) return true;
            }
            if (isPolygon)
                if (worldPosition.DistanceToLineSegment(WorldPoints[0], WorldPoints[WorldPoints.Length - 1]) <= lineWidth) return true;
            return false;
        }

        public override string getPrefix()
        {
            return "PathFixture_";
        }

        public override void drawSelectionFrame()
        {
            throw new NotImplementedException();
        }

        public void ToFixture()
        {

        }
    }
}
