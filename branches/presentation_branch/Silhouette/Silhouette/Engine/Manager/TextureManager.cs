using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Silhouette.Engine.Manager;
using System.ComponentModel;
using Microsoft.Xna.Framework;

namespace Silhouette.Engine.Manager
{
    public class TextureManager
    {
        /* Sascha:
         * Diese Klasse ist zuständig das Laden und Speichern der Texturen. Sie stellt zum einen Funktionen zum Laden bereit und prüft gleichzeitig ob
         * die Textur schonmal geladen wurde um mehrfaches Laden der gleichen Textur zu verhindern. Für den Fall gedacht, dass eine im Editor geladene Textur nicht in
         * der Content Pipeline hinterlegt ist. Kostet aber mehr Ladezeit.
        */
        private static TextureManager instance;
        public static TextureManager Instance
        {
            get
            {
                if (instance == null) instance = new TextureManager();
                return instance;
            }
        }

        Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        Dictionary<string, Color[]> cData = new Dictionary<string, Color[]>();

        public Texture2D LoadFromFile(string filename)
        {
            if (!textures.ContainsKey(filename))
            {
                try
                {
                    FileStream file = FileManager.LoadConfigFile(filename);
                    if (file != null)
                    {
                        textures[filename] = Texture2D.FromStream(GameLoop.gameInstance.GraphicsDevice, file);
                        file.Close();
                    }
                    else
                        return null;
                }
                catch (IOException e)
                {
                    return null;
                }
            }
            return textures[filename];
        }

        public Texture2D LoadFromFile(string filename, GraphicsDevice graphics)
        {
            if (!textures.ContainsKey(filename))
            {
                try
                {
                    FileStream file = FileManager.LoadConfigFile(filename);
                    if (file != null)
                    {
                        textures[filename] = Texture2D.FromStream(graphics, file);
                        if (!cData.ContainsKey(filename) && textures[filename].Width != 1280 && textures[filename].Height != 768)
                        {
                            cData[filename] = new Color[textures[filename].Width * textures[filename].Height];
                            textures[filename].GetData(cData[filename]);
                        }
                        file.Close();
                    }
                    else
                        return null;
                }
                catch (IOException e)
                {
                    return null;
                }
            }
            return textures[filename];
        }

        public Color[] GetCollisionData(string filename)
        { 
            if (cData.ContainsKey(filename))
                return cData[filename];
            else
                return null;
        }

        public void Clear()
        {
            textures.Clear();
        }
    }
}
