using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

//Klassen unserer eigenen Engine
using Silhouette;
using Silhouette.Engine;
using Silhouette.Engine.Manager;
using Silhouette.GameMechs;

//Partikel-Engine Klassen
using Silhouette.Engine.PartikelEngine;
using ProjectMercury;
using ProjectMercury.Emitters;
using ProjectMercury.Modifiers;
using ProjectMercury.Renderers;

namespace Silhouette.Engine.PartikelEngine
{
    public class ParticleRenderer
    {
        public Renderer particleRenderer;
        private List<LevelObject> particlesToRender;

        public ParticleRenderer()
        {
            particleRenderer = new SpriteBatchRenderer { GraphicsDeviceService = GameLoop.gameInstance.graphics };
            particlesToRender = new List<LevelObject>();
        }
        public ParticleRenderer(GraphicsDeviceManager gdm)
        {
            particleRenderer = new SpriteBatchRenderer { GraphicsDeviceService = gdm };
            particlesToRender = new List<LevelObject>();
        }

        public void addParticleObjects(LevelObject lo)
        {
            if (!particlesToRender.Contains(lo) && lo is ParticleObject)
            {
                particlesToRender.Add(lo);
                if (((ParticleObject)(lo)).particleEffect != null)
                {
                    ((ParticleObject)(lo)).particleEffect.LoadContent(GameLoop.gameInstance.Content);
                    ((ParticleObject)(lo)).particleEffect.Initialise();
                }
            }
        }

        public void removeParticleObjectsInEditor(LevelObject lo)
        {
            if (particlesToRender.Contains(lo))
            {
                particlesToRender.Remove(lo);
            }
        }

        public void addParticleObjectsInEditor(LevelObject lo, ContentManager content)
        {
            if (!particlesToRender.Contains(lo) && lo is ParticleObject)
            {
                particlesToRender.Add(lo);
                if (((ParticleObject)(lo)).particleEffect != null)
                {
                    ((ParticleObject)(lo)).particleEffect.LoadContent(content);
                    ((ParticleObject)(lo)).particleEffect.Initialise();
                }

                particleRenderer.LoadContent(content);
            }
        }

        public void initializeParticles()
        {
            if (!(particlesToRender.Count > 0))
                return;

            foreach (ParticleObject p in particlesToRender)
            {
                if (p.particleEffect != null)
                {
                    p.particleEffect.LoadContent(GameLoop.gameInstance.Content);
                    p.particleEffect.Initialise();
                }
            }

            particleRenderer.LoadContent(GameLoop.gameInstance.Content);
        }

        public void initializeParticlesInEditor(ContentManager content)
        {
            if (!(particlesToRender.Count > 0))
                return;

            foreach (ParticleObject p in particlesToRender)
            {
                //if (p.particleEffect != null)
                try
                {
                    p.particleEffect.LoadContent(content);
                    p.particleEffect.Initialise();
                }
                catch (Exception e)
                { }
            }

            particleRenderer.LoadContent(content);
        }

        public void updateParticles(GameTime gameTime)
        {
            if (!(particlesToRender.Count > 0))
                return;

            foreach (ParticleObject p in particlesToRender)
            {
                if (p.particleEffect != null)
                {
                    p.particleEffect.Trigger(p.position);
                    p.particleEffect.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
                }
            }
        }


        public void drawParticles()
        {
            if (!(particlesToRender.Count > 0))
                return;

            foreach (ParticleObject p in particlesToRender)
            {
                if (p.particleEffect != null)
                    particleRenderer.RenderEffect(p.particleEffect, ref Camera.matrix);
            }
        }
        public void drawParticlesInEditor(ContentManager content)
        {
            if (!(particlesToRender.Count > 0))
                return;

            foreach (ParticleObject p in particlesToRender)
            {
                if (p.particleEffect != null)
                {
                    particleRenderer.RenderEffect(p.particleEffect, ref Camera.matrix);
                }
                
            }
        }
    }
}
