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

            public int resolutionWidth 
            { 
                get 
                {
                    if (_resolutionWidth > 1280)
                        return _resolutionWidth;
                    else
                        return MinResolutionWidth;
                } 
                set { _resolutionWidth = value; } 
            }
            public int resolutionHeight 
            { 
                get 
                {
                    if (_resolutionHeight > 768)
                        return _resolutionHeight;
                    else
                        return MinResolutionHeight;
                } 
                set { _resolutionHeight = value; } 
            }

            private bool _fullscreen = false;
            public bool fullscreen { get { return _fullscreen; } set { _fullscreen = value; } }

            private float _soundVolume = 1.0f;
            private float _musicVolume = 1.0f;

            public float soundVolume { get { return _soundVolume; } set { _soundVolume = value; } }
            public float musicVolume { get { return _musicVolume; } set { _musicVolume = value; } }

        #endregion

        private const string GameSettingsFilename = "GameSettings.xml";
        private static GameSettings _instance;
        public static GameSettings Default { get { return _instance; } }

        private GameSettings() { }

        public static void Initialise()
        { 
            _instance = new GameSettings();
            LoadSettings();
        }

        public static void ApplyChanges(ref GraphicsDeviceManager graphics)
        {
            graphics.PreferredBackBufferWidth = Default.resolutionWidth;
            graphics.PreferredBackBufferHeight = Default.resolutionHeight;
            graphics.IsFullScreen = Default.fullscreen;
            graphics.ApplyChanges();
        }

        public static void LoadSettings()
        { 
            
        }

        public static void SaveSettings()
        { 
        
        }
    }
}
