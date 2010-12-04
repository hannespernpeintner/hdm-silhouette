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
    public class RectanglePrimitiveObject : DrawableLevelObject
    {
        private float _width;
        [DisplayName("Width"), Category("Primitive Data")]
        [Description("The width of the rectangle.")]
        public float width { get { return _width; } set { _width = value; transformed(); } }
        private float _height;
        [DisplayName("Height"), Category("Primitive Data")]
        [Description("The height of the rectangle.")]
        public float height { get { return _height; } set { _height = value; transformed(); } }
        private Color _color;
        [DisplayName("Color"), Category("Primitive Data")]
        [Description("The color of the primitive.")]
        public Color color { get { return _color; } set { _color = value; } }

        public Rectangle rectangle;

        public RectanglePrimitiveObject(Rectangle rectangle)
        {
            this.rectangle = rectangle;
            position = rectangle.Location.ToVector2();
            width = rectangle.Width;
            height = rectangle.Height;
            color = Constants.ColorPrimitives;
        }

        public override void Initialise() { }
        public override void LoadContent() { }
        public override void loadContentInEditor(GraphicsDevice graphics) { }
        public override void Update(GameTime gameTime) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Primitives.Instance.drawBoxFilled(spriteBatch, rectangle, color);
        }

        //---> Editor-Funktionalität <---//

        public override void drawInEditor(SpriteBatch spriteBatch)
        {
            Color onHover = color;
            if (this.mouseOn) onHover = Constants.onHover;
            Primitives.Instance.drawBoxFilled(spriteBatch, rectangle, onHover);
        }

        public override string getPrefix()
        {
            return "RectanglePrimitive_";
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
            throw new NotImplementedException();
        }

        public override bool contains(Vector2 worldPosition)
        {
            return rectangle.Contains(new Microsoft.Xna.Framework.Point((int)worldPosition.X, (int)worldPosition.Y));
        }

        public override void transformed()
        {
            rectangle.Location = position.ToPoint();
            rectangle.Width = (int)width;
            rectangle.Height = (int)height;
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
    }

    [Serializable]
    public class CirclePrimitiveObject : DrawableLevelObject
    {
        private Color _color;
        [DisplayName("Color"), Category("Primitive Data")]
        [Description("The color of the primitive.")]
        public Color color { get { return _color; } set { _color = value; } }

        public float radius;

        public CirclePrimitiveObject(Vector2 position, float radius)
        {
            this.position = position;
            this.radius = radius;
            color = Constants.ColorPrimitives;
        }

        public override void Initialise() { }
        public override void LoadContent() { }
        public override void loadContentInEditor(GraphicsDevice graphics) { }
        public override void Update(GameTime gameTime) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Primitives.Instance.drawCircleFilled(spriteBatch, position, radius, color);
        }

        //---> Editor-Funktionalität <---//

        public override void drawInEditor(SpriteBatch spriteBatch)
        {
            Color onHover = color;
            if (this.mouseOn) onHover = Constants.onHover;
            Primitives.Instance.drawCircleFilled(spriteBatch, position, radius, onHover);
        }

        public override string getPrefix()
        {
            return "CirclePrimitive_";
        }

        public override bool canScale() { return true; }
        public override Vector2 getScale() { return new Vector2(radius, radius); }
        public override void setScale(Vector2 scale) { radius = (float)Math.Round(scale.X); }

        public override bool canRotate() { return false; }
        public override float getRotation() { return 0; }
        public override void setRotation(float rotate) { }

        public override LevelObject clone()
        {
            throw new NotImplementedException();
        }

        public override bool contains(Vector2 worldPosition)
        {
            return (worldPosition - position).Length() <= radius;
        }

        public override void transformed() { }

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
    }

    [Serializable]
    public class PathPrimitiveObject : DrawableLevelObject
    {
        private bool _isPolygon;
        [DisplayName("Polygon"), Category("Primitive Data")]
        [Description("Defines wether or not the path should be treated like a polygon. If the value is true the start and end of the path will be connected.")]
        public bool isPolygon { get { return _isPolygon; } set { _isPolygon = value; } }
        private int _lineWidth;
        [DisplayName("Line Width"), Category("Primitive Data")]
        [Description("The line width of this path. Can be used for rendering.")]
        public int lineWidth { get { return _lineWidth; } set { _lineWidth = value; } }
        private Color _color;
        [DisplayName("Color"), Category("Primitive Data")]
        [Description("The color of the primitive.")]
        public Color color { get { return _color; } set { _color = value; } }

        public Vector2[] LocalPoints;
        public Vector2[] WorldPoints;

        public PathPrimitiveObject(Vector2[] Points)
        {
            WorldPoints = Points;
            LocalPoints = (Vector2[])Points.Clone();
            position = Points[0];
            for (int i = 0; i < LocalPoints.Length; i++) LocalPoints[i] -= position;
            lineWidth = 4;
            color = Constants.ColorPrimitives;
            transformed();
        }

        public override void Initialise() { }
        public override void LoadContent() { }
        public override void loadContentInEditor(GraphicsDevice graphics) { }
        public override void Update(GameTime gameTime) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.isPolygon)
                Primitives.Instance.drawPolygon(spriteBatch, WorldPoints, color, lineWidth);
            else
                Primitives.Instance.drawPath(spriteBatch, WorldPoints, color, lineWidth);
        }

        //---> Editor-Funktionalität <---//

        public override void drawInEditor(SpriteBatch spriteBatch)
        {
            Color onHover = color;
            if (this.mouseOn) onHover = Constants.onHover;
            if (this.isPolygon)
                Primitives.Instance.drawPolygon(spriteBatch, WorldPoints, onHover, lineWidth);
            else
                Primitives.Instance.drawPath(spriteBatch, WorldPoints, onHover, lineWidth);
        }

        public override string getPrefix()
        {
            return "PathPrimitive_";
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
        public override void setRotation(float rotate) {  }

        public override LevelObject clone()
        {
            throw new NotImplementedException();
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
    }
}
