using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Silhouette.Engine.Manager;
using System.ComponentModel;
using Silhouette.Engine;

//Physik-Engine Klassen
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;

namespace Silhouette.GameMechs.Physics
{
    public abstract class PhJoint : LevelObject
    {
        private InteractiveObject _Body1;
        [DisplayName("First Body"), Category("Joint Data")]
        [Description("The first body the joint is atached to.")]
        public InteractiveObject Body1 { get { return _Body1; } set { _Body1 = value; } }
        private InteractiveObject _Body2;
        [DisplayName("Second Body"), Category("Joint Data")]
        [Description("The second body the joint is atached to.")]
        public InteractiveObject Body2 { get { return _Body2; } set { _Body2 = value; } }

        public abstract void ToFixture();

        public override void Initialise() { }

        public override void LoadContent() { ToFixture(); }

        public override void Update(GameTime gameTime) { }
    }

    public class PhRevoluteJoint : PhJoint
    {
        RevoluteJoint joint;
        Vector2 anchor;

        public PhRevoluteJoint(InteractiveObject b1, InteractiveObject b2, Vector2 anchor) 
        {
            this.Body1 = b1;
            this.Body2 = b2;
            this.anchor = anchor;
        }

        public override void ToFixture()
        {
            joint = JointFactory.CreateRevoluteJoint(Body1.fixture.Body, Body2.fixture.Body, anchor);
        }
    }
}
