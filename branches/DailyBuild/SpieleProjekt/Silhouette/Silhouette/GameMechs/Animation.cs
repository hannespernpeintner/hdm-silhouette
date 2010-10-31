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

namespace Silhouette.GameMechs
{
    public class Animation
    {
        /* Hannes: 
        Da wir den Plan verworfen haben mehrere Einzelbilder in ein Spritebild zu packen, weil wir dann das zugehörige Polygon
        nicht mehr berechnet bekommen, hier eine neue Version für eine AnimatedSprite, der Kürze Halber Animation genannt. Es
        werden Einzelbilder verwendet, die zu ner Liste hinzugefügt werden. Eine Animation wird geladen, indem man beim
        Aufrufen von Load den Ordnerpfad angibt, sowie die Bilderzahl (!). Es werden nach und nach die durchnummerierten (!)
        Bilder reingeladen. Abgespielt wird automatisch und endlos.
        */

        public List<Texture2D> pictures;                    // Liste mit Einzelbildern, Nummer des gerade aktiven Bildes,
        public int activePictureNumber;                     // zur Sicherheit auch das aktive Bild selber, können wir später
        public Texture2D activePicture;                     // rauslöschen, wenn keine weitere Verwendung, außerdem framespersecond
        public float speed;

        private float totalElapsed;
        private int amount;

        public Animation()
        { 
            pictures = new List<Texture2D>();
            totalElapsed = 0;
        }

        // Wird in der Load des zugehörigen Trägers gerufen
        public void Load(int amount, String path, float speed)
        {
            this.speed = (1 / speed) * 100;
            this.amount = amount;
            for (int i = 0; i <= amount-1; i++)
            {
                String temp = path + i.ToString();
                pictures.Add(GameLoop.gameInstance.Content.Load<Texture2D>(temp));
                activePictureNumber = 0;
                activePicture = pictures[0];
            }
        }

        public void Update(GameTime gameTime)
        {
            float elapsed = gameTime.ElapsedGameTime.Milliseconds;
            totalElapsed += elapsed;

            if (totalElapsed > speed)
            {
                totalElapsed -= speed;
                if (activePictureNumber < amount-1)
                {
                    activePictureNumber++;
                }
                else if (activePictureNumber == amount - 1)
                {
                    activePictureNumber = 0;
                }
            }
            activePicture = pictures[activePictureNumber];
        }

        // Wird in der Draw des Trägers gerufen
        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            spriteBatch.Draw(activePicture, position, color);
        }
    }
}