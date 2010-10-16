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

//Sascha: Partikel-Engine Klassen
using ProjectMercury;
using ProjectMercury.Emitters;
using ProjectMercury.Modifiers;
using ProjectMercury.Renderers;

namespace Silhouette.PartikelEngine
{
    class ParticleEffectWrapper
    {
        /* Sascha:
        Klasse hat den Zweck alle Zusatzinformationen über Partikeleffekte zu speichern, 
        die für unseren Renderer bzw. unser Levelformat nötig sind
        */

        //Sascha: Zusatzinformationen
        private ParticleEffect particleEffect;
        private Vector2 particlePosition;

        //Sascha: Zugriff auf Zusatzinformationen über Properties
        public ParticleEffect getEffect
        {
            get { return particleEffect; }
            set { particleEffect = value; }
        }

        public Vector2 getPosition
        {
            get { return particlePosition; }
        }

        public ParticleEffectWrapper(ParticleEffect p, Vector2 v)
        {
            particleEffect = p;
            particlePosition = v;
        }
    }
}
