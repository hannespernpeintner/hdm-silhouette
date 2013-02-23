using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using Silhouette.Engine.Manager;

namespace Silhouette.Engine
{
    public class RenderTargetFlipFlop
    {

        private RenderTarget2D _basicTarget;
        public RenderTarget2D BasicTarget
        {
            get { return _basicTarget; }
            set { _basicTarget = value; }
        }

        private RenderTarget2D _target1;
        public RenderTarget2D Target1
        {
            get { return _target1; }
            set { _target1 = value; }
        }

        private RenderTarget2D _target2;
        public RenderTarget2D Target2
        {
            get { return _target2; }
            set { _target2 = value; }
        }
        private int _sourceTarget;

        public int SourceTarget
        {
            get { return _sourceTarget; }
            set { _sourceTarget = value; }
        }

        private RenderTarget2D _result;
        public RenderTarget2D Result
        {
            get { return _result; }
            set { _result = value; }
        }

        private SpriteBatch _spriteBatch;
        public SpriteBatch Batch
        {
            get { return _spriteBatch; }
            set { _spriteBatch = value; }
        }

        public RenderTargetFlipFlop(ref SpriteBatch spriteBatch)
        {
            Batch = spriteBatch;
        }

        public void Initialise()
        {
            BasicTarget = new RenderTarget2D(GameLoop.gameInstance.GraphicsDevice, GameSettings.Default.resolutionWidth, GameSettings.Default.resolutionHeight);
            Target1 = new RenderTarget2D(GameLoop.gameInstance.GraphicsDevice, GameSettings.Default.resolutionWidth, GameSettings.Default.resolutionHeight);
            Target2 = new RenderTarget2D(GameLoop.gameInstance.GraphicsDevice, GameSettings.Default.resolutionWidth, GameSettings.Default.resolutionHeight);

            Result = new RenderTarget2D(GameLoop.gameInstance.GraphicsDevice, GameSettings.Default.resolutionWidth, GameSettings.Default.resolutionHeight);
            SourceTarget = 1;
        }
        public void Draw(List<Layer> layerList)
        {
            foreach (Layer l in layerList)
            {
                Draw(l);
            }
            DrawToResultTarget(layerList);
        }

        private void Draw(Layer l)
        {
            Vector2 oldCameraPosition = Camera.Position;
            Camera.Position *= l.ScrollSpeed;

            GameLoop.gameInstance.GraphicsDevice.SetRenderTarget(l.Rt);
            GameLoop.gameInstance.GraphicsDevice.Clear(Color.Transparent);
            Batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Camera.matrix);
            l.drawLayer(Batch);
            Batch.End();
            Camera.Position = oldCameraPosition;
        }

        private void FlipTarget()
        {
            if (SourceTarget == 1)
            {
                SourceTarget = 2;
            }
            else if (SourceTarget == 2)
            {
                SourceTarget = 1;
            }

        }

        private void DrawToResultTarget(List<Layer> layerList)
        {
            GameLoop.gameInstance.GraphicsDevice.SetRenderTarget(BasicTarget);
            GameLoop.gameInstance.GraphicsDevice.Clear(Color.Transparent);

            foreach (Layer l in layerList)
            {
                Batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, l.getShaderByType(l.shaderType));
                //Batch.Begin();
                Batch.Draw(l.Rt, Vector2.Zero, Color.White);
                Batch.End();
            }

            GameLoop.gameInstance.GraphicsDevice.SetRenderTarget(Target2);
            GameLoop.gameInstance.GraphicsDevice.Clear(Color.Transparent);
            Batch.Begin(SpriteSortMode.Deferred, null, null, null, null, EffectManager.Godrays());
            Batch.Draw(BasicTarget, Vector2.Zero, Color.White);
            Batch.End();

            GameLoop.gameInstance.GraphicsDevice.SetRenderTarget(Target1);
            Batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null);
            Batch.Draw(BasicTarget, Vector2.Zero, Color.White);
            Batch.Draw(Target2, Vector2.Zero, Color.White);
            Batch.End();
            GameLoop.gameInstance.GraphicsDevice.SetRenderTarget(Target2);
            Batch.Begin(SpriteSortMode.Deferred, null, null, null, null, EffectManager.VignettenBlur());
            Batch.Draw(Target1, Vector2.Zero, Color.White);
            Batch.End();
            GameLoop.gameInstance.GraphicsDevice.SetRenderTarget(Result);
            Batch.Begin(SpriteSortMode.Deferred, null, null, null, null, EffectManager.ColorChange());
            Batch.Draw(Target2, Vector2.Zero, Color.White);
            Batch.End();

        }
    }
}
