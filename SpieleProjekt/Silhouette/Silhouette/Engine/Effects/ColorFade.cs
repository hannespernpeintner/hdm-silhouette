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

        public static int BlueTargetRed = 50;
        public static int BlueTargetGreen = 60;
        public static int BlueTargetBlue = 900;
        public static int OrangeTargetRed = 870;
        public static int OrangeTargetGreen = 210;
        public static int OrangeTargetBlue = 0;

        public float CurrentRed { get; set; }
        public float CurrentGreen { get; set; }
        public float CurrentBlue { get; set; }

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
                _effect.Parameters["targetRed"].SetValue(CurrentRed);
                _effect.Parameters["targetGreen"].SetValue(CurrentGreen);
                _effect.Parameters["targetBlue"].SetValue(CurrentBlue);
                
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
                _effect.Parameters["targetRed"].SetValue(CurrentRed);
                _effect.Parameters["targetGreen"].SetValue(CurrentGreen);
                _effect.Parameters["targetBlue"].SetValue(CurrentBlue);
                return _effect;
            }
        }

        public override void Initialise()
        {
            base.Initialise();
            Active = true;
            Path = "Effects/ColorChange";

            CurrentRed = 0;
            CurrentGreen = 0;
            CurrentBlue = 0;
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
            //Player player = null;
            Tom player = null;
            try
            {
                //player = GameLoop.gameInstance.playerInstance;
                player = GameLoop.gameInstance.playerInstance;
            }
            catch (Exception e)
            {

            }
            if (player != null && player.RedInterpolator != null)
            {
                {
                    CurrentRed = player.RedInterpolator.CurrentValue * 0.001f;
                    CurrentGreen = player.GreenInterpolator.CurrentValue * 0.001f;
                    CurrentBlue = player.BlueInterpolator.CurrentValue * 0.001f;
                    //Console.WriteLine(CurrentRed + ", " + CurrentGreen + ", " + CurrentBlue);
                }
            }
        }

        public override void UpdateInEditor(GameTime gameTime)
        {
            
        }
    }
}
