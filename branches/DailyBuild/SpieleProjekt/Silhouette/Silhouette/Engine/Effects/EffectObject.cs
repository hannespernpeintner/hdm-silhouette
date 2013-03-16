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
using System.Text;

using Silhouette.GameMechs;

namespace Silhouette.Engine.Effects
{
    [Serializable]
    public abstract class EffectObject : LevelObject
    {
        [NonSerialized]
        protected GraphicsDevice _graphics;

        private List<String> _types;
        public List<String> Types
        {
            get { return _types; }
            set { _types = value; }
        }

        private String _type;
        public virtual String Type
        {
            get { return "Normal"; }
            set { _type = value; }
        }

        private String _path;
        public String Path
        {
            get { return _path; }
            set { _path = value; }
        }

        [NonSerialized]
        private Effect _effect;
        public virtual Effect Effect
        {
            get { return _effect; }
            set { _effect = value; }
        }
        public virtual Effect EffectInEditor(GraphicsDevice graphics)
        {
            return Effect;
        }

        private bool _active;
        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }
        private float _factor;
        public float Factor
        {
            get { return _factor; }
            set { _factor = value; }
        }

        protected static int _effectCounter = 0;

        // Bitte initialisiert im Types-Array immer den Default-Type als ERSTES ELEMENT bei Index 0.
        public override void Initialise() { Factor = 1.0f; name = "EffectOject" + ++_effectCounter; }
        public override void LoadContent() { }
        public override void Update(GameTime gameTime){}
        public virtual void UpdateInEditor(GameTime gameTime) {this.Update(gameTime);}
        public virtual void loadContentInEditor(GraphicsDevice graphics, ContentManager content) { }

    }
}
