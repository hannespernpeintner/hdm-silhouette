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
   static class VideoManager
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

       //Julius: Enum Gedöns für die einfache Videowiedergae
       public enum Videoname { testvideo };



       //Der Container

       private static String currentlyPlaying;

       static Dictionary<String, VideoObject> Container;

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
           keyState = Keyboard.GetState();

           //Julius: Falls sich ein Objekt beim Event registriert hat: Schmeiß das Event!
           if (UpdateFrame != null)
               UpdateFrame(gameTime);
           //Julius: Jemand während der Wiedergabe ESC drückt: stop()
           if (IsPlaying)
           {
               if (keyState.IsKeyDown(Keys.Escape) && oldKeyState.IsKeyUp(Keys.Escape))
                   Container[currentlyPlaying].stop();   
           }

           oldKeyState = keyState;
       }


       public static void play(Videoname name)
       {
           if (!IsPlaying)
           {  //Julius: enum für jedes verfügbare video anpassen!
               switch (name) 
               {case Videoname.testvideo:
                    
                 /*Julius: BEISPIEL:
                  *"Testvideo" ist in diesem Beispiel der interne assetName (wie oben vergeben)
                  */ 
                 //Container["Testvideo"].play();
                 //currentlyPlaying = "Testvideo";
                   
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
