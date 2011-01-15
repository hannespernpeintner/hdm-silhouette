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
        private static Texture2D cbbVignette;

        private static Effect blender;
        private static Effect blurrer;
        private static Effect weakBlurrer;
        private static Effect strongBlurrer;
        private static Effect bleach;
        private static Effect weakBleach;
        private static Effect strongBleach;
        private static Effect bloomer;
        private static Effect vignettenBlur;
        private static Effect colorChange;

        public static void loadEffects() 
        {
            vignette = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/Overlays/Vignette");
            cbbVignette = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/Overlays/CenterBleachBlurMask");
            GameLoop.gameInstance.GraphicsDevice.Textures[1] = vignette;
            GameLoop.gameInstance.GraphicsDevice.Textures[2] = cbbVignette;

            blender = GameLoop.gameInstance.Content.Load<Effect>("Effects/blender");

            colorChange = GameLoop.gameInstance.Content.Load<Effect>("Effects/ColorChange");

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
            Player player = GameLoop.gameInstance.playerInstance;

            if (player.isRemembering)
            {
                bleach.Parameters["amount"].SetValue(0.5f);
                bleach.Parameters["bla"].SetValue(true);
                bleach.Parameters["targetRed"].SetValue(0.6f);
                bleach.Parameters["targetGreen"].SetValue(0.3f);
                bleach.Parameters["targetBlue"].SetValue(0.2f);
            }

            else if (player.isRecovering)
            {
                bleach.Parameters["amount"].SetValue(0.5f);
                bleach.Parameters["bla"].SetValue(true);
                bleach.Parameters["targetRed"].SetValue(0.1f);
                bleach.Parameters["targetGreen"].SetValue(0.2f);
                bleach.Parameters["targetBlue"].SetValue(0.4f);
            }
            else
            {
                bleach.Parameters["amount"].SetValue(0.5f);
                bleach.Parameters["bla"].SetValue(false);
                bleach.Parameters["targetRed"].SetValue(0);
                bleach.Parameters["targetGreen"].SetValue(0);
                bleach.Parameters["targetBlue"].SetValue(0);
            }

            return bleach;
        }

        public static Effect WeakBleach()
        {
            Player player = GameLoop.gameInstance.playerInstance;

            if (player.isRemembering)
            {
                bleach.Parameters["amount"].SetValue(0.3f);
                bleach.Parameters["bla"].SetValue(true);
                bleach.Parameters["targetRed"].SetValue(0.6f);
                bleach.Parameters["targetGreen"].SetValue(0.3f);
                bleach.Parameters["targetBlue"].SetValue(0.2f);
            }

            else if (player.isRecovering)
            {
                bleach.Parameters["amount"].SetValue(0.3f);
                bleach.Parameters["bla"].SetValue(true);
                bleach.Parameters["targetRed"].SetValue(0.1f);
                bleach.Parameters["targetGreen"].SetValue(0.2f);
                bleach.Parameters["targetBlue"].SetValue(0.4f);
            }
            else
            {
                bleach.Parameters["amount"].SetValue(0.3f);
                bleach.Parameters["bla"].SetValue(false);
                bleach.Parameters["targetRed"].SetValue(0);
                bleach.Parameters["targetGreen"].SetValue(0);
                bleach.Parameters["targetBlue"].SetValue(0);
            }
            return weakBleach;
        }

        public static Effect StrongBleach()
        {
            Player player = GameLoop.gameInstance.playerInstance;

            if (player.isRemembering)
            {
                bleach.Parameters["amount"].SetValue(0.7f);
                bleach.Parameters["bla"].SetValue(true);
                bleach.Parameters["targetRed"].SetValue(0.6f);
                bleach.Parameters["targetGreen"].SetValue(0.3f);
                bleach.Parameters["targetBlue"].SetValue(0.2f);
            }

            else if (player.isRecovering)
            {
                bleach.Parameters["amount"].SetValue(0.7f);
                bleach.Parameters["bla"].SetValue(true);
                bleach.Parameters["targetRed"].SetValue(0.1f);
                bleach.Parameters["targetGreen"].SetValue(0.2f);
                bleach.Parameters["targetBlue"].SetValue(0.4f);
            }
            else
            {
                bleach.Parameters["amount"].SetValue(0.7f);
                bleach.Parameters["bla"].SetValue(false);
                bleach.Parameters["targetRed"].SetValue(0);
                bleach.Parameters["targetGreen"].SetValue(0);
                bleach.Parameters["targetBlue"].SetValue(0);
            }
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

        public static Effect ColorChange()
        {
            Player player = GameLoop.gameInstance.playerInstance;

            if (player.isRemembering)
            {
                colorChange.Parameters["bla"].SetValue(true);
                colorChange.Parameters["targetRed"].SetValue(1);
                colorChange.Parameters["targetGreen"].SetValue(0.6f);
                colorChange.Parameters["targetBlue"].SetValue(0.3f);
            }

            else if (player.isRecovering)
            {
                colorChange.Parameters["bla"].SetValue(true);
                colorChange.Parameters["targetRed"].SetValue(0.3f);
                colorChange.Parameters["targetGreen"].SetValue(0.4f);
                colorChange.Parameters["targetBlue"].SetValue(0.7f);
            }
            else
            {
                colorChange.Parameters["bla"].SetValue(false);
                colorChange.Parameters["targetRed"].SetValue(0);
                colorChange.Parameters["targetGreen"].SetValue(0);
                colorChange.Parameters["targetBlue"].SetValue(0);
            }

            return colorChange;
        }

    }
}
