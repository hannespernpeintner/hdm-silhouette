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
using Silhouette.GameMechs;

namespace Silhouette.Engine.Manager
{
    // HANNES: Erste Version des EffectManagers. Selbsterklärend.
    // Achja: "This software contains source code provided by NVIDIA Corporation." nicht vergessen :D

    public static class EffectManager
    {
        private static Texture2D vignette;

        private static Effect blender;
        private static Effect blurrer;
        private static Effect weakBlurrer;
        private static Effect strongBlurrer;
        private static Effect bleachBlur;
        private static Effect bleach;
        private static Effect weakBleach;
        private static Effect strongBleach;
        private static Effect bloomer;
        private static Effect vignettenBlur;
        private static Effect colorChange;

        public static void loadEffects() 
        {
            vignette = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/Overlays/Vignette");
            GameLoop.gameInstance.GraphicsDevice.Textures[1] = vignette;

            blender = GameLoop.gameInstance.Content.Load<Effect>("Effects/blender");

            colorChange = GameLoop.gameInstance.Content.Load<Effect>("Effects/ColorChange");

            blurrer = GameLoop.gameInstance.Content.Load<Effect>("Effects/blurrer");
            weakBlurrer = GameLoop.gameInstance.Content.Load<Effect>("Effects/blurrer");
            strongBlurrer = GameLoop.gameInstance.Content.Load<Effect>("Effects/blurrer");
            weakBlurrer.Parameters["BlurDistance"].SetValue(0.001f);
            strongBlurrer.Parameters["BlurDistance"].SetValue(0.005f);

            bleachBlur = GameLoop.gameInstance.Content.Load<Effect>("Effects/BleachBlur");
            bleachBlur.Parameters["BlurDistance"].SetValue(0.002f);

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
            //strongBlurrer.Parameters["BlurDistance"].SetValue(0.004f);
            strongBlurrer.Parameters["BlurDistance"].SetValue(0.006f);
            return strongBlurrer;
        }

        public static Effect Bleach()
        {
            Player player = GameLoop.gameInstance.playerInstance;
            float fadeOrange = player.fadeOrange / 1000; // zählen beide von 0 bis 1
            float fadeBlue = player.fadeBlue / 1000;

            float orangeTargetRed = 0f;
            float orangeTargetGreen = -0.32f;
            float orangeTargetBlue = -0.45f;
            float blueTargetRed = -0.47f;
            float blueTargetGreen = -0.41f;
            float blueTargetBlue = -0.31f;

                bleach.Parameters["amount"].SetValue(0.5f);
                bleach.Parameters["targetRed"].SetValue(fadeOrange * orangeTargetRed + fadeBlue * blueTargetRed);
                bleach.Parameters["targetGreen"].SetValue(fadeOrange * orangeTargetGreen + fadeBlue * blueTargetGreen);
                bleach.Parameters["targetBlue"].SetValue(fadeOrange * orangeTargetBlue + fadeBlue * blueTargetBlue);
            
            return bleach;
        }

        public static Effect WeakBleach()
        {
            Player player = GameLoop.gameInstance.playerInstance;
            float fadeOrange = player.fadeOrange / 1000; // zählen beide von 0 bis 1
            float fadeBlue = player.fadeBlue / 1000;

            float orangeTargetRed = 0f;
            float orangeTargetGreen = -0.32f;
            float orangeTargetBlue = -0.45f;
            float blueTargetRed = -0.47f;
            float blueTargetGreen = -0.41f;
            float blueTargetBlue = -0.31f;

            bleach.Parameters["amount"].SetValue(0.3f);
            bleach.Parameters["targetRed"].SetValue(fadeOrange * orangeTargetRed + fadeBlue * blueTargetRed);
            bleach.Parameters["targetGreen"].SetValue(fadeOrange * orangeTargetGreen + fadeBlue * blueTargetGreen);
            bleach.Parameters["targetBlue"].SetValue(fadeOrange * orangeTargetBlue + fadeBlue * blueTargetBlue);

            return weakBleach;
        }

        public static Effect StrongBleach()
        {
            Player player = GameLoop.gameInstance.playerInstance;
            float fadeOrange = player.fadeOrange / 1000; // zählen beide von 0 bis 1
            float fadeBlue = player.fadeBlue / 1000;

            float orangeTargetRed = 0f;
            float orangeTargetGreen = -0.32f;
            float orangeTargetBlue = -0.45f;
            float blueTargetRed = -0.47f;
            float blueTargetGreen = -0.41f;
            float blueTargetBlue = -0.31f;

            bleach.Parameters["amount"].SetValue(0.7f);
            bleach.Parameters["targetRed"].SetValue(fadeOrange * orangeTargetRed + fadeBlue * blueTargetRed);
            bleach.Parameters["targetGreen"].SetValue(fadeOrange * orangeTargetGreen + fadeBlue * blueTargetGreen);
            bleach.Parameters["targetBlue"].SetValue(fadeOrange * orangeTargetBlue + fadeBlue * blueTargetBlue);

            return weakBleach;
        }

        public static Effect BleachBlur()
        {
            bleachBlur.Parameters["BlurDistance"].SetValue(0.004f);
            return bleachBlur;
        }

        public static Effect Bloom()
        {
            return bloomer;
        }

        public static Effect VignettenBlur()
        {
            return vignettenBlur;
        }

        public static Effect ColorChange()
        {
            Player player = GameLoop.gameInstance.playerInstance;
            float fadeOrange = player.fadeOrange / 1000; // zählen beide von 0 bis 1
            float fadeBlue = player.fadeBlue / 1000;

            float orangeTargetRed = 0f;
            float orangeTargetGreen = -0.32f;
            float orangeTargetBlue = -0.45f;
            float blueTargetRed = -0.47f;
            float blueTargetGreen = -0.41f;
            float blueTargetBlue = -0.31f;

            {
                {
                    colorChange.Parameters["bla"].SetValue(true);
                    colorChange.Parameters["alpha"].SetValue(0);
                    colorChange.Parameters["targetRed"].SetValue(fadeOrange * orangeTargetRed + fadeBlue * blueTargetRed);
                    colorChange.Parameters["targetGreen"].SetValue(fadeOrange * orangeTargetGreen + fadeBlue * blueTargetGreen);
                    colorChange.Parameters["targetBlue"].SetValue(fadeOrange * orangeTargetBlue + fadeBlue * blueTargetBlue);
                }
            }
            return colorChange;
        }
    }
}
