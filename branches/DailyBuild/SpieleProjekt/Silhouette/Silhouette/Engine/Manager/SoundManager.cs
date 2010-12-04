using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Silhouette.Engine.SoundEngine;


namespace Silhouette.Engine.Manager
{   
    [Serializable]
    public static class SoundManager 

    {
        //Julius: Deklarieren des Delegates für Fader Update
        public delegate void UpdateFaderEventHandler(GameTime gt);
        //Julius: Deklarieren des Events
        public static event UpdateFaderEventHandler UpdateFader;

        //Julius: Zustandsspeicher für Lautstärke
        static float _fvolume = 1.0f;

        public static void initialize() 
        {
            IrrAudioEngine.initialize();
        }

        public static void update(GameTime gameTime)
        {
            //Julius: Schmeiss das Event!
        if (UpdateFader != null) 
            UpdateFader(gameTime);
        
        }

        public static void muteAll()
        {
            _fvolume = Engine.SoundEngine.IrrAudioEngine.volume;
            IrrAudioEngine.volume = 0;
        }

        public static void unmuteAll()
        {
            IrrAudioEngine.volume = _fvolume;
        }

        public static void pauseAllSounds()
        {
            IrrAudioEngine.pauseAllSounds();
        }

        public static void unpauseAllSounds()
        {
            IrrAudioEngine.unpauseAllSounds();
        }

        public static void stopAllSounsd()
        {
            IrrAudioEngine.stopAllSounds();
        }
    }
}
