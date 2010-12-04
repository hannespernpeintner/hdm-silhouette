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
        static Dictionary<String, Silhouette.GameMechs.SoundObject> Container;

        public static void initialize() 
        {
            IrrAudioEngine.initialize();
            Container = new Dictionary<String, Silhouette.GameMechs.SoundObject>();
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

        public static void addSound(String AssetName, String PathToFile)
        {
            Container.Add(AssetName, new GameMechs.SoundObject(PathToFile));
        }

        public static void playSound(String AssetName)
        {
            Container[AssetName].Play();
        }

        public static void pauseSound(String AssetName, Boolean paused)
        {
            Container[AssetName].Pause = paused;
        }

        public static void setVolume(String AssetName, float Volume)
        {
            Container[AssetName].volume = Volume;
        }

        public static void enableEqualizer(String AssetName, float fCenter, float fBandwidth, float fGain)
        {
            Container[AssetName].EnableEqualizer(fCenter, fBandwidth, fGain);
        }

        public static void disableEqualizer(String AssetName)
        {
            Container[AssetName].DisableEqualizer();
        }

        public static void enableReverb(String AssetName, float fInGain, float fReverbMix, float fReverbTime, float fHighFreqRTRatio)
        {
            Container[AssetName].EnableReverb(fInGain, fReverbMix, fReverbTime, fHighFreqRTRatio);
        }

        public static void looped(String AssetName, Boolean looped)
        {
            Container[AssetName].looped = looped;
        }

        public static void fadeUp(String AssetName, float fadeTimeInSeconds)
        {
            Container[AssetName].fadeUp(fadeTimeInSeconds);
        }

        public static void fadeUp(String AssetName, float fadeTimeInSeconds, float VolumeGain)
        {
            Container[AssetName].fadeUp(fadeTimeInSeconds, VolumeGain);
        }

        public static void fadeDown(String AssetName, float fadeTimeInSeconds)
        {
            Container[AssetName].fadeDown(fadeTimeInSeconds);
        }

        public static void fadeDown(String AssetName, float fadeTimeInSeconds, float VolumeLoss)
        {
            Container[AssetName].fadeDown(fadeTimeInSeconds, VolumeLoss);
        }

        public static void crossFade(String AssetName, String SilentSoundAssetName, float GainSilentSound, float LossCurrentSound, float FadeTimeInSeconds)
        {
            Container[AssetName].Crossfade(Container[SilentSoundAssetName], GainSilentSound, LossCurrentSound, FadeTimeInSeconds);
        }

        public static void stop(String AssetName)
        {
            Container[AssetName].Stop();
        }

        public static void disableAllFXonSound(String AssetName)
        {
            Container[AssetName].DisableAllFX();
        }
    }

}
