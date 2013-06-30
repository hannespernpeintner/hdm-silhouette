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
using FarseerPhysics.Common.Decomposition;

//Physik-Engine Klassen
using FarseerPhysics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;

namespace Silhouette.GameMechs
{
    [Serializable]
    public abstract class JointObject : DrawableLevelObject
    {
        private Color _color;
        [DisplayName("Color"), Category("Primitive Data")]
        [Description("The color of the primitive.")]
        public Color color { get { return _color; } set { _color = value; } }

        [NonSerialized]
        private Joint _joint;
        public Joint Joint { get { return _joint; } set { _joint = value; } }


        protected static int radius = 15;
        private CirclePrimitiveObject _circle;
        public CirclePrimitiveObject Circle
        {
            get
            {
                return _circle;
            }
            set
            {
                _circle = value;
            }
        }

        private InteractiveObject _object1;
        [DisplayName("Object1"), Category("Object Data")]
        [Description("The first affected object.")]
        public InteractiveObject Object1
        {
            get
            {
                return _object1;
            }
            set
            {
                _object1 = value;
            }
        }
        private InteractiveObject _object2;
        [DisplayName("Object2"), Category("Object Data")]
        [Description("The second affected object.")]
        public InteractiveObject Object2
        {
            get
            {
                return _object2;
            }
            set
            {
                _object2 = value;
            }
        }
    }

    #region RevoluteJoint
    [Serializable]
    public class RevoluteJointObject : JointObject
    {

        private bool _enableLimit;
        [DisplayName("Enable Limit"), Category("Joint Data")]
        [Description("Enables the minimum and maximum angles for this joint.")]
        public bool EnableLimit
        {
            get { return _enableLimit; }
            set { _enableLimit = value; }
        }
        private bool _enableMotor;
        [DisplayName("Enable Motor"), Category("Joint Data")]
        [Description("Enables the joint's motor.")]
        public bool EnableMotor
        {
            get { return _enableMotor; }
            set { _enableMotor = value; }
        }

        private float _minAngle;
        [DisplayName("Minimum angle (rad)"), Category("Joint Data")]
        [Description("If defined and activated, the joint will prevent the object from underpassing this angle.")]
        public float MinAngle
        {
            get { return _minAngle; }
            set { _minAngle = value; }
        }

        private float _maxAngle;
        [DisplayName("Maximum angle (rad)"), Category("Joint Data")]
        [Description("If defined and activated, the joint will prevent the object from overpassing this angle.")]
        public float MaxAngle
        {
            get { return _maxAngle; }
            set { _maxAngle = value; }
        }

        private float _motorSpeed;
        [DisplayName("Motor speed (rad/s)"), Category("Joint Data")]
        [Description("If defined and activated, the joint will use a motor, that forces the object to rotate.")]
        public float MotorSpeed
        {
            get { return _motorSpeed; }
            set { _motorSpeed = value; }
        }

        public RevoluteJointObject()
        {
            color = Constants.ColorPrimitives;
            MaxAngle = (float)Math.PI;
            MinAngle = (float)-Math.PI;
            EnableLimit = false;
            MotorSpeed = (float)Math.PI;
            EnableMotor = false;
        }

        public override void Initialise()
        {
            Circle = new CirclePrimitiveObject(position, JointObject.radius);
        }

        public override void LoadContent()
        {
        }

        public override void loadContentInEditor(GraphicsDevice graphics) { }
        public override void Update(GameTime gameTime)
        {
            if (Joint == null)
            {
                if (Object1 == null) 
                {
                    return;
                }
                if (Object2 == null)
                {
                    Fixture referenceObject = FixtureFactory.CreateRectangle(Level.Physics, 0.1f, 0.1f, 0);
                    referenceObject.Body.Position = Object1.fixture.Body.Position;
                    referenceObject.Body.BodyType = BodyType.Static;
                    Joint = JointFactory.CreateRevoluteJoint(Level.Physics, Object1.fixture.Body, referenceObject.Body, Vector2.Zero);
                }
                else 
                {
                    Joint = JointFactory.CreateRevoluteJoint(Level.Physics, Object1.fixture.Body, Object2.fixture.Body, Vector2.Zero);
                }
                ((RevoluteJoint)Joint).LowerLimit = MinAngle;
                ((RevoluteJoint)Joint).UpperLimit = MaxAngle;
                ((RevoluteJoint)Joint).LimitEnabled = EnableLimit;
                ((RevoluteJoint)Joint).MotorSpeed = MotorSpeed;
                ((RevoluteJoint)Joint).MotorEnabled = EnableMotor;
            }


            Circle.position = position;
        }

        public override void transformed()
        {
            base.transformed();

            if (Circle == null)
            {
                return;
            }

            Circle.position = position;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }

        //---> Editor-Funktionalität <---//

        public override void drawInEditor(SpriteBatch spriteBatch)
        {
            if (Circle == null)
            {
                return;
            }
            Color onHover = color;
            if (this.mouseOn) onHover = Constants.onHover;
            color = onHover;
            Circle.drawInEditor(spriteBatch);

            if (Object1 != null)
            {
                Primitives.Instance.drawLine(spriteBatch, Object1.position, Circle.position, color, 5);
            }
            if (Object2 != null)
            {
                Primitives.Instance.drawLine(spriteBatch, Object2.position, Circle.position, color, 5);
            }
        }

        public override string getPrefix()
        {
            return "RevoluteJointObject_";
        }

        public override LevelObject clone()
        {
            RevoluteJointObject result = (RevoluteJointObject)this.MemberwiseClone();
            result.mouseOn = false;
            return result;
        }

        public override bool contains(Vector2 worldPosition)
        {
            return Circle.contains(new Vector2((int)worldPosition.X, (int)worldPosition.Y));
        }

        public override void drawSelectionFrame(SpriteBatch spriteBatch, Matrix matrix)
        {
            Rectangle rect = new Rectangle((int)(Circle.position.X - Circle.radius), (int)(Circle.position.Y - Circle.radius), 2*(int)Circle.radius, 2*(int)Circle.radius);
            Primitives.Instance.drawBox(spriteBatch, rect, Color.Yellow, 2);

            Vector2[] poly = rect.ToPolygon();

            foreach (Vector2 p in poly)
            {
                Primitives.Instance.drawCircleFilled(spriteBatch, p, 4, Color.Yellow);
            }
        }
    }
    #endregion

    #region PulleyJoint
    [Serializable]
    public class PulleyJointObject : JointObject
    {
        private float _ratio;
        [DisplayName("Ratio of the ropes"), Category("Joint Data")]
        [Description("The ratio between the growths of the ropes.")]
        public float Ratio
        {
            get { return _ratio; }
            set { _ratio = value; }
        }
        private int _lineWidth;
        [DisplayName("Width of the ropes"), Category("Joint Data")]
        [Description("The thickness of the rope.")]
        public int LineWidth
        {
            get { return _lineWidth; }
            set { _lineWidth = value; }
        }

        private Vector2 _anchorA;
        [DisplayName("Anchor A"), Category("Joint Data")]
        [Description("Vector for the offset from Object 1's center.")]
        public Vector2 AnchorA
        {
            get { return _anchorA; }
            set { _anchorA = value; }
        }

        private Vector2 _anchorB;
        [DisplayName("Anchor B"), Category("Joint Data")]
        [Description("Vector for the offset from Object 2's center.")]
        public Vector2 AnchorB
        {
            get { return _anchorB; }
            set { _anchorB = value; }
        }

        public PulleyJointObject()
        {
            color = Constants.ColorPrimitives;
            Ratio = 1.0f;
            LineWidth = 8;
            AnchorA = Vector2.Zero;
            AnchorB = Vector2.Zero;
        }

        [NonSerialized]
        private Vector2 tempAnchorA;

        [NonSerialized]
        private Vector2 tempAnchorB;
        [NonSerialized]
        private Vector2 tempAnchorA2;

        [NonSerialized]
        private Vector2 tempAnchorB2;

        public override void Initialise()
        {
            Circle = new CirclePrimitiveObject(position, JointObject.radius);
        }

        public override void LoadContent()
        {
        }

        public override void loadContentInEditor(GraphicsDevice graphics) { }
        public override void Update(GameTime gameTime)
        {
            if (Joint == null)
            {
                /*Object1.bodyType = BodyType.Dynamic;
                if (Object1.fixtures != null)
                    Object1.fixtures[0].Body.BodyType = BodyType.Dynamic;
                Object2.bodyType = BodyType.Dynamic;
                if (Object2.fixtures != null)
                    Object2.fixtures[0].Body.BodyType = BodyType.Dynamic;*/

                tempAnchorA = new Vector2(Object1.fixture.Body.WorldCenter.X, this.position.Y / Level.PixelPerMeter);
                tempAnchorB = new Vector2(Object2.fixture.Body.WorldCenter.X, this.position.Y / Level.PixelPerMeter);
                tempAnchorA2 = new Vector2(Object1.position.X, this.position.Y );
                tempAnchorB2 = new Vector2(Object2.position.X, this.position.Y );
                Joint = JointFactory.CreatePulleyJoint(Level.Physics, Object1.fixture.Body, Object2.fixture.Body, tempAnchorA, tempAnchorB, AnchorA, AnchorB, Ratio);
                Joint.CollideConnected = true;
            }
        }

        public override void transformed()
        {
            base.transformed();

            if (Circle == null)
            {
                return;
            }

            Circle.position = position;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(Object1 != null && Object2 != null && Joint != null)
            {
                Vector2 anchorA = new Vector2(Object1.fixture.Body.WorldCenter.X, this.position.Y / Level.PixelPerMeter);
                Vector2 anchorB = new Vector2(Object2.fixture.Body.WorldCenter.X, this.position.Y / Level.PixelPerMeter);

                Primitives.Instance.drawLine(spriteBatch, Object1.position, tempAnchorA2, Color.Black, LineWidth);
                Primitives.Instance.drawLine(spriteBatch, Object2.position, tempAnchorB2, Color.Black, LineWidth);
                Primitives.Instance.drawLine(spriteBatch, tempAnchorA2, tempAnchorB2, Color.Black, LineWidth);

            }
        }

        //---> Editor-Funktionalität <---//

        public override void drawInEditor(SpriteBatch spriteBatch)
        {
            if (Circle == null)
            {
                return;
            }
            Color onHover = color;
            if (this.mouseOn) onHover = Constants.onHover;
            color = onHover;
            Circle.drawInEditor(spriteBatch);

            if (Object1 != null)
            {
                tempAnchorA = new Vector2(Object1.position.X, this.position.Y / Level.PixelPerMeter);
                Primitives.Instance.drawLine(spriteBatch, Object1.position, tempAnchorA, color, LineWidth);
            }
            if (Object2 != null)
            {
                tempAnchorB = new Vector2(Object2.position.X, this.position.Y / Level.PixelPerMeter);
                Primitives.Instance.drawLine(spriteBatch, Object2.position, tempAnchorB, color, LineWidth);
            }
        }

        public override string getPrefix()
        {
            return "PulleyJointObject_";
        }

        public override LevelObject clone()
        {
            PulleyJointObject result = (PulleyJointObject)this.MemberwiseClone();
            result.mouseOn = false;
            return result;
        }

        public override bool contains(Vector2 worldPosition)
        {
            return Circle.contains(new Vector2((int)worldPosition.X, (int)worldPosition.Y));
        }

        public override void drawSelectionFrame(SpriteBatch spriteBatch, Matrix matrix)
        {
            Rectangle rect = new Rectangle((int)(Circle.position.X - Circle.radius), (int)(Circle.position.Y - Circle.radius), 2*(int)Circle.radius, 2*(int)Circle.radius);
            Primitives.Instance.drawBox(spriteBatch, rect, Color.Yellow, 2);

            Vector2[] poly = rect.ToPolygon();

            foreach (Vector2 p in poly)
            {
                Primitives.Instance.drawCircleFilled(spriteBatch, p, 4, Color.Yellow);
            }
        }
    }
    #endregion
    #region DistanceJoint
    [Serializable]
    public class DistanceJointObject : JointObject
    {

        private Vector2 _anchorA;
        [DisplayName("Anchor A"), Category("Joint Data")]
        [Description("Vector for the offset from Object 1's center.")]
        public Vector2 AnchorA
        {
            get { return _anchorA; }
            set { _anchorA = value; }
        }

        private Vector2 _anchorB;
        [DisplayName("Anchor B"), Category("Joint Data")]
        [Description("Vector for the offset from Object 2's center.")]
        public Vector2 AnchorB
        {
            get { return _anchorB; }
            set { _anchorB = value; }
        }

        private float _dampingRatio;
        [DisplayName("Damping"), Category("Joint Data")]
        [Description("Damping between 0 and 1.")]
        public float DampingRatio
        {
            get { return _dampingRatio; }
            set { _dampingRatio = value; }
        }

        private float _frequencyHz;
        [DisplayName("Frequency"), Category("Joint Data")]
        [Description("Only change if you know what you do... JULIUS!")]
        public float FrequencyHz
        {
            get { return _frequencyHz; }
            set { _frequencyHz = value; }
        }

        public DistanceJointObject()
        {
            color = Constants.ColorPrimitives;
            AnchorA = Vector2.Zero;
            AnchorB = Vector2.Zero;
            FrequencyHz = 1.0f;
            DampingRatio = 0.5f;
        }

        public override void Initialise()
        {
            Circle = new CirclePrimitiveObject(position, JointObject.radius);
        }

        public override void LoadContent()
        {
        }

        public override void loadContentInEditor(GraphicsDevice graphics) { }
        public override void Update(GameTime gameTime)
        {
            if (Joint == null && Object1 != null && Object2 != null)
            {
                Joint = JointFactory.CreateDistanceJoint(Level.Physics, Object1.fixture.Body, Object2.fixture.Body, AnchorA, AnchorB);
                ((DistanceJoint)Joint).DampingRatio = DampingRatio;
                ((DistanceJoint)Joint).CollideConnected = true;
                ((DistanceJoint)Joint).Frequency = FrequencyHz;
            }

            Circle.position = position;
        }

        public override void transformed()
        {
            base.transformed();

            if (Circle == null)
            {
                return;
            }

            Circle.position = position;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }


        //---> Editor-Funktionalität <---//

        public override void drawInEditor(SpriteBatch spriteBatch)
        {
            if (Circle == null)
            {
                return;
            }
            Color onHover = color;
            if (this.mouseOn) onHover = Constants.onHover;
            color = onHover;
            Circle.drawInEditor(spriteBatch);

            if (Object1 != null)
            {
                Primitives.Instance.drawLine(spriteBatch, Object1.position, Circle.position, color, 5);
            }
            if (Object2 != null)
            {
                Primitives.Instance.drawLine(spriteBatch, Object2.position, Circle.position, color, 5);
            }
            
        }

        public override string getPrefix()
        {
            return "DistanceJointObject_";
        }

        public override LevelObject clone()
        {
            DistanceJointObject result = (DistanceJointObject)this.MemberwiseClone();
            result.mouseOn = false;
            return result;
        }

        public override bool contains(Vector2 worldPosition)
        {
            return Circle.contains(new Vector2((int)worldPosition.X, (int)worldPosition.Y));
        }

        public override void drawSelectionFrame(SpriteBatch spriteBatch, Matrix matrix)
        {
            Rectangle rect = new Rectangle((int)(Circle.position.X - Circle.radius), (int)(Circle.position.Y - Circle.radius), 2*(int)Circle.radius, 2*(int)Circle.radius);
            Primitives.Instance.drawBox(spriteBatch, rect, Color.Yellow, 2);

            Vector2[] poly = rect.ToPolygon();

            foreach (Vector2 p in poly)
            {
                Primitives.Instance.drawCircleFilled(spriteBatch, p, 4, Color.Yellow);
            }
        }
    }
    #endregion
    #region PrismaticJoint
    [Serializable]
    public class PrismaticJointObject : JointObject
    {

        private Vector2 _anchorB;
        [DisplayName("Anchor B"), Category("Joint Data")]
        [Description("Vector for the offset from Object 2's center.")]
        public Vector2 AnchorB
        {
            get { return _anchorB; }
            set { _anchorB = value; }
        }

        private float _minTranslation;
        public float MinTranslation
        {
            get { return _minTranslation; }
            set { _minTranslation = value; }
        }

        private float _maxTranslation;
        public float MaxTranslation
        {
            get { return _maxTranslation; }
            set { _maxTranslation = value; }
        }

        private bool _enableLimit;
        public bool EnableLimit
        {
            get { return _enableLimit; }
            set { _enableLimit = value; }
        }

        private bool _enableMotor;
        public bool EnableMotor
        {
            get { return _enableMotor; }
            set { _enableMotor = value; }
        }

        private float _motorSpeed;
        public float MotorSpeed
        {
            get { return _motorSpeed; }
            set { _motorSpeed = value; }
        }

        public PrismaticJointObject()
        {
            color = Constants.ColorPrimitives;
            MotorSpeed = 0.5f;
            EnableMotor = false;
            MinTranslation = -5;
            MaxTranslation = 5;
            EnableLimit = true;
        }

        public override void Initialise()
        {
            Circle = new CirclePrimitiveObject(position, JointObject.radius);
        }

        public override void LoadContent()
        {
        }

        public override void loadContentInEditor(GraphicsDevice graphics) { }
        public override void Update(GameTime gameTime)
        {
            if (Joint == null && Object1 != null && Object2 != null)
            {
                //Object1.bodyType = BodyType.Static;
                Vector2 axis = Object1.fixture.Body.WorldCenter - Object2.fixture.Body.WorldCenter;
                axis.Normalize();
                Joint = JointFactory.CreatePrismaticJoint(Level.Physics, Object1.fixture.Body, Object2.fixture.Body, AnchorB, axis);
                ((PrismaticJoint)Joint).LowerLimit = MinTranslation;
                ((PrismaticJoint)Joint).UpperLimit = MaxTranslation;
                ((PrismaticJoint)Joint).LimitEnabled = EnableLimit;
                ((PrismaticJoint)Joint).MotorEnabled = EnableMotor;
                ((PrismaticJoint)Joint).MotorSpeed = MotorSpeed;
            }
        }
        public override void transformed()
        {
            base.transformed();

            if (Circle == null)
            {
                return;
            }

            Circle.position = position;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }

        //---> Editor-Funktionalität <---//

        public override void drawInEditor(SpriteBatch spriteBatch)
        {
            if (Circle == null)
            {
                return;
            }
            Color onHover = color;
            if (this.mouseOn) onHover = Constants.onHover;
            color = onHover;
            Circle.drawInEditor(spriteBatch);

            if (Object1 != null)
            {
                Primitives.Instance.drawLine(spriteBatch, Object1.position, Circle.position, color, 5);
            }
            if (Object2 != null)
            {
                Primitives.Instance.drawLine(spriteBatch, Object2.position, Circle.position, color, 5);
            }
        }

        public override string getPrefix()
        {
            return "PrismaticJointObject_";
        }

        public override LevelObject clone()
        {
            PrismaticJointObject result = (PrismaticJointObject)this.MemberwiseClone();
            result.mouseOn = false;
            return result;
        }

        public override bool contains(Vector2 worldPosition)
        {
            return Circle.contains(new Vector2((int)worldPosition.X, (int)worldPosition.Y));
        }

        public override void drawSelectionFrame(SpriteBatch spriteBatch, Matrix matrix)
        {
            Rectangle rect = new Rectangle((int)(Circle.position.X - Circle.radius), (int)(Circle.position.Y - Circle.radius), 2*(int)Circle.radius, 2*(int)Circle.radius);
            Primitives.Instance.drawBox(spriteBatch, rect, Color.Yellow, 2);

            Vector2[] poly = rect.ToPolygon();

            foreach (Vector2 p in poly)
            {
                Primitives.Instance.drawCircleFilled(spriteBatch, p, 4, Color.Yellow);
            }
        }
    }
    #endregion
    #region GearJoint
    [Serializable]
    public class GearJointObject : JointObject
    {
        private float _ratio;
        public float Ratio
        {
            get { return _ratio; }
            set { _ratio = value; }
        }

        private JointObject _joint1;
        public JointObject Joint1
        {
            get { return _joint1; }
            set { _joint1 = value; }
        }

        private JointObject _joint2;
        public JointObject Joint2
        {
            get { return _joint2; }
            set { _joint2 = value; }
        }

        public GearJointObject()
        {
            color = Constants.ColorPrimitives;
            Ratio = 1.0f;
            
        }

        public override void Initialise()
        {
            Circle = new CirclePrimitiveObject(position, JointObject.radius);
        }

        public override void LoadContent()
        {
        }

        public override void loadContentInEditor(GraphicsDevice graphics) { }
        public override void Update(GameTime gameTime)
        {
            if (Joint == null)
            {
                try
                {
                    // A GearJoint can only have two revolutes or a revolute and a prismatic joint
                    if (Joint1.GetType() == typeof(PrismaticJointObject))
                    {
                        if (Joint2.GetType() == typeof(RevoluteJointObject))
                        {
                            Joint = JointFactory.CreateGearJoint(Level.Physics, Joint1.Joint, Joint2.Joint, Ratio);
                        }
                    }
                    else if (Joint1.GetType() == typeof(RevoluteJointObject))
                    {
                        if (Joint2.GetType() == typeof(RevoluteJointObject) || Joint2.GetType() == typeof(PrismaticJointObject))
                        {
                            Joint = JointFactory.CreateGearJoint(Level.Physics, Joint1.Joint, Joint2.Joint, Ratio);
                        }
                    }
                }
                catch (Exception e)
                {
                    #if DEBUG
                        Console.WriteLine("OMG - Wasn't able to create Gear Joint.");
                    #endif
                }

            }

            Circle.position = position;
        }

        public override void transformed()
        {
            base.transformed();

            if (Circle == null)
            {
                return;
            }

            Circle.position = position;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }

        //---> Editor-Funktionalität <---//

        public override void drawInEditor(SpriteBatch spriteBatch)
        {
            if (Circle == null)
            {
                return;
            }
            Color onHover = color;
            if (this.mouseOn) onHover = Constants.onHover;
            color = onHover;
            Circle.drawInEditor(spriteBatch);

            if (Object1 != null)
            {
                Primitives.Instance.drawLine(spriteBatch, Object1.position, Circle.position, color, 5);
            }
            if (Object2 != null)
            {
                Primitives.Instance.drawLine(spriteBatch, Object2.position, Circle.position, color, 5);
            }
        }

        public override string getPrefix()
        {
            return "GearJointObject_";
        }

        public override LevelObject clone()
        {
            GearJointObject result = (GearJointObject)this.MemberwiseClone();
            result.mouseOn = false;
            return result;
        }

        public override bool contains(Vector2 worldPosition)
        {
            return Circle.contains(new Vector2((int)worldPosition.X, (int)worldPosition.Y));
        }

        public override void drawSelectionFrame(SpriteBatch spriteBatch, Matrix matrix)
        {
            Rectangle rect = new Rectangle((int)(Circle.position.X - Circle.radius), (int)(Circle.position.Y - Circle.radius), 2*(int)Circle.radius, 2*(int)Circle.radius);
            Primitives.Instance.drawBox(spriteBatch, rect, Color.Yellow, 2);

            Vector2[] poly = rect.ToPolygon();

            foreach (Vector2 p in poly)
            {
                Primitives.Instance.drawCircleFilled(spriteBatch, p, 4, Color.Yellow);
            }
        }
    }
    #endregion
    #region RopeJoint
    [Serializable]
    public class RopeJointObject : JointObject
    {

        private int _segments;
        public int Segments
        {
            get { return _segments; }
            set { _segments = value; }
        }

        private int _length;
        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }

        private int _width;
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        [NonSerialized]
        private FarseerPhysics.Common.Path _path;
        public FarseerPhysics.Common.Path Path
        {
            get { return _path; }
            set { _path = value; }
        }
        [NonSerialized]
        private List<Body> _bodies;
        public List<Body> Bodies
        {
            get { return _bodies; }
            set { _bodies = value; }
        }
        [NonSerialized]
        private List<Shape> _shapes;
        public List<Shape> Shapes
        {
            get { return _shapes; }
            set { _shapes = value; }
        }


        public override void Initialise()
        {
            Circle = new CirclePrimitiveObject(position, JointObject.radius);
            Length = 200;
            Segments = 20;
            Width = 15;
        }

        public override void LoadContent()
        {
        }

        public override void loadContentInEditor(GraphicsDevice graphics) { }
        public override void Update(GameTime gameTime)
        {
            if (Path == null)
            {
                Path = new FarseerPhysics.Common.Path();
                Path.Add(position/Level.PixelPerMeter);
                Path.Add(new Vector2(position.X, position.Y + Length) / Level.PixelPerMeter);
                Path.Closed = false;

                float rectLength = (Length / Segments) / Level.PixelPerMeter;
                float rectWidth = (Width) / Level.PixelPerMeter;

                List<Shape> shapes = new List<Shape>(1);
                //shapes.Add(new PolygonShape(PolygonTools.CreateRectangle(Width/Level.PixelPerMeter, (Length/Segments)/Level.PixelPerMeter, new Vector2(0, 0), 0f), 1));
                shapes.Add(new PolygonShape(PolygonTools.CreateRectangle(rectWidth, rectLength, new Vector2(rectWidth, rectLength), 0f), 1));
                Bodies = PathManager.EvenlyDistributeShapesAlongPath(Level.Physics, Path, shapes, BodyType.Dynamic, Segments);
                

                if (Object1 == null)
                {
                    Bodies.ElementAt(0).BodyType = BodyType.Static;
                }
                else
                {
                    WeldJoint joint = JointFactory.CreateWeldJoint(Level.Physics, Object1.fixture.Body, Bodies.ElementAt(0), Vector2.Zero, Vector2.Zero);
                    foreach (Body body in Bodies)
                    {
                        foreach(Fixture fix in body.FixtureList)
                        {
                            fix.IgnoreCollisionWith(Object1.fixture);
                        }
                    }
                }
                if (Object2 != null)
                {
                    WeldJoint joint = JointFactory.CreateWeldJoint(Level.Physics, Object2.fixture.Body, Bodies.ElementAt(Bodies.Count - 1), Vector2.Zero, Vector2.Zero);
                    foreach (Body body in Bodies)
                    {
                        foreach (Fixture fix in body.FixtureList)
                        {
                            fix.IgnoreCollisionWith(Object2.fixture);
                        }
                    }
                }

                PathManager.AttachBodiesWithRevoluteJoint(Level.Physics, Bodies, new Vector2(0, 0.25f), new Vector2(0, -0.25f), false, false);
            }


            Circle.position = position;
        }

        public override void transformed()
        {
            base.transformed();
            if (Circle == null)
            {
                return;
            }
            Circle.position = position;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            List<Vector2> centers = new List<Vector2>();

            foreach (Body body in Bodies)
            {
                centers.Add(body.WorldCenter*Level.PixelPerMeter);
            }
            
            Primitives.Instance.drawRope(spriteBatch, centers.ToArray(), Color.Black,(int)Width);
        }

        //---> Editor-Funktionalität <---//

        public override void drawInEditor(SpriteBatch spriteBatch)
        {
            if (Circle == null)
            {
                return;
            }
            Color onHover = color;
            if (this.mouseOn) onHover = Constants.onHover;
            color = onHover;
            Circle.drawInEditor(spriteBatch);

            if (Object1 != null)
            {
                Primitives.Instance.drawLine(spriteBatch, Object1.position, Circle.position, color, 5);
            }
            if (Object2 != null)
            {
                Primitives.Instance.drawLine(spriteBatch, Object2.position, Circle.position, color, 5);
            }
        }

        public override string getPrefix()
        {
            return "RopeJointObject_";
        }

        public override LevelObject clone()
        {
            RopeJointObject result = (RopeJointObject)this.MemberwiseClone();
            result.mouseOn = false;
            return result;
        }

        public override bool contains(Vector2 worldPosition)
        {
            return Circle.contains(new Vector2((int)worldPosition.X, (int)worldPosition.Y));
        }

        public override void drawSelectionFrame(SpriteBatch spriteBatch, Matrix matrix)
        {
            Rectangle rect = new Rectangle((int)(Circle.position.X - Circle.radius), (int)(Circle.position.Y - Circle.radius), 2*(int)Circle.radius, 2*(int)Circle.radius);
            Primitives.Instance.drawBox(spriteBatch, rect, Color.Yellow, 2);

            Vector2[] poly = rect.ToPolygon();

            foreach (Vector2 p in poly)
            {
                Primitives.Instance.drawCircleFilled(spriteBatch, p, 4, Color.Yellow);
            }
        }
    }

    
    #endregion
}
