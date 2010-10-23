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

    public class ParticleManager
    {
        //Sascha: Klasse hat den Zweck alle Partikeleffekte im Level zentral zu verwalten

        /* Sascha:
        Wegen Problemen mit der ContentPipeline müssen wir alle Partikeleffekte zentral hier im Manager speichern und dann
        beim Auslesen aus der XML-Datei eine Kopie des entsprechenden Effekts übergeben. Ansonsten zeigen alle Effekte mit
        dem gleichen Content auf das selbe Objekt und es kommt zu massiven Darstellungsfehlern.
        */

        private Renderer particleRenderer;
        private ArrayList particleList;

        private ParticleEffect waterfall;

        public ParticleManager(GraphicsDeviceManager g)
        {
            particleRenderer = new SpriteBatchRenderer { GraphicsDeviceService = g}; //Sascha: Eigener Renderer für alle Partikel wegen Zusatzeffekten wie Shader
            particleList = new ArrayList();
        }

        protected void loadParticles(Game Game)
        {
            waterfall = Game.Content.Load<ParticleEffect>("ParticleEffects/Water");

            //Sascha: Alle Partikeleffekte werden aus der XML-Datei des Levels geladen und initialisiert
            particleList.Add(new ParticleEffectWrapper(new ParticleEffect(), new Vector2(500, 100)));

            foreach (ParticleEffectWrapper p in particleList)
            {
                p.particleEffect = waterfall.DeepCopy();                                 //Sascha: Kopie des entsprechenden Effekts wird erzeugt und übergeben
                p.particleEffect.LoadContent(Game.Content);
                p.particleEffect.Initialise();
            }
            particleRenderer.LoadContent(Game.Content);
        }

        public void updateParticles(GameTime gameTime)
        {
            foreach (ParticleEffectWrapper p in particleList)
            {
                p.particleEffect.Trigger(p.particlePosition);                            //Sascha: Auslöser für den Partikeleffekt wird auf der Map gesetzt
                p.particleEffect.Update((float)gameTime.ElapsedGameTime.TotalSeconds);   //Sascha: Die Partikel des Effekts werden abhängig von der Spielzeit upgedated
            }
        }

        public void drawParticles(GameTime gameTime)
        {
            foreach (ParticleEffectWrapper p in particleList)
            {
                particleRenderer.RenderEffect(p.particleEffect);                 //Sascha: Der Renderer rendert alle Partikeleffekte unabhängig vom SpriteBatch
            }
        }
    }
}
