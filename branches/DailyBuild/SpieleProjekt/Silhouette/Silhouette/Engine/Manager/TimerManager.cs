using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

//Sascha: Partikel-Engine Klassen
using ProjectMercury;
using ProjectMercury.Emitters;
using ProjectMercury.Modifiers;
using ProjectMercury.Renderers;

using Silhouette.GameMechs;

namespace Silhouette.Engine.Manager
{

    public static class TimerManager
    {
        private static List<Timer> _timers;

        public static List<Timer> Timers
        {
            get { return _timers; }
            set { _timers = value; }
        }

        public static void initialize()
        {
            Timers = new List<Timer>();
        }
        public static void initializeInEditor(ContentManager content)
        {
        }
        public static void Update(GameTime gameTime)
        {
            foreach (Timer t in Timers)
            {
                t.Update(gameTime);
            }
        }
        public static void UpdateInEditor(GameTime gameTime)
        {
        }
    }
}
