using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Silhouette;
using Silhouette.Engine;
using Silhouette.Engine.Manager;
using Silhouette.GameMechs;
using SilhouetteEditor.Forms;
using System.IO;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SilhouetteEditor
{
    public class TextureWrapper
    {
        public string fullPath;
        public Texture2D texture;
        public int width, height;

        public TextureWrapper(string fullPath, int width, int height)
        {
            this.fullPath = fullPath;
            this.width = width;
            this.height = height;
            texture = Texture2DLoader.Instance.LoadFromFile(fullPath);
        }

        public string getPrefix()
        {
            string s = "Texture_";
            return s;
        }
    }
}


