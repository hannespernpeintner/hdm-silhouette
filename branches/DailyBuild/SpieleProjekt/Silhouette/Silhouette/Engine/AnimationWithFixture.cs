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
using Silhouette.Engine;
using Silhouette;
using Silhouette.GameMechs;

namespace Silhouette.Engine
{
    public class AnimationWithFixture
    {
        /*  Hannes: 
            AnimationWithFixture verwenden wir für alle Objekte, die ihr Äußeres ändern (eine Animation) und dabei auch ihre Form. Wenn
            diese zum Beispiel Einfluss auf Kollision mit dem Charakter hat. Ein gutes Beispiel ist ein Schaufelrad, auf dessen
            Schaufelblätter man springen kann, um nach oben zu gelangen.
            Für Animationen, die nur optischen Einfluss nehmen, verwenden wir Animation.
        */

        public Animation animation;
        public List<List<Fixture>> polygons;
        public List<Fixture> activePolygon;

        public AnimationWithFixture()
        {
            animation = new Animation();
            polygons = new List<List<Fixture>>();
        }

        // Wird in der Load des zugehörigen Trägers gerufen
        public void Load(int amount, String path, float speed)
        {
            animation.Load(amount, path, speed);
            polygons = FixtureManager.AnimationToPolygons(animation);
            activePolygon = polygons[animation.activeFrameNumber];
        }

        //Braucht die position des Trägers!
        public void Update(GameTime gameTime, Vector2 position)
        {
            animation.Update(gameTime, position);
            activePolygon = polygons[animation.activeFrameNumber];
            activePolygon[0].Body.Position = position;
        }

        // Wird in der Draw des Trägers gerufen
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(animation.activeTexture, activePolygon[0].Body.Position, Color.White);
        }
    }
}
