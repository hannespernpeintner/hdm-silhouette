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

            private FirstBackgroundScreen _firstBackgroundScreen;
            private SecondBackgroundScreen _secondBackgroundScreen;
            private PlayerScreen _playerScreen;
            private ForegroundScreen _foregroundScreen;

            public FirstBackgroundScreen firstBackgroundScreen
            {
                get { return _firstBackgroundScreen; }
                set { _firstBackgroundScreen = value; }
            }

            public SecondBackgroundScreen secondBackgroundScreen
            {
                get { return _secondBackgroundScreen; }
                set { _secondBackgroundScreen = value; }
            }

            public PlayerScreen playerScreen
            {
                get { return _playerScreen; }
                set { _playerScreen = value; }
            }

            public ForegroundScreen foregroundScreen
            {
                get { return _foregroundScreen; }
                set { _foregroundScreen = value; }
            }

            private MenuScreen menuScreen;
            private MainMenuScreen mainMenuScreen;

            List<Screen> gameScreens;
        #endregion

        #region Singleton
            public static void Initialise()
            {
                if (_instance == null)
                    _instance = new ScreenManager();
            }

            private ScreenManager() { spriteBatch = new SpriteBatch(GameLoop.gameInstance.GraphicsDevice); }
        #endregion

        public void LoadScreens()
        {
            //Sascha: Normal Deserialisierung, aber da wir noch keinen LE haben wird es erstmal so gemacht

            //Sascha: Spielescreens, die aus Levelfile geladen werden, da sie je nach Level unterschiedlich sind
            ScreenManager.Default.firstBackgroundScreen = new FirstBackgroundScreen();
            ScreenManager.Default.secondBackgroundScreen = new SecondBackgroundScreen();
            ScreenManager.Default.playerScreen = new PlayerScreen();
            ScreenManager.Default.foregroundScreen = new ForegroundScreen();

            //Sascha: Menüs, die nicht aus Levelfile geladen werden, da sie überall gleich sind
            ScreenManager.Default.menuScreen = new MenuScreen();
            ScreenManager.Default.mainMenuScreen = new MainMenuScreen();
        }

        public void UpdateScreens(GameTime gameTime)
        {
            //Sascha: Provisorischer Code, wird später ersetzt
            ScreenManager.Default.firstBackgroundScreen.updateScreen(gameTime);
            ScreenManager.Default.secondBackgroundScreen.updateScreen(gameTime);
            ScreenManager.Default.playerScreen.updateScreen(gameTime);
            ScreenManager.Default.foregroundScreen.updateScreen(gameTime);
        }

        public void DrawScreens()
        {
            //Sascha: Provisorischer Code, wird später ersetzt
            spriteBatch.Begin();
            ScreenManager.Default.firstBackgroundScreen.drawScreen(spriteBatch);
            ScreenManager.Default.secondBackgroundScreen.drawScreen(spriteBatch);
            ScreenManager.Default.playerScreen.drawScreen(spriteBatch);
            ScreenManager.Default.foregroundScreen.drawScreen(spriteBatch);
            spriteBatch.End();
        }
    }
}
