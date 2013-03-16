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
    public class GodRays : Effects.EffectObject
    {
        

        private float _exposureBase;
        public float ExposureBase
        {
            get { return _exposureBase; }
            set { _exposureBase = value; }
        }
        private float _exposure;
        private float Exposure
        {
            get { return _exposure; }
            set { _exposure = value; }
        }
        private float _noiseMove;
        public float NoiseMove
        {
            get { return _noiseMove; }
            set { _noiseMove = value; }
        }
        private Vector2 _lightPosition;
        public Vector2 LightPosition
        {
            get { return _lightPosition; }
            set { _lightPosition = value; }
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
                _effect.Parameters["Exposure"].SetValue(Exposure * Factor);
                _effect.Parameters["NoiseMove"].SetValue(NoiseMove);
                _effect.Parameters["LightPositionX"].SetValue(LightPosition.X);
                _effect.Parameters["LightPositionY"].SetValue(LightPosition.Y);

                return _effect;
            }
            set { _effect = value; }
        }
        public override Effect EffectInEditor(GraphicsDevice graphics)
        {
            {
                Matrix projection = Matrix.CreateOrthographicOffCenter(0, _graphics.Viewport.Width, _graphics.Viewport.Height, 0, 0, 1);
                Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);

                _effect.Parameters["MatrixTransform"].SetValue(halfPixelOffset * projection);
                _effect.Parameters["Exposure"].SetValue(Exposure * Factor);
                _effect.Parameters["NoiseMove"].SetValue(NoiseMove);
                _effect.Parameters["LightPositionX"].SetValue(LightPosition.X);
                _effect.Parameters["LightPositionY"].SetValue(LightPosition.Y);

                return _effect;
            }
        }


        public override void Initialise()
        {
            base.Initialise();
            Active = true;
            Path = "Effects/GodRays";
            NoiseMove = 0.5f;
            ExposureBase = 0.04515f;
            LightPosition = new Vector2(0.5f, 0f);
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
            double temp = 0.001;
            temp = Math.Sin(0.0015f * gameTime.TotalGameTime.TotalMilliseconds) * 0.01 * (new Random().Next(95, 105) * 0.01f);

            NoiseMove += (float) Math.Sin(0.000005f * gameTime.TotalGameTime.TotalMilliseconds);
            Exposure = ExposureBase + (float)(temp);
        }

    }
}
