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

namespace Silhouette.Engine.Effects
{
    public class Blur : Effects.EffectObject
    {
        private static Dictionary<String, float> _radiuses;
        private static Dictionary<String, float> Radiuses
        {
            get { return Blur._radiuses; }
            set { Blur._radiuses = value; }
        }

        private int _samples;
        public int Samples
        {
            get { return _samples; }
            set { _samples = value; }
        }
        private float _radius;
        public float Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        private String _type;
        public override String Type
        {
            get { return _type; }
            set 
            {
                Radius = Radiuses[value];
                _type = value;
            }
        }

        private Effect _effect;
        public override Effect Effect
        {
            get
            {
                _effect.Parameters["BlurDistance"].SetValue(Radius);
                _effect.Parameters["Samples"].SetValue(Samples);
                return _effect;
            }
            set { _effect = value; }
        }


        public override void Initialise()
        {
            Radiuses = new Dictionary<String, float>();
            Radiuses.Add("Normal", 2f);
            Radiuses.Add("Weak", 1f);
            Radiuses.Add("Strong", 3f);

            Active = true;
            Path = "Effects/Blur";
            Samples = 4;
            Types = new List<string>();

            Dictionary<String, float>.Enumerator enumerator = Radiuses.GetEnumerator();
            while(enumerator.MoveNext())
            {
                KeyValuePair<String, float> pair = enumerator.Current;
                Types.Add(pair.Key);
            }
            Type = "Normal";
        }
        public override void LoadContent()
        {
            Effect = GameLoop.gameInstance.Content.Load<Effect>(Path);
        }

        public override void Uddate(GameTime gameTime)
        {
        
        }
        public virtual void loadContentInEditor(GraphicsDevice graphics) { }

    }
}
