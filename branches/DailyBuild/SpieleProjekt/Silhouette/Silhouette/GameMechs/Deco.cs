using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silhouette.Engine;
using FarseerPhysics.Dynamics;
using Silhouette.Engine.Manager;
using FarseerPhysics.Dynamics.Contacts;

namespace Silhouette.GameMechs
{
    /* Deco containert eine Animation, ist jedoch dafür vorgesehen nur einmalig eine Position zu übergeben, die sich im Folgenden
    auch nicht mehr ändert. Daher wird sie auch beim Konstruktor übergeben und nicht mehr in der Updatemethode. */

    public partial class Deco: DrawableLevelObject
    {
        public Animation animation;
        public int amount;
        public float speed;
        public String path;
        public Fixture fixture;

        public Deco(Vector2 position, int amount, String path, float speed)
        {
            this.position = position;
            this.amount = amount;
            this.path = path;
            this.speed = speed;
            animation = new Animation();
            fixture = FixtureManager.CreateRectangle(animation.activeTexture.Width, animation.activeTexture.Height, position, BodyType.Static, 1.0f);
            fixture.OnCollision += this.OnCollision;
            fixture.OnSeparation += this.OnSeperation;
        }

        public override void Initialise() { }

        public override void LoadContent()
        {
            animation.Load(amount, path, speed, false);
        }

        public override void Update(GameTime gameTime)
        {
            animation.Update(gameTime, position);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }

        public bool OnCollision(Fixture f1, Fixture f2, Contact contact) 
        {
            this.animation.start();
            return true;
        }

        public void OnSeperation(Fixture f1, Fixture f2)
        {
            this.animation.playedOnce = false;
        }
    }
}
