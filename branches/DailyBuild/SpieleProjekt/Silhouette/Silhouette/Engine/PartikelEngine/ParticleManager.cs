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

    public class ParticleManager : Microsoft.Xna.Framework.DrawableGameComponent
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

        public ParticleManager(Game game, GraphicsDeviceManager g)
            : base(game)
        {
            particleRenderer = new SpriteBatchRenderer { GraphicsDeviceService = g}; //Sascha: Eigener Renderer für alle Partikel wegen Zusatzeffekten wie Shader
            particleList = new ArrayList();
        }

        protected override void LoadContent()
        {
            waterfall = Game.Content.Load<ParticleEffect>("ParticleEffects/Water");

            //Sascha: Alle Partikeleffekte werden aus der XML-Datei des Levels geladen und initialisiert
            particleList.Add(new ParticleEffectWrapper(new ParticleEffect(), new Vector2(500, 100)));

            foreach (ParticleEffectWrapper p in particleList)
            {
                p.getEffect = waterfall.DeepCopy();                                 //Sascha: Kopie des entsprechenden Effekts wird erzeugt und übergeben
                p.getEffect.LoadContent(Game.Content);
                p.getEffect.Initialise();
            }
            particleRenderer.LoadContent(Game.Content);
            base.LoadContent();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (ParticleEffectWrapper p in particleList)
            {
                p.getEffect.Trigger(p.getPosition);                                 //Sascha: Auslöser für den Partikeleffekt wird auf der Map gesetzt
                p.getEffect.Update((float)gameTime.ElapsedGameTime.TotalSeconds);   //Sascha: Die Partikel des Effekts werden abhängig von der Spielzeit upgedated
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (ParticleEffectWrapper p in particleList)
            {
                particleRenderer.RenderEffect(p.getEffect);                 //Sascha: Der Renderer rendert alle Partikeleffekte unabhängig vom SpriteBatch
            }
            base.Draw(gameTime);
        }
    }
}
