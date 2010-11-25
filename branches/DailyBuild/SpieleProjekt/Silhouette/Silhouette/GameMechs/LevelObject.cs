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
        public string name;

        [NonSerialized]
        private Vector2 _position;
        public Vector2 position { get { return _position; } set { _position = value; } }

        [NonSerialized]
        private float _scale;
        public float Scale { get { return _scale; } set { _scale = value; } }

        [NonSerialized]
        private float _rotation;
        public float Rotation { get { return _rotation; } set { _rotation = value; } }

        public LevelObject() { }

        public abstract void Initialise();
        public abstract void LoadContent();
        public abstract void Update(GameTime gameTime);

        public abstract string getPrefix();
        public abstract bool contains(Vector2 worldPosition);
        public abstract void drawSelectionFrame();
    }
}
