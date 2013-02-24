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

//Partikel-Engine Klassen
using Silhouette.Engine.PartikelEngine;
using ProjectMercury;
using ProjectMercury.Emitters;
using ProjectMercury.Modifiers;
using ProjectMercury.Renderers;

using System.ComponentModel;

namespace Silhouette.GameMechs
{
    public enum ParticleType
    {
        None,
        Waterfall,
        Smoke,
        HeavyRain,
        Rain
    }

    [Serializable]
    public class ParticleObject : LevelObject
    {
        [NonSerialized]
        [Browsable(false)]
        public ParticleEffect particleEffect;

        [Browsable(false)]
        public float radius;

        private ParticleType _particleType;
        [DisplayName("Particle Effect"), Category("Particle Data")]
        [Description("The particle effect you want to display.")]
        public ParticleType particleType { get { return _particleType; } set { _particleType = value; } }

        private LevelObject _levelObject;
        [DisplayName("LevelObject"), Category("Particle Data")]
        [Description("The LevelObject the particle effect should be used on. Set to null if not needed.")]
        public LevelObject levelObject { get { return _levelObject; } set { _levelObject = value; } }

        private Vector2 _anchor;
        [DisplayName("Anchor"), Category("Particle Data")]
        [Description("The anchor where the particle effect is attached to the LevelObject.")]
        public Vector2 anchor { get { return _anchor; } set { _anchor = value; } }

        public ParticleObject()
        {
            this.radius = 20;
        }

        public override void Initialise() { }

        public override void LoadContent() 
        {
            particleEffect = ParticleManager.getParticleEffect(particleType);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime) 
        {
            if (levelObject != null)
            {
                this.position = levelObject.position + anchor;
            }
        }

        public override string getPrefix()
        {
            return "ParticleObject_";
        }

        public override LevelObject clone()
        {
            ParticleObject result = (ParticleObject)this.MemberwiseClone();
            result.mouseOn = false;
            return result;
        }

        public override bool contains(Vector2 worldPosition)
        {
            return (worldPosition - position).Length() <= radius;
        }

        public override void transformed() { }

        public override void drawSelectionFrame(SpriteBatch spriteBatch, Matrix matrix)
        {
            Vector2 transformedRadius = Vector2.UnitX * radius;
            Primitives.Instance.drawCircle(spriteBatch, position, transformedRadius.Length(), Color.Yellow, 2);

            Vector2[] extents = new Vector2[4];
            extents[0] = position + Vector2.UnitX * transformedRadius.Length();
            extents[1] = position + Vector2.UnitY * transformedRadius.Length();
            extents[2] = position - Vector2.UnitX * transformedRadius.Length();
            extents[3] = position - Vector2.UnitY * transformedRadius.Length();

            foreach (Vector2 p in extents)
            {
                Primitives.Instance.drawCircleFilled(spriteBatch, p, 4, Color.Yellow);
            }
        }
    }
}
