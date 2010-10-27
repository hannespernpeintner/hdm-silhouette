using System;
using System.Collections;
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

namespace Silhouette.Engine.PartikelEngine
{

    public static class ParticleManager
    {
        //Sascha: Klasse hat den Zweck alle Partikeleffekte im Level zentral zu verwalten

        /* Sascha:
         * Wegen Problemen mit der ContentPipeline müssen wir alle Partikeleffekte zentral hier im Manager speichern und dann
         * beim Auslesen aus der XML-Datei eine Kopie des entsprechenden Effekts übergeben. Ansonsten zeigen alle Effekte mit
         * dem gleichen Content auf das selbe Objekt und es kommt zu massiven Darstellungsfehlern.
        */

        private static ParticleEffect _waterfall;

        public static ParticleEffect waterfall { get; set; }

        public static void initialise(GameLoop Game)
        {
            _waterfall = Game.Content.Load<ParticleEffect>("ParticleEffects/Water");
        }
    }
}
