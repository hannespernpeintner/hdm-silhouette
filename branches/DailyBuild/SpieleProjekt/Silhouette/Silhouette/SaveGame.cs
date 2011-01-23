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
    public class SaveGame
    {
        public string levelToContinue;

        private const string gameSaveFileName = "SaveGame.xml";

        private static SaveGame _instance;
        public static SaveGame Default
        {
            get
            {
                return _instance;
            }
        }

        private SaveGame() { }

        public static void LoadSaveGame()
        {
            FileStream file = FileManager.LoadConfigFile(gameSaveFileName); //Sascha: Verwendung des FileManagers um die XML-Datei zu laden

            SaveGame loadedGameSave = (SaveGame)new XmlSerializer(typeof(SaveGame)).Deserialize(file); //Sascha: Deserialisierung
            if (loadedGameSave != null) //Sascha: Wenn das Objekt erfolgreich deserialisiert wurde, ist die statische Instanz gleich dem deserialisierten Objekt
                _instance = loadedGameSave;
            file.Close();
        }

        public static void saveSaveGame()
        {
            FileStream file = FileManager.SaveConfigFile(gameSaveFileName); //Sascha: Verwendung des FileManagers um die XML-Datei zu laden oder neu zu erstellen
            new XmlSerializer(typeof(SaveGame)).Serialize(file, _instance); //Sascha: Serialisierung
            file.Close();
        }
    }
}
