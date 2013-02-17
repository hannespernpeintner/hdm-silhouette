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
        /* Sascha:
         * Diese Klasse speichert alle wichtigen Informationen, die für die Engine wichtig sind, z.B. Auflösung, Grafikeinstellungen etc.
         * Sie wurde so realisiert, dass sie beim Spielstart die Einstellungen aus GameSettings.xml liest und ausführt. Sollte während dem
         * Spiel was geändert werden, wird es ausgeführt und für den nächsten Spielstart gespeichert. Dadurch ist es möglich die Einstellungen
         * auch rein über XML außerhalb des Spiels festzusetzen.
        */

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

            private float _gameCamScale = 0.4f;
            public float gameCamScale
            {
                get { return _gameCamScale; }
                set
                {
                    if (_gameCamScale != value)
                        Changed = true;
                    _gameCamScale = value;
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

        private static bool Changed = false;                                //Sascha: Statische Variable die prüft, ob etwas geändert wurde -> Speichern notwendig
        private const string GameSettingsFilename = "GameSettings.xml";     //Sascha: Relativer Pfad zur Datei, in der die Settings gespeichert werden
        private static GameSettings _instance;                              //Sascha: Singleton-Pattern
        public static GameSettings Default 
        { 
            get 
            {
                return _instance; 
            } 
        }

        private GameSettings() { }

        public static void Initialise()
        {
            _instance = new GameSettings();
            LoadSettings();                     //Sascha: Lädt die Daten aus der XML-Datei
        }

        public static void ApplyChanges(ref GraphicsDeviceManager graphics)
        {
            /* Sascha:
             * Methode um die Einstellungen von GameSettings auf das Spiel zu übertragen. Der GDM wird als ref übergeben,
             * damit man auch wirklich mit dem Grafikkontext vom Spielfenster arbeitet.
            */
            graphics.PreferredBackBufferWidth = Default.resolutionWidth;
            graphics.PreferredBackBufferHeight = Default.resolutionHeight;
            graphics.IsFullScreen = Default.fullscreen;
            graphics.ApplyChanges();
        }

        public static void LoadSettings()
        {
            FileStream file = FileManager.LoadConfigFile(GameSettingsFilename); //Sascha: Verwendung des FileManagers um die XML-Datei zu laden

            if (file == null)       //Sascha: Wenn kein File existiert, wird einfach ein neues File mit den Standard-Werten erstellt
            {
                Changed = true;
                SaveSettings();
                return;
            }

            GameSettings loadedGameSettings = (GameSettings)new XmlSerializer(typeof(GameSettings)).Deserialize(file); //Sascha: Deserialisierung
            if (loadedGameSettings != null) //Sascha: Wenn das Objekt erfolgreich deserialisiert wurde, ist die statische Instanz gleich dem deserialisierten Objekt
                _instance = loadedGameSettings;
            file.Close();
        }

        public static void SaveSettings()
        {
            if (!Changed)       //Sascha: Wenn nichts geändert wurde, muss man auch nichts speichern
                return;

            Changed = false;
            FileStream file = FileManager.SaveConfigFile(GameSettingsFilename); //Sascha: Verwendung des FileManagers um die XML-Datei zu laden oder neu zu erstellen
            new XmlSerializer(typeof(GameSettings)).Serialize(file, _instance); //Sascha: Serialisierung
            file.Close();
        }
    }
}
