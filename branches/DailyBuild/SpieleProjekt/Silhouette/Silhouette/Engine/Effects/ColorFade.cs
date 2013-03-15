using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Silhouette.GameMechs;

namespace Silhouette.Engine.Effects
{
    [Serializable]
    public class ColorFade : Effects.EffectObject
    {
        private float _orangeTargetRed;
        public float OrangeTargetRed
        {
            get { return _orangeTargetRed; }
            set { _orangeTargetRed = value; }
        }
        private float _orangeTargetGreen;
        public float OrangeTargetGreen
        {
            get { return _orangeTargetGreen; }
            set { _orangeTargetGreen = value; }
        }
        private float _orangeTargetBlue;
        public float OrangeTargetBlue
        {
            get { return _orangeTargetBlue; }
            set { _orangeTargetBlue = value; }
        }

        private float _blueTargetRed;
        public float BlueTargetRed
        {
            get { return _blueTargetRed; }
            set { _blueTargetRed = value; }
        }
        private float _blueTargetGreen;
        public float BlueTargetGreen
        {
            get { return _blueTargetGreen; }
            set { _blueTargetGreen = value; }
        }
        private float _blueTargetBlue;
        public float BlueTargetBlue
        {
            get { return _blueTargetBlue; }
            set { _blueTargetBlue = value; }
        }

        private float _fadeOrange;
        public float FadeOrange
        {
            get { return _fadeOrange; }
            set { _fadeOrange = value; }
        }
        private float _fadeBlue;
        public float FadeBlue
        {
            get { return _fadeBlue; }
            set { _fadeBlue = value; }
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
                _effect.Parameters["bla"].SetValue(true);
                _effect.Parameters["alpha"].SetValue(0);
                _effect.Parameters["targetRed"].SetValue(FadeOrange * OrangeTargetRed + FadeBlue * BlueTargetRed);
                _effect.Parameters["targetGreen"].SetValue(FadeOrange * OrangeTargetGreen + FadeBlue * BlueTargetGreen);
                _effect.Parameters["targetBlue"].SetValue(FadeOrange * OrangeTargetBlue + FadeBlue * BlueTargetBlue);
                
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
                _effect.Parameters["bla"].SetValue(true);
                _effect.Parameters["alpha"].SetValue(0);
                _effect.Parameters["targetRed"].SetValue(FadeOrange * OrangeTargetRed + FadeBlue * BlueTargetRed);
                _effect.Parameters["targetGreen"].SetValue(FadeOrange * OrangeTargetGreen + FadeBlue * BlueTargetGreen);
                _effect.Parameters["targetBlue"].SetValue(FadeOrange * OrangeTargetBlue + FadeBlue * BlueTargetBlue);
                return _effect;
            }
        }

        public override void Initialise()
        {
            base.Initialise();
            Active = true;
            Path = "Effects/ColorChange";

            OrangeTargetRed = 0f;
            OrangeTargetGreen = -0.32f;
            OrangeTargetBlue = -0.45f;
            BlueTargetRed = -0.47f;
            BlueTargetGreen = -0.41f;
            BlueTargetBlue = -0.31f;
            FadeBlue = 0;
            FadeOrange = 0;
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

        public override void Update(GameTime gameTime)
        {
            Player player = null;
            try
            {
                player = GameLoop.gameInstance.playerInstance;
                //Tom player = GameLoop.gameInstance.playerInstance;
            }
            catch (Exception e)
            {

            }
            if (player != null)
            {
                //Tom player = GameLoop.gameInstance.playerInstance;
                FadeOrange = player.fadeOrange / 1000; // zählen beide von 0 bis 1
                FadeBlue = player.fadeBlue / 1000;
            }
        }

        public override void UpdateInEditor(GameTime gameTime)
        {
            
        }
    }
}
