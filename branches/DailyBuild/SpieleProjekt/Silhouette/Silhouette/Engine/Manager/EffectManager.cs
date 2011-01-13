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

namespace Silhouette.Engine.Manager
{
    // HANNES: Erste Version des EffectManagers. Selbsterklärend.
    // Achja: "This software contains source code provided by NVIDIA Corporation." nicht vergessen :D
    public static class EffectManager
    {
        private static Effect blender;
        private static Effect blurrer;
        private static Effect weakBlurrer;
        private static Effect strongBlurrer;
        private static Effect bleach;
        private static Effect weakBleach;
        private static Effect strongBleach;
        private static Effect bloomer;
        private static Effect vignettenBlur;
        private static Effect colorFader;

        public static void loadEffects() 
        {
            blender = GameLoop.gameInstance.Content.Load<Effect>("Effects/blender");

            blurrer = GameLoop.gameInstance.Content.Load<Effect>("Effects/blurrer");
            weakBlurrer = GameLoop.gameInstance.Content.Load<Effect>("Effects/blurrer");
            strongBlurrer = GameLoop.gameInstance.Content.Load<Effect>("Effects/blurrer");

            weakBlurrer.Parameters["BlurDistance"].SetValue(0.001f);
            strongBlurrer.Parameters["BlurDistance"].SetValue(0.005f);

            bleach = GameLoop.gameInstance.Content.Load<Effect>("Effects/bleach");
            weakBleach = GameLoop.gameInstance.Content.Load<Effect>("Effects/bleach");
            strongBleach = GameLoop.gameInstance.Content.Load<Effect>("Effects/bleach");

            weakBleach.Parameters["amount"].SetValue(0.3f);
            strongBleach.Parameters["amount"].SetValue(0.7f);

            bloomer = GameLoop.gameInstance.Content.Load<Effect>("Effects/bloom");
            vignettenBlur = GameLoop.gameInstance.Content.Load<Effect>("Effects/VignettenBlur");
        }

        public static Effect Blender()
        {
            return blender;
        }

        public static Effect Blurrer()
        {
            blurrer.Parameters["BlurDistance"].SetValue(0.002f);
            return blurrer;
        }

        public static Effect WeakBlurrer()
        {
            weakBlurrer.Parameters["BlurDistance"].SetValue(0.001f);
            return weakBlurrer;
        }

        public static Effect StrongBlurrer()
        {
            strongBlurrer.Parameters["BlurDistance"].SetValue(0.004f);
            return strongBlurrer;
        }

        public static Effect Bleach()
        {
            bleach.Parameters["amount"].SetValue(0.5f);
            return bleach;
        }

        public static Effect WeakBleach()
        {
            weakBleach.Parameters["amount"].SetValue(0.3f);
            return weakBleach;
        }

        public static Effect StrongBleach()
        {
            strongBleach.Parameters["amount"].SetValue(0.7f);
            return strongBleach;
        }

        public static Effect Bloom()
        {
            return bloomer;
        }

        public static Effect VignettenBlur()
        {
            return vignettenBlur;
        }

        public static Effect ColorFader()
        {
            return colorFader;
        }

    }
}
