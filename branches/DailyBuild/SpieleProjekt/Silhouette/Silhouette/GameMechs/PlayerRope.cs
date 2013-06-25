using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Common;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Dynamics.Contacts;

using Silhouette.Engine.Manager;
using Silhouette.Engine;

namespace Silhouette.GameMechs
{
    public class PlayerRope : RopeJointObject
    {
        [NonSerialized]
        private Fixture _ropeSensor;
        public Fixture RopeSensor
        {
            get { return _ropeSensor; }
            set { _ropeSensor = value; }
        }

        private WeldJoint _joint1;

        public WeldJoint Joint1
        {
            get { return _joint1; }
            set { _joint1 = value; }
        }
        private WeldJoint _joint2;

        public WeldJoint Joint2
        {
            get { return _joint2; }
            set { _joint2 = value; }
        }

        private WeldJoint _jointOnCollision;
        public WeldJoint JointOnCollision
        {
            get { return _jointOnCollision; }
            set { _jointOnCollision = value; }
        }

        
        public PlayerRope(int mouseX, int mouseY)
        {
            /*
            Vector2 force = new Vector2(GameLoop.gameInstance.GraphicsDevice.Viewport.Width / 2, GameLoop.gameInstance.GraphicsDevice.Viewport.Height / 2) - new Vector2(mouseX, mouseY);
            //force /= 15;
            force.Normalize();
            force *= 10;
            force.Y = -force.Y;

            Circle = new CirclePrimitiveObject(position, JointObject.radius);
            Length = 100;
            Segments = 20;
            Width = 5;


            RopeSensor = FixtureManager.CreateCircle(15, (GameLoop.gameInstance.playerInstance.charRect.Body.Position + new Vector2(-force.X, force.Y) / 1) * Level.PixelPerMeter, BodyType.Dynamic, 1.0f);
            RopeSensor.IsSensor = true;
            RopeSensor.OnCollision += RopeSensorOnCollision;

            Path = new FarseerPhysics.Common.Path();
            Path.Add(GameLoop.gameInstance.playerInstance.charRect.Body.Position );
            //Path.Add(new Vector2(position.X, position.Y + Length) / Level.PixelPerMeter);
            Path.Add((RopeSensor.Body.Position ));
            Path.Closed = false;

            float rectLength = (Length / Segments) / Level.PixelPerMeter;
            float rectWidth = (Width) / Level.PixelPerMeter;

            List<Shape> shapes = new List<Shape>(1);
            //shapes.Add(new PolygonShape(PolygonTools.CreateRectangle(Width/Level.PixelPerMeter, (Length/Segments)/Level.PixelPerMeter, new Vector2(0, 0), 0f), 1));
            shapes.Add(new PolygonShape(PolygonTools.CreateRectangle(rectWidth, rectLength, new Vector2(rectWidth, rectLength)/2, 0f), 1));
            Bodies = new List<Body>();
            Bodies.AddRange(PathManager.EvenlyDistributeShapesAlongPath(Level.Physics, Path, shapes, BodyType.Dynamic, Segments));
            //Bodies.Add(RopeSensor.Body);
            //List<RevoluteJoint> revoluteJoints = PathManager.AttachBodiesWithRevoluteJoint(Level.Physics, Bodies, new Vector2(0, 0.25f), new Vector2(0, -0.25f), false, false);
            List<SliderJoint> sliderJoints = PathManager.AttachBodiesWithSliderJoint(Level.Physics, Bodies, new Vector2(0, 0.25f), new Vector2(0, -0.25f), false, false, 1f, 1f);

            Joint1 = JointFactory.CreateWeldJoint(Level.Physics, GameLoop.gameInstance.playerInstance.charRect.Body, Bodies.ElementAt(0), Vector2.Zero, Vector2.Zero);
            Joint2 = JointFactory.CreateWeldJoint(Level.Physics, RopeSensor.Body, Bodies.ElementAt(Bodies.Count-1), Vector2.Zero, Vector2.Zero);

            foreach (SliderJoint sj in sliderJoints)
            {
                sj.DampingRatio = 0.0f;
            }

            foreach (Body body in Bodies)
            {
                foreach (Fixture fix in body.FixtureList)
                {
                    if (!fix.isPlayer)
                    {
                        fix.IsSensor = true;
                    }
                    fix.IgnoreCollisionWith(GameLoop.gameInstance.playerInstance.charRect);
                    fix.IgnoreCollisionWith(GameLoop.gameInstance.playerInstance.camRect);
                    fix.IgnoreCollisionWith(GameLoop.gameInstance.playerInstance.nRect);
                    fix.IgnoreCollisionWith(GameLoop.gameInstance.playerInstance.eRect);
                    fix.IgnoreCollisionWith(GameLoop.gameInstance.playerInstance.sRect);
                    fix.IgnoreCollisionWith(GameLoop.gameInstance.playerInstance.wRect);
                    fix.IgnoreCollisionWith(RopeSensor);

                }
            }

            

            RopeSensor.Body.ApplyForce(ref force);
            */
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            List<Vector2> centers = new List<Vector2>();

            foreach (Body body in Bodies)
            {
                centers.Add(body.WorldCenter * Level.PixelPerMeter);
            }

            Primitives.Instance.drawRope(spriteBatch, centers.ToArray(), Color.Black, (int)Width);
        }

        public void delete()
        {
            if (Joint1 != null)
            {
                Level.Physics.RemoveJoint(Joint1);
            }
            if (Joint2 != null)
            {
                Level.Physics.RemoveJoint(Joint2);
            }
            //Level.Physics.RemoveBody(RopeSensor.Body);
            foreach(Body body in Bodies)
            {
                /*foreach(Joint joint in Level.Physics.JointList)
                {
                    if((joint.BodyA == body || joint.BodyB == body) && joint is RevoluteJoint)
                    {
                        Level.Physics.RemoveJoint(joint);
                    }
                }*/
                body.Active = false;
                //Level.Physics.RemoveBody(body);
            }
            //Level.Physics.RemoveBody(RopeSensor.Body);
            RopeSensor.Body.Active = false;
        }

        public override void Initialise()
        {
        }
        public override void LoadContent()
        {
        }
        public override void Update(GameTime gameTime)
        {

        }

        public bool RopeSensorOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if ((fixtureB.IsSensor || fixtureB.isEvent) && fixtureB.Body.BodyType == BodyType.Static)
            {
                return true;
            }
            //RopeSensor.Body.BodyType = BodyType.Static;
            if (JointOnCollision != null)
            {
                Level.Physics.RemoveJoint(JointOnCollision);
            }

            JointOnCollision = JointFactory.CreateWeldJoint(Level.Physics, RopeSensor.Body, fixtureB.Body, Vector2.Zero, Vector2.Zero);

            return false;
        }

    }
}
