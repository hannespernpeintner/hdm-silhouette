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
        public DrawableLevelObject() { }
        public abstract void Draw(SpriteBatch spriteBatch);

        //Editor-Methoden

        public virtual void loadContentInEditor(GraphicsDevice graphics) { }
        public virtual void drawInEditor(SpriteBatch spriteBatch) { }
    }
}
