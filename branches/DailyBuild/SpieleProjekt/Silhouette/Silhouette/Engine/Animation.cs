using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics.Dynamics;
using Silhouette.Engine.Manager;

namespace Silhouette.Engine
{
    public class Animation
    {
        /* Hannes: 
        Da wir den Plan verworfen haben mehrere Einzelbilder in ein Spritebild zu packen, weil wir dann das zugehörige Polygon
        nicht mehr berechnet bekommen, hier eine neue Version für eine AnimatedSprite, der Kürze Halber Animation genannt. Es
        werden Einzelbilder verwendet, die zu ner Liste hinzugefügt werden. Eine Animation wird geladen, indem man beim
        Aufrufen von Load den Ordnerpfad angibt, sowie die Bilderzahl (!). Es werden nach und nach die durchnummerierten (!)
        Bilder reingeladen. Abgespielt wird automatisch und endlos. Die Animation ist beweglich.
        */

        public List<Texture2D> pictures;                    // Liste mit Einzelbildern, Nummer des gerade aktiven Bildes,
        public int activeFrameNumber;                       // zur Sicherheit auch das aktive Bild selber, können wir später
        public Texture2D activeTexture;                     // rauslöschen, wenn keine weitere Verwendung, auÃ erdem framespersecond
        public float speed;
        public Vector2 position;
        public float rotation;
        public bool looped;
        public bool playedOnce;

        private bool started;
        private float totalElapsed;
        public int amount;

        public Animation()
        {
            pictures = new List<Texture2D>();
            totalElapsed = 0;
            this.playedOnce = false;
            this.started = false;
        }

        // Wird in der Load des zugehörigen Trägers gerufen
        public void Load(int amount, String path, float speed, bool looped)
        {
            this.speed = (1 / speed) * 100;
            this.amount = amount;
            this.position = Vector2.Zero;
            this.looped = looped;
            

            for (int i = 0; i <= amount - 1; i++)
            {
                String temp = path + i.ToString();
                pictures.Add(GameLoop.gameInstance.Content.Load<Texture2D>(temp));
            }
            activeFrameNumber = 0;
            activeTexture = pictures[activeFrameNumber];
        }

        //Braucht die position des Trägers!
        public void Update(GameTime gameTime, Vector2 position)
        {
            float elapsed = gameTime.ElapsedGameTime.Milliseconds;
            totalElapsed += elapsed;
            this.position = position;

            if (looped && started)
            {
                if (totalElapsed > speed)
                {
                    totalElapsed -= speed;
                    if (activeFrameNumber < amount - 1)
                    {
                        activeFrameNumber++;
                    }
                    else if (activeFrameNumber == amount - 1)
                    {
                        activeFrameNumber = 0;
                    }
                }
            }

            if (!looped && !playedOnce && started)
            {
                if (totalElapsed > speed)
                {
                    totalElapsed -= speed;
                    if (activeFrameNumber < amount - 1)
                    {
                        activeFrameNumber++;
                    }
                    else if (activeFrameNumber == amount - 1)
                    {
                        activeFrameNumber = 0;
                        playedOnce = true;
                    }
                }
            }
            activeTexture = pictures[activeFrameNumber];
        }

        // Wird in der Draw des Trägers gerufen
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(activeTexture,new Rectangle((int)position.X, (int)position.Y, activeTexture.Width, activeTexture.Height),new Rectangle(0,0,activeTexture.Width, activeTexture.Height), Color.White, this.rotation, Vector2.Zero, SpriteEffects.None, 0.0f);
        }

        public void start()
        {
            started = true;
        }
    }
}