using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Silhouette.Engine.Manager;
using System.ComponentModel;
using System.IO;
namespace Silhouette.GameMechs
{
    public class VideoObject : LevelObject
    {
        Video vid;
        VideoPlayer player;
        String _AssetName;

        //Julius: Konstruktor
        public VideoObject(String AssetName)
        {
            this._AssetName = AssetName;

            //Julius: Wir registrieren uns beim Event
            VideoManager.UpdateFrame += new VideoManager.UpdateFrameEventHandler(Update);
        }

        public void play()
        {
            //Julius: Checken, ob schon ein Video gespielt wird, wenn nicht: Tu es!
            if (!VideoManager.IsPlaying)
            {
                VideoManager.IsPlaying = true;
                VideoManager.VideoHeight = vid.Height;
                VideoManager.VideoWidth = vid.Width;
                player.Play(vid);
            }
        }
        public void stop()
        {
            player.Stop();
            VideoManager.IsPlaying = false;
        }

        public override void Initialise() { }

        public override void LoadContent()
        {
            //Julius: Schmeiß es aus der Content Pipeline in das VideoObjekt   
            vid = GameLoop.gameInstance.Content.Load<Video>(_AssetName);
            //Julius: Wir backen uns einen VideoPlayer...
            player = new VideoPlayer();
        }

        public override void Update(GameTime gameTime)
        {
            //Julius: falls ein Video gespielt wird: Update den Frame. Falls nicht: setze die Property auf false


            if (player.State == MediaState.Playing)
            {
                VideoManager.VideoFrame = player.GetTexture();

            }
            else
            {
                if (player.State == MediaState.Stopped)
                {
                    VideoManager.IsPlaying = false;
                }
            }

        }

        public override string getPrefix()
        {
            return "VideoObject_";
        }
    }
}
