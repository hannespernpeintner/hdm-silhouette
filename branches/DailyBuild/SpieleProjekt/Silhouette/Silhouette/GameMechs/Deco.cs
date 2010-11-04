using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Silhouette.GameMechs
{
    /* Deco containert eine Animation, ist jedoch dafür vorgesehen nur einmalig eine Position zu übergeben, die sich im Folgenden
    auch nicht mehr ändert. Daher wird sie auch beim Konstruktor übergeben und nicht mehr in der Updatemethode. */

    class Deco: DrawableLevelObject
    {
        public Animation animation;
        public int amount;
        public float speed;
        public String path;

        public Deco(Vector2 position, int amount, String path, float speed)
        {
            this.position = position;
            this.amount = amount;
            this.path = path;
            this.speed = speed;
            animation = new Animation();
        }

        public override void Initialise(){}

        public override void LoadContent()
        {

        }
        public void Load()
        {
            animation.Load(amount, path, speed);
        }

        public override void Update(GameTime gameTime)
        {
            animation.Update(gameTime, position);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }
    }
}
