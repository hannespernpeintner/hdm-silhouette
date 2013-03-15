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
    public class Bleach : Effects.EffectObject
    {


        private float _bleachMultiplyAmount;
        public float BleachAdditionAmount
        {
            get { return _bleachMultiplyAmount; }
            set { _bleachMultiplyAmount = value; }
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
                _effect.Parameters["BleachAdditionAmount"].SetValue(BleachAdditionAmount - (BleachAdditionAmount * Factor));
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
                _effect.Parameters["BleachAdditionAmount"].SetValue(BleachAdditionAmount - (BleachAdditionAmount * Factor));
                return _effect;
            }
        }

        public override void Initialise()
        {
            base.Initialise();
            Active = true;
            Path = "Effects/Bleach";

            BleachAdditionAmount = 0.5f;
        }
        public override void LoadContent()
        {
            Effect = GameLoop.gameInstance.Content.Load<Effect>(Path);
        }
        public override void loadContentInEditor(GraphicsDevice graphics, ContentManager content)
        {
            Effect = content.Load<Effect>(Path);
            
            _graphics = graphics;
        }

    }
}
