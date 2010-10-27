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
    [Serializable]
    public class LevelSettings
    {
        /* Sascha:
         * Speichert die Voreinstellungen eines Levels, wie Gravitationsstärke oder Grundfarbton. Lädt beim Start des Levels aus einer XML-Datei
         * die Einstellungen und setzt sie um. Die XML-Datei kann man entweder selbst schreiben oder den Editor benutzen.
        */ 
        private Vector2 _gravitation = new Vector2(0.0f, 9.8f);

        public Vector2 gravitation { get { return _gravitation; } set { _gravitation = value; } }

        private Color _backgroundColor = Color.Black;
        public Color backgroundColor { get { return _backgroundColor; } set { _backgroundColor = value; } }

        private LevelSettings() {}

        private static LevelSettings _instance;     //Sascha: Singleton-Pattern
        public static LevelSettings Default { get {return _instance; } }

        public static void Initialise()
        {
            _instance = new LevelSettings();
        }
    }
}
