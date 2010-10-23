using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Silhouette.Engine;
using Silhouette.Engine.Manager;


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
                set 
                {
                    if (_resolutionWidth != value)
                        Changed = true;
                    _resolutionWidth = value; 
                } 
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
                set 
                {
                    if (_resolutionHeight != value)
                        Changed = true;
                    _resolutionHeight = value; 
                } 
            }

            private bool _fullscreen = false;
            public bool fullscreen 
            { 
                get { return _fullscreen; } 
                set 
                {
                    if (_fullscreen != value)
                        Changed = true;
                    _fullscreen = value; 
                }
            }

            private float _soundVolume = 1.0f;
            private float _musicVolume = 1.0f;

            public float soundVolume 
            { 
                get { return _soundVolume; } 
                set 
                {
                    if (_soundVolume != value)
                        Changed = true;
                    _soundVolume = value; 
                } 
            }
            public float musicVolume 
            { 
                get { return _musicVolume; } 
                set 
                {
                    if (_musicVolume != value)
                        Changed = true;
                    _musicVolume = value; 
                } 
            }
        #endregion

        private static bool Changed = false;
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
            FileStream file = FileManager.LoadConfigFile(GameSettingsFilename);

            if (file == null)
            {
                Changed = true;
                return;
            }

            GameSettings loadedGameSettings = (GameSettings)new XmlSerializer(typeof(GameSettings)).Deserialize(file);
            if (loadedGameSettings != null)
                _instance = loadedGameSettings;
            file.Close();
        }

        public static void SaveSettings()
        {
            if (!Changed)
                return;

            Changed = false;
            FileStream file = FileManager.SaveConfigFile(GameSettingsFilename);
            new XmlSerializer(typeof(GameSettings)).Serialize(file, _instance);
            file.Close();
        }
    }
}
