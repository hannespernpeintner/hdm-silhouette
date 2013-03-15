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
    public class VignettenBlur : Effects.EffectObject
    {
        private bool _overallBlur;
        public bool OverallBlur
        {
            get { return _overallBlur; }
            set { _overallBlur = value; }
        }
        private bool _overallVignette;
        public bool OverallVignette
        {
            get { return _overallVignette; }
            set { _overallVignette = value; }
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
                _effect.Parameters["bBlur"].SetValue(OverallBlur);
                _effect.Parameters["bVignette"].SetValue(OverallVignette);
                return _effect;
            }
            set { _effect = value; }
        }
        public override Effect EffectInEditor(GraphicsDevice graphics)
        {
            {
                Matrix projection = Matrix.CreateOrthographicOffCenter(0, graphics.Viewport.Width, graphics.Viewport.Height, 0, 0, 1);
                Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);

                _effect.Parameters["MatrixTransform"].SetValue(halfPixelOffset * projection);
                _effect.Parameters["bBlur"].SetValue(OverallBlur);
                _effect.Parameters["bVignette"].SetValue(OverallVignette); 
                return _effect;
            }
        }


        public override void Initialise()
        {
            base.Initialise();
            Active = true;
            Path = "Effects/VignettenBlur";
            OverallBlur = true;
            OverallVignette = true;
        }
        public override void LoadContent()
        {
            _graphics = GameLoop.gameInstance.GraphicsDevice;
            Effect = GameLoop.gameInstance.Content.Load<Effect>(Path);
        }
        public override void loadContentInEditor(GraphicsDevice graphics, ContentManager content)
        {
            Effect = content.Load<Effect>(Path);
            _graphics = graphics;
        }

        public override void Update(GameTime gameTime)
        {
        
        }

    }
}
