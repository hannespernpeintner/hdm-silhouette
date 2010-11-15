﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Silhouette.Engine.Manager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SilhouetteEditor
{
    class Texture2DLoader
    {
        private static Texture2DLoader instance;
        public static Texture2DLoader Instance
        {
            get
            {
                if (instance == null) instance = new Texture2DLoader();
                return instance;
            }
        }

        Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();



        public Texture2D LoadFromFile(string filename)
        {
            if (!textures.ContainsKey(filename))
            {
                try
                {
                    FileStream file = FileManager.LoadConfigFile(filename);
                    if (file != null)
                        textures[filename] = Texture2D.FromStream(EditorLoop.EditorLoopInstance.GraphicsDevice, file);
                    else
                        return null;
                }
                catch (IOException e)
                {
                    MessageBox.Show("Fehler beim Laden der Textur!", "Error");
                    return null;
                }
            }
            return textures[filename];
        }

        public void Clear()
        {
            textures.Clear();
        }

    }
}
