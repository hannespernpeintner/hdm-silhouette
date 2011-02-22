using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



using Silhouette.GameMechs;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Silhouette.Engine.Manager
{
    public enum VideoName 
    { 
        None,
        Cutscene_1,
        Cutscene_2
    }

    static public class VideoManager
    {
        //Julius: Deklarieren des Delegates für Frame Update
        public delegate void UpdateFrameEventHandler(GameTime gt);
        //Julius: Deklarieren des Events
        public static event UpdateFrameEventHandler UpdateFrame;

        //Julius: Videoeigenschaften
        public static Texture2D VideoFrame;
        public static float VideoHeight;
        public static float VideoWidth;
        public static Boolean IsPlaying { get; set; }

        private static KeyboardState keyState;
        private static KeyboardState oldKeyState;

        //Der Container

        public static String currentlyPlaying;

        public static Dictionary<String, VideoObject> Container;

        public static void Initialize()
        {
            Container = new Dictionary<String, VideoObject>();
            /*Julius: Hier die Videofiles in den Container adden!
             *Und NICHT VERGESSEN: das enum entsprechend erweitern und die switch Anweisung in play() erweitern.
             */

            /*Julius: BEISPIEL
             *der erste String "Testvideo" ist der interne assetName (für den Dictonary Container), "Cutscenes/Wildlife" der assetName für die Contentpipeline
             */
            //Container.Add("Testvideo", new VideoObject("Cutscenes/Wildlife"));

            //Container.Add("Cutscene 1", new VideoObject("Cutscenes/Cutscene_1"));
            //Container.Add("Cutscene 2", new VideoObject("Cutscenes/Cutscene_2"));
        }

        public static void LoadContent()
        {   //Julius: LoadContent() für jedes Object im Container
            foreach (KeyValuePair<String, VideoObject> video in Container)
            {
                video.Value.LoadContent();
            }

        }

        public static void Update(GameTime gameTime)
        {
            //Julius: Falls sich ein Objekt beim Event registriert hat: Schmeiß das Event!
            if (UpdateFrame != null)
                UpdateFrame(gameTime);
        }


        public static void play(VideoName name)
        {
            if (!IsPlaying)
            {  //Julius: enum für jedes verfügbare video anpassen!
                switch (name)
                {
                    /*Julius: BEISPIEL:
                         *"Testvideo" ist in diesem Beispiel der interne assetName (wie oben vergeben)
                         */
                    //Container["Testvideo"].play();
                    //currentlyPlaying = "Testvideo";
                    
                    case VideoName.None:
                        return;
                    
                    case VideoName.Cutscene_1:
                        Container["Cutscene 1"].play();
                        currentlyPlaying = "Cutscene 1";
                        GameStateManager.Default.currentGameState = GameState.PlayingCutscene;
                        break;

                    case VideoName.Cutscene_2:
                        Container["Cutscene 2"].play();
                        currentlyPlaying = "Cutscene 2";
                        GameStateManager.Default.currentGameState = GameState.PlayingCutscene;
                        break; 
                }
            }
        }

        public static void stop()
        {
            Container[currentlyPlaying].stop();
        }
    }
}
