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

        [NonSerialized]
        public bool mouseOn;

        public LevelObject() { }

        public abstract void Initialise();
        public abstract void LoadContent();
        public abstract void Update(GameTime gameTime);

        //Editor-Methoden

        public abstract string getPrefix();
        public abstract bool contains(Vector2 worldPosition);
        public abstract void transformed();
        public abstract LevelObject clone();
        public abstract void drawSelectionFrame(SpriteBatch spriteBatch, Matrix matrix);
    }
}
