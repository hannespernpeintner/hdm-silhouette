using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Silhouette.Engine.Screens;

namespace Silhouette.Engine.Manager
{
    public class ScreenManager
    {
        #region Definitions
            private static ScreenManager _instance;

            public static ScreenManager Default
            {
                get { return _instance; }
            }

            private SpriteBatch _spriteBatch;

            public SpriteBatch spriteBatch
            {
                get { return _spriteBatch; }
                set { _spriteBatch = value; }
            }

            private ArrayList _gameScreens;

            public ArrayList gameScreens
            {
                get { return _gameScreens; }
            }
        #endregion

        #region Singleton
            public static void Initialise(GameLoop Game)
            {
                if (_instance == null)
                    _instance = new ScreenManager(Game);
            }

            private ScreenManager(GameLoop Game) { spriteBatch = new SpriteBatch(Game.GraphicsDevice); }
        #endregion
    }
}
