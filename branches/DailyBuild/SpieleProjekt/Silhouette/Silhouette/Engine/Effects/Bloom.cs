using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Silhouette.Engine.Effects
{
    [Serializable]
    public class Bloom : Effects.EffectObject
    {
        private float _blurDistanceInPixels;
        public float BlurDistanceInPixels
        {
            get { return _blurDistanceInPixels; }
            set { _blurDistanceInPixels = value; }
        }

        private float _blurStrength;
        public float BlurStrength
        {
            get { return _blurStrength; }
            set { _blurStrength = value; }
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
                _effect.Parameters["BlurDistanceInShaderCoords"].SetValue(Factor * BlurDistanceInPixels / _graphics.Viewport.Width);
                _effect.Parameters["BlurStrength"].SetValue(BlurStrength * Factor);
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
                _effect.Parameters["BlurDistanceInShaderCoords"].SetValue(Factor * BlurDistanceInPixels / _graphics.Viewport.Width);
                _effect.Parameters["BlurStrength"].SetValue(BlurStrength * Factor);
                return _effect;
            }
        }

        public override void Initialise()
        {
            base.Initialise();
            Active = true;
            Path = "Effects/Bloom";

            BlurStrength = 0.5f;
            BlurDistanceInPixels = 7;
        }
        public override void LoadContent()
        {
            _graphics = GameLoop.gameInstance.GraphicsDevice;
            Effect = GameLoop.gameInstance.Content.Load<Effect>(Path);
        }
        public override void loadContentInEditor(GraphicsDevice graphics, ContentManager content)
        {
            _graphics = graphics;
            Effect = content.Load<Effect>(Path);
        }

    }
}
