using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel;
using Silhouette.Engine;

//Physik-Engine Klassen
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;

namespace Silhouette.GameMechs
{
    [Serializable]
    public abstract class LevelObject
    {
        private string _name;
        [DisplayName("Name"), Category("General")]
        [Description("The name of the object.")]
        public string name { get { return _name; } set { _name = value; } }

        private Vector2 _position;
        [DisplayName("Position"), Category("General")]
        [Description("The object's position in the world.")]
        public Vector2 position { get { return _position; } set { _position = value; transformed(); } }

        public Layer layer;

        public bool isVisible = true;

        [NonSerialized]
        public bool mouseOn;

        public LevelObject() { }

        public abstract void Initialise();
        public abstract void LoadContent();
        public abstract void Update(GameTime gameTime);

        //Editor-Methoden

        public virtual string getPrefix() { return "LevelObject_"; }
        public virtual bool canScale() { return false; }
        public virtual Vector2 getScale() { return Vector2.One; }
        public virtual void setScale(Vector2 scale) { }
        public virtual bool canRotate() { return false; }
        public virtual float getRotation() { return 0; }
        public virtual void setRotation(float rotate) { }
        public virtual bool contains(Vector2 worldPosition) { return false; }
        public virtual void transformed() { }
        public virtual LevelObject clone() { return null; }
        public virtual void drawSelectionFrame(SpriteBatch spriteBatch, Matrix matrix) { }
    }
}
