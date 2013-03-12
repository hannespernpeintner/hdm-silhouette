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
using Silhouette.Engine.Effects;

namespace Silhouette.Engine
{
    public class RenderTargetFlipFlop
    {

        private RenderTarget2D _target0;
        public RenderTarget2D Target0
        {
            get { return _target0; }
            set { _target0 = value; }
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

        private RenderTarget2D _sourceTarget;
        public RenderTarget2D SourceTarget
        {
            get { return _sourceTarget; }
            set { _sourceTarget = value; }
        }
        private RenderTarget2D _destTarget;
        public RenderTarget2D DestTarget
        {
            get { return _destTarget; }
            set { _destTarget = value; }
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

        private GraphicsDevice _graphics;

        public RenderTargetFlipFlop(ref SpriteBatch spriteBatch)
        {
            Batch = spriteBatch;
        }

        public void Initialise()
        {
            Target0 = new RenderTarget2D(GameLoop.gameInstance.GraphicsDevice, GameSettings.Default.resolutionWidth, GameSettings.Default.resolutionHeight);
            Target1 = new RenderTarget2D(GameLoop.gameInstance.GraphicsDevice, GameSettings.Default.resolutionWidth, GameSettings.Default.resolutionHeight);
            Target2 = new RenderTarget2D(GameLoop.gameInstance.GraphicsDevice, GameSettings.Default.resolutionWidth, GameSettings.Default.resolutionHeight);

            Result = new RenderTarget2D(GameLoop.gameInstance.GraphicsDevice, GameSettings.Default.resolutionWidth, GameSettings.Default.resolutionHeight);
            SourceTarget = Target0;
            DestTarget = Target1;
        }
        public void InitialiseInEditor(GraphicsDevice graphics, float viewportWidth, float viewportHeight)
        {
            _graphics = graphics;
            Target0 = new RenderTarget2D(graphics, (int)viewportWidth, (int)viewportHeight);
            Target1 = new RenderTarget2D(graphics, (int)viewportWidth, (int)viewportHeight);
            Target2 = new RenderTarget2D(graphics, (int)viewportWidth, (int)viewportHeight);

            Result = new RenderTarget2D(graphics, (int)viewportWidth, (int)viewportHeight);
            SourceTarget = Target0;
            DestTarget = Target1;
        }

        public void Draw(List<Layer> layerList)
        {
            foreach (Layer l in layerList)
            {
                Draw(l);
            }
            DrawToResultTarget(layerList);
        }
        public void DrawInEditor(List<Layer> layerList)
        {
            foreach (Layer l in layerList)
            {
                DrawInEditor(l);
            }
            DrawInEditorToResultTarget(layerList);
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
        private void DrawInEditor(Layer l)
        {
            Vector2 oldCameraPosition = Camera.Position;
            Camera.Position *= l.ScrollSpeed;

            _graphics.SetRenderTarget(l.Rt);
            _graphics.Clear(Color.Transparent);
            Batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Camera.matrix);
            l.drawInEditor(Batch);
            Batch.End();
            Camera.Position = oldCameraPosition;
        }

        private void FlipTarget()
        {
            if (DestTarget == Target0)
            {
                SourceTarget = Target0;
                DestTarget = Target1;
            }
            else if (DestTarget == Target1)
            {
                SourceTarget = Target1;
                DestTarget = Target2;
            }
            else if (DestTarget == Target2)
            {
                SourceTarget = Target2;
                DestTarget = Target0;
            }

            GameLoop.gameInstance.GraphicsDevice.SetRenderTarget(DestTarget);
            GameLoop.gameInstance.GraphicsDevice.Clear(Color.Transparent);
        }

        private void FlipTargetInEditor()
        {
            if (DestTarget == Target0)
            {
                SourceTarget = Target0;
                DestTarget = Target1;
            }
            else if (DestTarget == Target1)
            {
                SourceTarget = Target1;
                DestTarget = Target2;
            }
            else if (DestTarget == Target2)
            {
                SourceTarget = Target2;
                DestTarget = Target0;
            }

            _graphics.SetRenderTarget(DestTarget);
            _graphics.Clear(Color.Transparent);
        }

        private void DrawToResultTarget(List<Layer> layerList)
        {
            FlipTarget();
            foreach (Layer l in layerList)
            {
                Batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, l.getShaderByType(l.shaderType));
                Batch.Draw(l.Rt, Vector2.Zero, Color.White);
                Batch.End();


                foreach (EffectObject eo in l.Effects)
                {
                    FlipTarget();
                    Batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, eo.Effect);
                    Batch.Draw(SourceTarget, Vector2.Zero, Color.White);
                    Batch.End();
                }
            }


            FlipTarget();
            GameLoop.gameInstance.GraphicsDevice.Clear(Color.Transparent);
            Batch.Begin(SpriteSortMode.Deferred, null, null, null, null, EffectManager.Godrays());
            Batch.Draw(SourceTarget, Vector2.Zero, Color.White);
            Batch.End();

            FlipTarget();
            Batch.Begin(SpriteSortMode.Deferred, null, null, null, null, EffectManager.VignettenBlur());
            Batch.Draw(SourceTarget, Vector2.Zero, Color.White);
            Batch.End();

            FlipTarget();
            Batch.Begin(SpriteSortMode.Deferred, null, null, null, null, EffectManager.Bloom());
            Batch.Draw(SourceTarget, Vector2.Zero, Color.White);
            Batch.End();

            GameLoop.gameInstance.GraphicsDevice.SetRenderTarget(Result);

            Batch.Begin(SpriteSortMode.Deferred, null, null, null, null, EffectManager.ColorChange());
            Batch.Draw(DestTarget, Vector2.Zero, Color.White);
            Batch.End();

            /*Batch.Begin(SpriteSortMode.Deferred, null, null, null, null, EffectManager.ColorChange());
            Batch.Draw(Target2, Vector2.Zero, Color.White);
            Batch.End();*/

        }
        private void DrawInEditorToResultTarget(List<Layer> layerList)
        {
            FlipTargetInEditor();
            foreach (Layer l in layerList)
            {
                if (l.Rt == null)
                {
                    Console.WriteLine("Rendertarget is null at Layer " + l.name);
                    continue;
                }
                Batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, l.getShaderByType(l.shaderType));
                Batch.Draw(l.Rt, Vector2.Zero, Color.White);
                Batch.End();

                if (l.Effects == null)
                {
                    continue;
                }
                foreach (EffectObject eo in l.Effects)
                {
                    FlipTargetInEditor();
                    Batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, eo.Effect);
                    Batch.Draw(SourceTarget, Vector2.Zero, Color.White);
                    Batch.End();
                }
            }


            FlipTargetInEditor();
            _graphics.Clear(Color.Transparent);
            Batch.Begin(SpriteSortMode.Deferred, null, null, null, null, EffectManager.GodraysInEditor(_graphics));
            Batch.Draw(SourceTarget, Vector2.Zero, Color.White);
            Batch.End();

            FlipTargetInEditor();
            Batch.Begin(SpriteSortMode.Deferred, null, null, null, null, EffectManager.VignettenBlur());
            Batch.Draw(SourceTarget, Vector2.Zero, Color.White);
            Batch.End();

            FlipTargetInEditor();
            Batch.Begin(SpriteSortMode.Deferred, null, null, null, null, EffectManager.BloomInEditor(_graphics));
            Batch.Draw(SourceTarget, Vector2.Zero, Color.White);
            Batch.End();

            _graphics.SetRenderTarget(Result);

            Batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null);
            Batch.Draw(DestTarget, Vector2.Zero, Color.White);
            Batch.End();

            /*Batch.Begin(SpriteSortMode.Deferred, null, null, null, null, EffectManager.ColorChange());
            Batch.Draw(Target2, Vector2.Zero, Color.White);
            Batch.End();*/

        }
    }
}
