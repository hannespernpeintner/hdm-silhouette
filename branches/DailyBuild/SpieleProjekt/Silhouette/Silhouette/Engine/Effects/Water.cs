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
    [Serializable]
    public class Water : Effects.EffectObject
    {
        private float _sinTime;
        public float SinTime
        {
            get { return _sinTime; }
            set { _sinTime = value; }
        }
        private float _sinTime2;
        public float SinTime2
        {
            get { return _sinTime2; }
            set { _sinTime2 = value; }
        }
        private float _random;
        public float Random
        {
            get { return _random; }
            set { _random = value; }
        }
        

        [NonSerialized]
        private Effect _effect;
        public override Effect Effect
        {
            get
            {
                Matrix projection = Matrix.CreateOrthographicOffCenter(0, _graphics.Viewport.Width, _graphics.Viewport.Height, 0, 0, 1);
                Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);

                _effect.Parameters["MatrixTransform"].SetValue(halfPixelOffset * projection);
                _effect.Parameters["Time"].SetValue((SinTime+SinTime2)/2);
                return _effect;
            }
            set { _effect = value; }
        }
        public override Effect EffectInEditor(GraphicsDevice graphics)
        {
            {
                Matrix projection = Matrix.CreateOrthographicOffCenter(0, graphics.Viewport.Width, graphics.Viewport.Height, 0, 0, 1);
                Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);
                if (_effect == null)
                {
                    return null;
                }
                _effect.Parameters["MatrixTransform"].SetValue(halfPixelOffset * projection);
                _effect.Parameters["Time"].SetValue((SinTime + SinTime2) / 2);
                return _effect;
            }
        }


        public override void Initialise()
        {
            base.Initialise();
            Active = true;
            Path = "Effects/Water";
            SinTime = 0.5f;
            SinTime = 0.05f;
            Random = 0;
        }
        public override void LoadContent()
        {
            Effect = GameLoop.gameInstance.Content.Load<Effect>(Path);
            _graphics = GameLoop.gameInstance.GraphicsDevice;
        }
        public override void loadContentInEditor(GraphicsDevice graphics, ContentManager content)
        {
            Effect = content.Load<Effect>(Path);
            _graphics = graphics;
        }

        public override void Update(GameTime gameTime)
        {
            SinTime = 0.1f * (float)Math.Sin(0.001f * (float)gameTime.TotalGameTime.TotalMilliseconds);
            SinTime2 = 0.02f * (float)Math.Sin(0.002f * (float)gameTime.TotalGameTime.TotalMilliseconds + 10000 / (float)gameTime.TotalGameTime.TotalMilliseconds);

        }
        public override void UpdateInEditor(GameTime gameTime)
        {
            SinTime = 0.1f * (float)Math.Sin(0.001f * (float)gameTime.TotalGameTime.TotalMilliseconds);
            SinTime2 = 0.02f * (float)Math.Sin(0.002f * (float)gameTime.TotalGameTime.TotalMilliseconds + 10000 / (float)gameTime.TotalGameTime.TotalMilliseconds);

            //SinTime += SinTime;
        }

    }
}
