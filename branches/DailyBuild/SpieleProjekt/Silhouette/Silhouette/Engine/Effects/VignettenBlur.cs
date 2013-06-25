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

        private float _motionBlurNorth;
        public float MotionBlurNorth
        {
            get { return _motionBlurNorth; }
            set { _motionBlurNorth = value; }
        }
        private float _motionBlurEast;
        public float MotionBlurEast
        {
            get { return _motionBlurEast; }
            set { _motionBlurEast = value; }
        }
        private float _motionBlurSouth;
        public float MotionBlurSouth
        {
            get { return _motionBlurSouth; }
            set { _motionBlurSouth = value; }
        }
        private float _motionBlurWest;
        public float MotionBlurWest
        {
            get { return _motionBlurWest; }
            set { _motionBlurWest = value; }
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
                _effect.Parameters["bN"].SetValue(MotionBlurNorth);
                _effect.Parameters["bE"].SetValue(MotionBlurEast);
                _effect.Parameters["bS"].SetValue(MotionBlurSouth);
                _effect.Parameters["bW"].SetValue(MotionBlurWest);
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
            MotionBlurNorth = 0;
            MotionBlurEast = 0;
            MotionBlurSouth = 0;
            MotionBlurWest = 0;
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
            Vector2 velo = GameLoop.gameInstance.playerInstance.CharFix.Body.LinearVelocity;
            if (velo.X > 0)
            {
                MotionBlurEast = velo.X;
                MotionBlurWest = 0;
            }
            else if (velo.X < 0)
            {
                MotionBlurWest = velo.X;
                MotionBlurEast = 0;
            }
            else
            {
                MotionBlurWest = 0;
                MotionBlurEast = 0;
            }

            if (velo.Y > 0)
            {
                MotionBlurNorth = velo.Y;
                MotionBlurSouth = 0;
            }
            else if (velo.Y < 0)
            {
                MotionBlurSouth = velo.Y;
                MotionBlurNorth = 0;
            }
            else
            {
                MotionBlurSouth = 0;
                MotionBlurNorth = 0;
            }

            float blurFactor = 200;
            MotionBlurNorth /= blurFactor;
            MotionBlurEast /= blurFactor;
            MotionBlurSouth /= blurFactor;
            MotionBlurWest /= blurFactor;
        }

        public override void UpdateInEditor(GameTime gameTime)
        {
            
        }
    }
}
