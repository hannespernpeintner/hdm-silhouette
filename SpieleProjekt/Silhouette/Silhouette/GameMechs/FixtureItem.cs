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

namespace Silhouette.GameMechs
{
    [Serializable]
    public abstract class FixtureItem : LevelObject
    {
        [NonSerialized]
        public Fixture fixture;

        private float _density;
        [DisplayName("Mass"), Category("Physical Behavior")]
        [Description("The mass of the object to calculate physical interaction.")]
        public float density { get { return _density; } set { _density = value; } }

        public abstract void ToFixture();
    }

    #region Rectangle
        [Serializable]
        public class RectangleFixtureItem : FixtureItem
        {
            public Microsoft.Xna.Framework.Rectangle rectangle;

            private float _width;
            [DisplayName("Width"), Category("Fixture Data")]
            [Description("The width of the rectangle.")]
            public float width { get { return _width; } set { _width = value; transformed(); } }
            private float _height;
            [DisplayName("Height"), Category("Fixture Data")]
            [Description("The height of the rectangle.")]
            public float height { get { return _height; } set { _height = value; transformed(); } }

            private float _density;
            [DisplayName("Mass"), Category("Physical Behavior")]
            [Description("The mass of the object to calculate physical interaction.")]
            public float density { get { return _density; } set { _density = value; } }

            private BodyType _bodyType;
            [DisplayName("BodyType"), Category("Physical Behavior")]
            [Description("The BodyType defines the behavior of an object. A static object never changes position or rotation, like the dynamic ones do.")]
            public BodyType bodyType { get { return _bodyType; } set { _bodyType = value; } }

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
                RectangleFixtureItem result = (RectangleFixtureItem)this.MemberwiseClone();
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

            public override void ToFixture()
            {
                fixture = FixtureManager.CreateRectangle(width, height, position, bodyType, density);
            }
        }
    #endregion

    #region Circle
        [Serializable]
        public class CircleFixtureItem : FixtureItem
        {
            private float _radius;
            [DisplayName("Radius"), Category("Fixture Data")]
            [Description("The radius of the circle fixture.")]
            public float radius { get { return _radius; } set { _radius = value; } }

            private BodyType _bodyType;
            [DisplayName("BodyType"), Category("Physical Behavior")]
            [Description("The BodyType defines the behavior of an object. A static object never changes position or rotation, like the dynamic ones do.")]
            public BodyType bodyType { get { return _bodyType; } set { _bodyType = value; } }

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

            public override bool canScale() { return true; }
            public override Vector2 getScale() { return new Vector2(radius, radius); }
            public override void setScale(Vector2 scale) { radius = (float)Math.Round(scale.X); }

            public override bool canRotate() { return false; }
            public override float getRotation() { return 0; }
            public override void setRotation(float rotate) { }

            public override LevelObject clone()
            {
                CircleFixtureItem result = (CircleFixtureItem)this.MemberwiseClone();
                result.mouseOn = false;
                return result;
            }

            public override void transformed() { }

            public override bool contains(Vector2 worldPosition)
            {
                return (worldPosition - position).Length() <= radius;
            }

            public override void drawSelectionFrame(SpriteBatch spriteBatch, Matrix matrix)
            {
                Vector2 transformedRadius = Vector2.UnitX * radius;
                Primitives.Instance.drawCircle(spriteBatch, position, transformedRadius.Length(), Color.Yellow, 2);

                Vector2[] extents = new Vector2[4];
                extents[0] = position + Vector2.UnitX * transformedRadius.Length();
                extents[1] = position + Vector2.UnitY * transformedRadius.Length();
                extents[2] = position - Vector2.UnitX * transformedRadius.Length();
                extents[3] = position - Vector2.UnitY * transformedRadius.Length();

                foreach (Vector2 p in extents)
                {
                    Primitives.Instance.drawCircleFilled(spriteBatch, p, 4, Color.Yellow);
                }
            }

            public override void ToFixture()
            {
                fixture = FixtureManager.CreateCircle(radius, position, bodyType, density);
            }
        }
    #endregion

    #region Path
        [Serializable]
        public class PathFixtureItem : FixtureItem
        {
            private bool _isPolygon;
            [DisplayName("Polygon"), Category("Fixture Data")]
            [Description("Defines wether or not the path should be treated like a polygon. If the value is true the start and end of the path will be connected.")]
            public bool isPolygon { get { return _isPolygon; } set { _isPolygon = value; } }

            private int _lineWidth;
            [DisplayName("Line Width"), Category("Fixture Data")]
            [Description("The line width of this path. Can be used for rendering.")]
            public int lineWidth { get { return _lineWidth; } set { _lineWidth = value; } }

            public Vector2[] LocalPoints;
            public Vector2[] WorldPoints;

            public override void Initialise() { }
            public override void LoadContent() { ToFixture(); }
            public override void Update(GameTime gameTime) { }

            public PathFixtureItem(Vector2[] Points)
            {
                WorldPoints = Points;
                LocalPoints = (Vector2[])Points.Clone();
                position = Points[0];
                for (int i = 0; i < LocalPoints.Length; i++) LocalPoints[i] -= position;
                lineWidth = 4;
                transformed();
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

            public override bool canScale() { return true; }
            public override Vector2 getScale()
            {
                float length = (LocalPoints[1] - LocalPoints[0]).Length();
                return new Vector2(length, length);
            }
            public override void setScale(Vector2 scale)
            {
                float factor = scale.X / (LocalPoints[1] - LocalPoints[0]).Length();
                for (int i = 1; i < LocalPoints.Length; i++)
                {
                    Vector2 olddistance = LocalPoints[i] - LocalPoints[0];
                    LocalPoints[i] = LocalPoints[0] + olddistance * factor;
                }
                transformed();
            }

            public override bool canRotate() { return false; }
            public override float getRotation() { return 0; }
            public override void setRotation(float rotate) { }

            public override LevelObject clone()
            {
                PathFixtureItem result = (PathFixtureItem)this.MemberwiseClone();
                result.LocalPoints = (Vector2[])this.LocalPoints.Clone();
                result.WorldPoints = (Vector2[])this.WorldPoints.Clone();
                result.mouseOn = false;
                return result;
            }

            public override void transformed()
            {
                for (int i = 0; i < WorldPoints.Length; i++) WorldPoints[i] = LocalPoints[i] + position;
            }

            public override void drawSelectionFrame(SpriteBatch spriteBatch, Matrix matrix)
            {
                if (isPolygon)
                    Primitives.Instance.drawPolygon(spriteBatch, WorldPoints, Color.Yellow, 2);
                else
                    Primitives.Instance.drawPath(spriteBatch, WorldPoints, Color.Yellow, 2);

                foreach (Vector2 p in WorldPoints)
                {
                    Primitives.Instance.drawCircleFilled(spriteBatch, p, 4, Color.Yellow);
                }
            }

            public override void ToFixture()
            {
                if (isPolygon)
                {
                    FarseerPhysics.Common.Path path = new FarseerPhysics.Common.Path();
                    foreach (Vector2 v in WorldPoints)
                    {
                        path.Add(FixtureManager.ToMeter(v));
                    }
                    path.Closed = true;

                    PathManager.ConvertPathToPolygon(path, new Body(Level.Physics), density, WorldPoints.Length);
                }
                else
                {
                    FarseerPhysics.Common.Path path = new FarseerPhysics.Common.Path();
                    foreach (Vector2 v in WorldPoints)
                    {
                        path.Add(FixtureManager.ToMeter(v));
                    }
                    path.Closed = false;

                    PathManager.ConvertPathToEdges(path, new Body(Level.Physics), WorldPoints.Length * 3);
                }
            }
        }
    #endregion
}
