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
        public static FileStream LoadConfigFile(string relativePath)
        {
            if (File.Exists(relativePath) == false)
                return null;
            else
                return File.Open(relativePath, FileMode.Open, FileAccess.Read);
        }

        public static FileStream SaveConfigFile(string relativePath)
        {
            return File.Open(relativePath, FileMode.OpenOrCreate, FileAccess.Write);
        }
    }
}
