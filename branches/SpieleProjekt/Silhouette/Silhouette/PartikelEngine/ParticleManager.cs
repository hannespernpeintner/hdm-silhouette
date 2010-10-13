using System;
using System.Collections.Generic;
using System.Collections;
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
    class ParticleManager
    {
        private Renderer particleRenderer;
        private ArrayList particleList;

        public void initialize(GraphicsDeviceManager g)
        {
            particleRenderer = new SpriteBatchRenderer { GraphicsDeviceService = g };
            particleList = new ArrayList();
        }

        public void loadParticles(GameLoop game)
        { 
            particleList.Add(new ParticleEffectWrapper(new ParticleEffect(),new Vector2(500,100))); //Sascha: Nur provisorisch, wird später durch XML-Abfrage ersetzt
            
            foreach(ParticleEffectWrapper p in particleList)
            {
                p.getEffect = game.Content.Load<ParticleEffect>("ParticleEffects/Water"); //Sascha: Nur provisorisch, wird später durch XML-Abfrage ersetzt
                p.getEffect.LoadContent(game.Content);
                p.getEffect.Initialise();

            }

            particleRenderer.LoadContent(game.Content);
        }

        public void updateParticles(GameTime gt)
        {
            foreach (ParticleEffectWrapper p in particleList)
            {
                p.getEffect.Trigger(p.getPosition);
                p.getEffect.Update((float)gt.ElapsedGameTime.TotalSeconds);
            }
        }

        public void drawParticles()
        {
            foreach (ParticleEffectWrapper p in particleList)
            {
                particleRenderer.RenderEffect(p.getEffect);
            }
        }
    }
}
