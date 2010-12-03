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

namespace Silhouette.GameMechs
{
    public partial class Deco
    {
        public override string getPrefix()
        {
            return "Deco_";
        }

        public override LevelObject clone()
        {
            throw new NotImplementedException();
        }

        public override void transformed()
        {
            throw new NotImplementedException();
        }

        public override void loadContentInEditor(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphics) { }

        public override bool contains(Microsoft.Xna.Framework.Vector2 worldPosition)
        {
            throw new NotImplementedException();
        }

        public override void drawInEditor(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public override void drawSelectionFrame(SpriteBatch spriteBatch, Matrix matrix)
        {
            throw new NotImplementedException();
        }
    }
}
