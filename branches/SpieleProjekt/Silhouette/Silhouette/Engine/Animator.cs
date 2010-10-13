using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Silhouette.Engine
{
    class Animator
    {
        // Hannes: Diese Klasse ist kein Manager, sondern ein direkter Helfer. Für jede Animation, die anfällt, wird ein
        // Animator erstellt. Dieser macht dann sofort darauf animate und braucht einige Paramter.

        private float timer = 0f;
        private int currentFrame = 0;                   // bei welchem Bild der Animation er grad ist
        private Rectangle sourceRect;                   // ist das Viereck aus dem Sprite, was gezeichnet werden soll
        public Rectangle destinationRect;               // ist wo es hingezeichnet werden soll. public, um korrektur
                                                        // von außen, bspweise Physikroutine, zu ermöglichen

        public void animate(Texture2D picture, Rectangle size, Rectangle destinationRect, 
                            int frameCount, int fps, GameTime gameTime, SpriteBatch batch)
        {
            this.destinationRect = destinationRect;

            while (currentFrame < frameCount)                                       // Hannes: solange, bis das
            {                                                                       // letzte Bild der Animation erreicht ist
                timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;         // wird timer um vergangene ms erhöht.
                if (timer > ((float)frameCount / (float)fps))                       // Wenn das Bild der Animation wechselt,
                {                                                                   // wird currentFrame erhöht.
                    currentFrame++;
                }
                sourceRect = new Rectangle(currentFrame * size.Width, 0, size.Width, size.Height);
                batch.Draw(picture, destinationRect, sourceRect, Color.White);      // schließlich wird in das mitgegebene Batch
                                                                                    // gezeichnet
            }
        }


        public void pingPongAnimate(Texture2D picture, Rectangle size, Rectangle destinationRect,
                            int frameCount, int fps, GameTime gameTime, SpriteBatch batch)
        {
            this.destinationRect = destinationRect;                                 //wenn wir ne PingPong-Animation brauchen
            int pingPongDirection = 0;                                              //verwenden wir einfach diese Funktion

            while (currentFrame < frameCount && pingPongDirection == 0)
            {
                timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (timer > ((float)frameCount / (float)fps))
                {
                    currentFrame++;
                    if (currentFrame == frameCount) { pingPongDirection = 1; }
                }
                sourceRect = new Rectangle(currentFrame * size.Width, 0, size.Width, size.Height);
                batch.Draw(picture, destinationRect, sourceRect, Color.White);
            }

            while (currentFrame < frameCount && pingPongDirection == 1)
            {
                timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (timer > ((float)frameCount / (float)fps))
                {
                    currentFrame--;
                    if (currentFrame == 0) { pingPongDirection = 0; }
                }
                sourceRect = new Rectangle(currentFrame * size.Width, 0, size.Width, size.Height);
                batch.Draw(picture, destinationRect, sourceRect, Color.White);
            }

        }

    }
}
