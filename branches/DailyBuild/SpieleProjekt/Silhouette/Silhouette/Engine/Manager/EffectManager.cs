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
        private static Effect bloomer;
        private static Effect godRays;

        public static void loadEffects() 
        {
            blender = GameLoop.gameInstance.Content.Load<Effect>("Effects/blender");
            blurrer = GameLoop.gameInstance.Content.Load<Effect>("Effects/blurrer");
            bloomer = GameLoop.gameInstance.Content.Load<Effect>("Effects/bloom");
            godRays = GameLoop.gameInstance.Content.Load<Effect>("Effects/GodRays");
        }

        public static Effect Blender()
        {
            return blender;
        }

        public static Effect Blurrer()
        {
            return blurrer;
        }

        public static Effect Bloom()
        {
            return bloomer;
        }

        public static Effect GodRays()
        {
            return godRays;
        }

    }
}
