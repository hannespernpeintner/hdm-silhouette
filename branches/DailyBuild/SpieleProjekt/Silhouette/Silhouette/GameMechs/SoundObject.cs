﻿using System;
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
using Silhouette;
using Silhouette.Engine;
using Silhouette.Engine.Manager;
using Silhouette.GameMechs;
using Silhouette.Engine.SoundEngine;
using System.IO;

using System.ComponentModel;

using IrrKlang;

/*Julius:
 * Diese Klasse repräsentiert ein SoundObjekt. Diese kommunizieren direkt mit der statischen Instanz der irrKlang Engine
 * Der SoundManager wirft ein UpdateEvent, bei dem sich jeder Sound bei Erzeugung registriert.
 * 
 * Zur Beachtung:
 * Soll ein Sound geloopt werden, muss die entsprechedne Property looped gesetzt werden BEVOR play() aufgerufen wird.
 * Selbiges gilt für die paused Property. Gibt auch Sinn. Wenn man aus Performance Gründen doch mal einen Track pausiert starten will
 * (Trennnen von Erzeugung der Objektinstanz und abspielen), der soll bescheid geben. Dann wird das implementiert
 * 
 * Soll ein Sound erneut abgespielt werden, kann play() erneut aufgerufen werden. Allerdings verliert man dann die Referenz für den Sound überschrieben!
 * Lange Rede, kurzer Unsinn: Wenn man play() ein zweites mal aufruft, bevor der Track zuende ist, kann man vom alten Sound Dinge wie Effekte, Lautstärke etc. nicht mehr ändern!
 */
namespace Silhouette.GameMechs
{
    [Serializable]
    public class SoundObject : LevelObject
    {
        private string _assetName;
        [DisplayName("Filename"), Category("Initial Sound Data")]
        [Description("The filename of the attached texture.")]
        public string assetName { get { return _assetName; } set { _assetName = value; } }

        private string _fullPath;
        [DisplayName("Path"), Category("Initial Sound Data")]
        [Description("The full path of the texture.")]
        public string fullPath { get { return _fullPath; } set { _fullPath = value; } }


        private Boolean _looped;
        [DisplayName("Looped"), Category("Initial Sound Data")]
        [Description("Decides wether a Sound is played in an endless loop. This is an initial setting")]
        public Boolean looped { get { return _looped; } set { _looped = value; } }

        private String pathToFile;

        /* Julius: Property für die Pause.
         * ACHTUNG: Funktioniert nur, wenn davor play() aufgerufen worden ist.
          */
        [Browsable(false)]
        public Boolean Pause
        {

            get
            {
                if (Sound != null)
                { return Sound.Paused; }
                else return false;
            }

            set
            {
                if (Sound != null)
                { Sound.Paused = value; }


            }
        }


        [DisplayName("Volume"), Category("Initial Sound Data")]
        [Description("The Sound's inital Volume. Float Value Min/Max: 0.0 / 1.0")]
        public float volume
        {
            get
            {
                if (Sound != null)
                {
                    return Sound.Volume;

                }
                else return _volume;
            }

            set
            {
                if (Sound != null)
                { Sound.Volume = value; _volume = value; }
                else _volume = value;
            }
        }
        [DisplayName("Start Muted"), Category("Initial Sound Data")]
        [Description("Decides wether a Sound is muted or not")]
        public Boolean mute
        {
            get { return _mute; }
            set
            {
                if (value == false)
                {
                    volume = _MuteVolume;
                    _mute = false;
                }
                else
                {
                    _mute = true;
                    _MuteVolume = volume;
                    volume = 0;
                }


            }
        }

        public Boolean isPlaying()
        {
            if (Sound == null)
            {
                return false;
            }
            else
            {
                if (this.Pause == false && Sound.Finished == false)
                    return true;
                else
                    return false;
            }

        }

        private Boolean _mute;
        private float _MuteVolume = 1;
        //Julius: Zustandsspeicher, falls sound noch nicht erzeugt wurde
        float _volume = 1;
        [NonSerialized]
        IrrKlang.ISound Sound;


        //Julius: Zustandsspeicher und Parameter für EQ
        Boolean _EQActivated = false;
        [Browsable(false)]
        public float _EQBandwith { get; set; }
        [Browsable(false)]
        public float _EQCenter { get; set; }
        [Browsable(false)]
        public float _EQGain { get; set; }

        //Julius: Zustandsspeicher sammt Paramter für Reverb
        [DisplayName("Reverb activated"), Category("Initial Sound Data")]
        [Description("Decides wether the reverb effect is activated when the Sound starts for the first time")]
        public Boolean ReverbActivated { get { return _ReverbActivated; } set { _ReverbActivated = value; } }
        Boolean _ReverbActivated = false;

        [DisplayName("Reverb input gain"), Category("Initial Sound Data")]
        [Description("Inital setting: Input gain of signal, in decibels (dB). Min/Max: [-96.0,0.0]")]
        public float RevInGain { get { return _RevInGain; } set { _RevInGain = value; } }
        private float _RevInGain;

        [DisplayName("Reverb mix"), Category("Initial Sound Data")]
        [Description("Reverb mix, in dB. Min/Max: [-96.0,0.0]")]
        public float RevfReverbMix { get { return _RevfReverbMix; } set { _RevfReverbMix = value; } }
        private float _RevfReverbMix;

        [DisplayName("Reverb time"), Category("Initial Sound Data")]
        [Description("Reverb time, in milliseconds. Min/Max: [0.001,3000.0]")]
        public float RevfReverbTime { get { return _RevfReverbTime; } set { _RevfReverbTime = value; } }
        private float _RevfReverbTime;

        [DisplayName("Reverb HF reverb/time ratio"), Category("Initial Sound Data")]
        [Description("High-frequency reverb time ratio. Min/Max: [0.001,0.999]")]
        public float RevfHighFreqRTRatio { get { return _RevfHighFreqRTRatio; } set { _RevfHighFreqRTRatio = value; } }
        private float _RevfHighFreqRTRatio;


        //Julius: Zustandsspeicher & Krempel für Fader

        Boolean _FaderActivated;
        [Browsable(false)]
        public float _fFadeTime { get; set; }

        float _fFadeStep;
        enum FadeType { FadeUp, FadeDown }
        FadeType _eFadeType;

        public SoundObject(String path)
        {
            this.fullPath = path;
            this.assetName = Path.GetFileNameWithoutExtension(path);

            looped = false;

            //Julius: Beim Event registrieren...
            Engine.Manager.SoundManager.UpdateFader += new SoundManager.UpdateFaderEventHandler(Update);

            _volume = 1;

        }


        /*Julius:
         * Allgemeine Anmerkung zu diesen ganzen Fading Geschichten: Alle Lautsärke-Parameter sind 
         * 
         * --->GAIN ANGABEN!<----
         * 
         * Beispiel:
         * 
         * Sound Hat Lautstärke 0.8f
         * Ein Sound.fadeDown(2.5f, 0.5f); verringert die Lautstärke in 2.5 Sekunden um 0.5
         * Damit ist nach 2.5 Sekunden Sound.Volume = 0.3f . 
         * kapiert?
         */
        //Julius: Erhöht um VolumeGain
        public void fadeUp(float FadeTimeInSeconds, float VolumeGain)
        {
            if (VolumeGain != 0)
                _fFadeStep = (float)(VolumeGain / (double)FadeTimeInSeconds);
            else
                _fFadeStep = (float)(1.0f / (double)FadeTimeInSeconds);
            _fFadeTime = FadeTimeInSeconds;
            _eFadeType = FadeType.FadeUp;
            _FaderActivated = true;
        }
        //Julius: Folgende Methode fadet bis maximalAusschlag!
        public void fadeUp(float FadeTimeInSeconds)
        {
            _fFadeStep = (float)(1.0f / (double)FadeTimeInSeconds);
            _fFadeTime = FadeTimeInSeconds;
            _eFadeType = FadeType.FadeUp;
            _FaderActivated = true;
        }
        //Julius: Fadet bis 0
        public void fadeDown(float FadeTimeInSeconds)
        {
            _fFadeStep = (float)(Sound.Volume / (double)FadeTimeInSeconds);
            _fFadeTime = FadeTimeInSeconds;
            _eFadeType = FadeType.FadeDown;
            _FaderActivated = true;
        }
        //Julius: Erniedrigt um Loss
        public void fadeDown(float FadeTimeInSeconds, float Loss)
        {   //Julius: Denkt dran Kinder, immer schön an die Division durch 0 denken!
            if (Loss != 0)
                _fFadeStep = (float)(Loss / (double)FadeTimeInSeconds);
            else
                _fFadeStep = (float)(1.0f / (double)FadeTimeInSeconds);

            _fFadeTime = FadeTimeInSeconds;
            _eFadeType = FadeType.FadeDown;
            _FaderActivated = true;
        }

        /*Julius:
         * Okay, mal zu Erörterung: Diese Methode wird auf dem aktuell abgespielten Sound ( Volume > 0 aufgerufen). Dieses wird leiser gedreht.
         * Das SoundObject SilentSound ist das SoundObject, dass lauter gedreht werden soll. Allerdings darf auch hier Volume != 0 oder Volume = 0 sein.
         * Es gilt:
         * 
         * this.Volume > SilentSound.Volume
         * 
         */
        public void Crossfade(SoundObject SilentSound, float GainSilentSound, float LossCurrentSound, float FadeTimeInSeconds)
        {
            SilentSound.fadeUp(FadeTimeInSeconds, GainSilentSound);
            this.fadeDown(FadeTimeInSeconds, LossCurrentSound);
        }

        public void Play()
        {
            /* Julius:
             This is, where the magic happens: Um dieser verkorksten Architektur aus dem Weg zu gehen, wird jeder Song pausiert gestartet.
             * Auf das beim Starten erzeugte ISound Objekt werden die Effekte angewendet und nachher je nach Bedarf unpaused oder auch nicht.
             */

            if (File.Exists(fullPath))
            {
                pathToFile = fullPath;

            }
            else
            {
                pathToFile = Environment.CurrentDirectory + "\\Content\\Audio\\" + _assetName + ".ogg";
            }

            Sound = IrrAudioEngine.play(pathToFile, looped, true);

            if (_EQActivated)
            {
                Sound.SoundEffectControl.EnableParamEqSoundEffect(_EQCenter, _EQBandwith, _EQGain);
            }

            if (_ReverbActivated)
            {
                Sound.SoundEffectControl.EnableWavesReverbSoundEffect(_RevInGain, _RevfReverbMix, _RevfReverbTime, _RevfHighFreqRTRatio);
            }

            if (mute)
                Sound.Volume = 0;
            else
                Sound.Volume = _volume;


            Sound.Paused = false;


        }

        public void EnableEqualizer(float fCenter, float fBandwidth, float fGain)
        {  //Julius: EQ direkt aktiveren, oder Parameter zwischenspeichern. So einfach ist das.
            if (Sound != null)
            {
                _EQActivated = true;
                Sound.SoundEffectControl.EnableParamEqSoundEffect(fCenter, fBandwidth, fGain);

                _EQBandwith = fBandwidth;
                _EQCenter = fCenter;
                _EQGain = fGain;
            }
            else
            {
                _EQActivated = true;
                _EQBandwith = fBandwidth;
                _EQCenter = fCenter;
                _EQGain = fGain;
            }
        }

        public void DisableEqualizer()
        {
            if (Sound != null)
            {
                Sound.SoundEffectControl.DisableParamEqSoundEffect();
                _EQActivated = false;
            }
            else
                _EQActivated = false;
        }

        public void EnableReverb(float fInGain, float fReverbMix, float fReverbTime, float fHighFreqRTRatio)
        {
            //Julius: Reverb direkt aktiveren, oder Parameter zwischenspeichern. So einfach ist das.
            if (Sound != null)
            {
                Sound.SoundEffectControl.EnableWavesReverbSoundEffect(fInGain, fReverbMix, fReverbTime, fHighFreqRTRatio);
                _ReverbActivated = true;
                _RevInGain = fInGain;
                _RevfReverbMix = fReverbMix;
                _RevfReverbTime = fReverbTime;
                _RevfHighFreqRTRatio = fHighFreqRTRatio;
            }
            else
            {
                _ReverbActivated = true;
                _RevInGain = fInGain;
                _RevfReverbMix = fReverbMix;
                _RevfReverbTime = fReverbTime;
                _RevfHighFreqRTRatio = fHighFreqRTRatio;
            }
        }

        public void DisableReverb()
        {
            if (Sound != null)
            {
                Sound.SoundEffectControl.DisableWavesReverbSoundEffect();
                _ReverbActivated = false;
            }
            else
                _ReverbActivated = false;
        }

        public void DisableAllFX()
        {
            if (Sound != null)
            {
                Sound.SoundEffectControl.DisableAllEffects();
                _EQActivated = false;
                _ReverbActivated = false;
            }
            else
            {
                _EQActivated = false;
                _ReverbActivated = false;
            }
        }

        public void Stop()
        {
            if (Sound != null)
            {
                Sound.Stop();
            }
        }

        public override void Update(GameTime gameTime)
        {
            //Julius: Wir updaten den Fader...
            if ((Sound != null) && (_FaderActivated))
            {
                //Julius: leiser faden
                if (_eFadeType == FadeType.FadeDown)
                {   //Julius: Lautstärke anpassen
                    Sound.Volume -= _fFadeStep * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                //Julius: lauter faden
                if (_eFadeType == FadeType.FadeUp)
                {
                    Sound.Volume += _fFadeStep * (float)gameTime.ElapsedGameTime.TotalSeconds;

                }
                //Julius: restliche FadeZeit mit der GameTime anpassen.
                _fFadeTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                //Julius: Wenn Fadezeit zu ende: Alles auf Anfang!
                if (_fFadeTime <= 0)
                {
                    _fFadeTime = 0;
                    _fFadeStep = 0;
                    _FaderActivated = false;
                }
            }
        }

        public override void Initialise() { SoundManager.addSoundToList(this); }
        public override void LoadContent() { }

        public override string getPrefix() { return "SoundObject_"; }

        public override LevelObject clone()
        {
            SoundObject so = (SoundObject)this.MemberwiseClone();
            return so;
        }
    }


}
