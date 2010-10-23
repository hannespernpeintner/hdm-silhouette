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


namespace Silhouette.Engine
{
    public class LevelSettings
    {
        private Vector2 _gravitation;

        public Vector2 gravitation { get { return _gravitation; } set { _gravitation = value; } }

        private LevelSettings() {}

        private static LevelSettings _instance;
        public static LevelSettings instance { get {return _instance; } }

        public static void initialise()
        {
            _instance = new LevelSettings();
        }
    }
}
