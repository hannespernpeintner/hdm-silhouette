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

namespace Silhouette.GameMechs
{
    [Serializable]
    public abstract class DrawableLevelObject : LevelObject
    {
        [NonSerialized]
        private string _assetName;
        [DisplayName("Filename"), Category("Object Data")]
        [Description("The filename of the attached texture.")]
        public string assetName { get { return _assetName; } set { _assetName = value; } }
        [NonSerialized]
        private string _fullPath;
        [DisplayName("Path"), Category("Object Data")]
        [Description("The full path of the texture.")]
        public string fullPath { get { return _fullPath; } set { _fullPath = value; } }

        public DrawableLevelObject() { }
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
