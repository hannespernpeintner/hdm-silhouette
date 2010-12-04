using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IrrKlang;

namespace Silhouette.Engine.SoundEngine
{   [Serializable]
     static class IrrAudioEngine
    {
        static IrrKlang.ISoundEngine engine;

        public static float volume { get { return engine.SoundVolume; } set { engine.SoundVolume = value; } }
        public static void initialize() 
        {
            engine = new IrrKlang.ISoundEngine();
        }

         public static IrrKlang.ISound play(String path, Boolean looped, Boolean startPaused)
         {
             
          return (ISound)engine.Play2D(path, looped, startPaused);

          
         }
         public static void stopAllSounds()
         {
             engine.StopAllSounds();
         }

         public static void pauseAllSounds()
         {
             engine.SetAllSoundsPaused(true);
         }

         public static void unpauseAllSounds()
         {
             engine.SetAllSoundsPaused(false);
         }


      
    }
}
