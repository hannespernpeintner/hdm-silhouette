﻿using System;
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
                Object1.bodyType = BodyType.Static;
                Joint = JointFactory.CreateRevoluteJoint(Level.Physics, Object1.fixture.Body, Object2.fixture.Body, Vector2.Zero);
                ((RevoluteJoint)Joint).LowerLimit = MinAngle;
                ((RevoluteJoint)Joint).UpperLimit = MaxAngle;
                ((RevoluteJoint)Joint).LimitEnabled = EnableLimit;
                ((RevoluteJoint)Joint).MotorSpeed = MotorSpeed;
                ((RevoluteJoint)Joint).MotorEnabled = EnableMotor;
            }
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
            Rectangle rect = new Rectangle((int)(Circle.position.X - Circle.radius), (int)(Circle.position.Y - Circle.radius), (int)Circle.radius, (int)Circle.radius);
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
            AnchorA = Vector2.Zero;
            AnchorB = Vector2.Zero;
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
                /*Object1.bodyType = BodyType.Dynamic;
                if (Object1.fixtures != null)
                    Object1.fixtures[0].Body.BodyType = BodyType.Dynamic;
                Object2.bodyType = BodyType.Dynamic;
                if (Object2.fixtures != null)
                    Object2.fixtures[0].Body.BodyType = BodyType.Dynamic;*/

                Vector2 anchorA = new Vector2(Object1.fixture.Body.WorldCenter.X, this.position.Y / Level.PixelPerMeter);
                Vector2 anchorB = new Vector2(Object2.fixture.Body.WorldCenter.X, this.position.Y / Level.PixelPerMeter);
                Joint = JointFactory.CreatePulleyJoint(Level.Physics, Object1.fixture.Body, Object2.fixture.Body, anchorA, anchorB, AnchorA, AnchorB, Ratio);
                Joint.CollideConnected = true;
            }
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
            Rectangle rect = new Rectangle((int)(Circle.position.X - Circle.radius), (int)(Circle.position.Y - Circle.radius), (int)Circle.radius, (int)Circle.radius);
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
            if (Joint == null)
            {
                Joint = JointFactory.CreateDistanceJoint(Level.Physics, Object1.fixture.Body, Object2.fixture.Body, AnchorA, AnchorB);
                ((DistanceJoint)Joint).DampingRatio = DampingRatio;
                ((DistanceJoint)Joint).CollideConnected = true;
                ((DistanceJoint)Joint).Frequency = FrequencyHz;
            }
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
            Rectangle rect = new Rectangle((int)(Circle.position.X - Circle.radius), (int)(Circle.position.Y - Circle.radius), (int)Circle.radius, (int)Circle.radius);
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
            if (Joint == null)
            {
                Object1.bodyType = BodyType.Static;
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
            Rectangle rect = new Rectangle((int)(Circle.position.X - Circle.radius), (int)(Circle.position.Y - Circle.radius), (int)Circle.radius, (int)Circle.radius);
            Primitives.Instance.drawBox(spriteBatch, rect, Color.Yellow, 2);

            Vector2[] poly = rect.ToPolygon();

            foreach (Vector2 p in poly)
            {
                Primitives.Instance.drawCircleFilled(spriteBatch, p, 4, Color.Yellow);
            }
        }
    }
    #endregion
    #region GearRevRevJoint
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
                Joint = JointFactory.CreateGearJoint(Level.Physics, Joint1.Joint, Joint2.Joint, Ratio);
            }
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
            Rectangle rect = new Rectangle((int)(Circle.position.X - Circle.radius), (int)(Circle.position.Y - Circle.radius), (int)Circle.radius, (int)Circle.radius);
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
