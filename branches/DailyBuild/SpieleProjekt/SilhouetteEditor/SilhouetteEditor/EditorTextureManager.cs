﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Silhouette.Engine.Manager;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SilhouetteEditor
{
    class EditorTextureManager
    {
        /* Sascha:
         * Diese Klasse ist zuständig das Laden und Speichern der Texturen. Sie stellt zum einen Funktionen zum Laden bereit und prüft gleichzeitig ob
         * die Textur schonmal geladen wurde um mehrfaches Laden der gleichen Textur zu verhindern.
         * 
         * Für den Editor wird ein extra Manager gebraucht, da der Graphicsdevice mit übergeben werden muss für die Methode FromStream aus Texture2D.
        */

        private static EditorTextureManager instance;
        public static EditorTextureManager Instance
        {
            get
            {
                if (instance == null) instance = new EditorTextureManager();
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
                    MessageBox.Show("Error while loading texture! Check if the Resource is in use!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
