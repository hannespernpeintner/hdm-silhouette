using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;


namespace Silhouette.Engine.Manager
{
    public static class FileManager
    {
        /* Sascha:
         * Managed den Zugriff auf Dateien. Für jeden Typ Datei gibt es eine eigene Zugriffsfunktion,
         * da wir ein stark spezialisiertes System haben, dass gerade im Hinblick auf den Leveleditor
         * flexibel sein muss.
        */ 
        public static FileStream LoadConfigFile(string relativePath)
        {
            if (!File.Exists(relativePath))
                return null;
            else
                return File.Open(relativePath, FileMode.Open, FileAccess.Read);
        }

        public static FileStream SaveConfigFile(string relativePath)
        {
            return File.Open(relativePath, FileMode.OpenOrCreate, FileAccess.Write);
        }

        public static FileStream LoadLevelFile(string relativeLevelPath, int levelNumber)
        {
            if (!Directory.Exists(relativeLevelPath))
            {
                Directory.CreateDirectory(relativeLevelPath);
            }

            string[] levelFiles = Directory.GetFiles(relativeLevelPath);
            string fullPath = Path.Combine(relativeLevelPath, levelFiles[levelNumber]);

            if (!File.Exists(fullPath))
                return null;
            else
                return File.Open(fullPath, FileMode.Open, FileAccess.Read);

        }
    }
}
