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

//Physik-Engine Klassen
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;

namespace Silhouette.GameMechs
{
    public class TextureObject : DrawableLevelObject
    {
        [NonSerialized]
        public Texture2D texture;

        public TextureObject(string path)
        {
            this.fullPath = path;
            this.assetName = Path.GetFileNameWithoutExtension(path);
        }

        public override void Initialise()
        {
            
        }

        public override void LoadContent()
        {
            texture = GameLoop.gameInstance.Content.Load<Texture2D>(assetName);
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        public override string getPrefix()
        {
            return "TextureObject_";
        }

        public override bool contains(Vector2 worldPosition)
        {
            Rectangle r = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            return r.Contains((int)worldPosition.X, (int)worldPosition.Y);
        }
    }
}
