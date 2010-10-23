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


namespace Silhouette
{
    [Serializable]
    public class GameSettings
    {
        # region Definitions

            private int _resolutionWidth = 0;
            private int _resolutionHeight = 0;
        
            public const int MinResolutionWidth = 1280;
            public const int MinResolutionHeight = 768;

            public int resolutionWidth { get; set; }
            public int resolutionHeight { get; set; }

            private bool _fullscreen;
            public bool fullcreen { get; set; }

            private float _soundVolume = 1.0f;
            private float _musicVolume = 1.0f;

            public float soundVolume { get; set; }
            public float musicVolume { get; set; }

        #endregion

        private const string GameSettingsFilename = "GameSettings.xml";
        private static GameSettings _instance;
        public static GameSettings instance { get { return _instance; } }

        private GameSettings() { }

        public static void initialise()
        { 
            _instance = new GameSettings();
        }

        public static void LoadSettings()
        { 
            
        }

        public static void SaveSettings()
        { 
        
        }
    }
}
