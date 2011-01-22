using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

//Klassen unserer eigenen Engine
using Silhouette;
using Silhouette.Engine;
using Silhouette.Engine.Manager;
using Silhouette.Engine.Screens;

//Partikel-Engine Klassen
using Silhouette.Engine.PartikelEngine;
using ProjectMercury;
using ProjectMercury.Emitters;
using ProjectMercury.Modifiers;
using ProjectMercury.Renderers;

//Physik-Engine Klassen
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;

namespace Silhouette
{
    public enum GameState
    { 
        MainMenu,
        InGame,
        PlayingCutscene,
        Menu
    }

    public class GameStateManager
    {
        SpriteBatch spriteBatch;

        public GameState currentGameState;

        public MainMenuScreen mainMenuScreen;
        public MenuScreen menuScreen;
        public QuitScreen quitScreen;
        public Level currentLevel;

        String[] levelPaths;
        int currentLevelNumber;

        public bool reallyWantToQuit = false;

        KeyboardState kstate;
        KeyboardState oldkstate;

        public static GameStateManager Default;

        public GameStateManager()
        {
            spriteBatch = new SpriteBatch(GameLoop.gameInstance.GraphicsDevice);

            currentGameState = GameState.MainMenu;
            mainMenuScreen = new MainMenuScreen();
            menuScreen = new MenuScreen();
            quitScreen = new QuitScreen();

            Default = this;
        }

        public void Initialize() 
        {
            mainMenuScreen.initializeScreen();
            menuScreen.initializeScreen();
            quitScreen.initializeScreen();
        }

        public void LoadContent() 
        {
            mainMenuScreen.loadScreen();
            menuScreen.loadScreen();
            quitScreen.loadScreen();
        }

        public void Update(GameTime gameTime) 
        {
            kstate = Keyboard.GetState();

            if (currentGameState == GameState.InGame)
            {
                if (!reallyWantToQuit)
                    currentLevel.Update(gameTime);
                else
                    quitScreen.updateScreen(gameTime);

                if (kstate.IsKeyDown(Keys.Escape) && oldkstate.IsKeyUp(Keys.Escape))
                {
                    reallyWantToQuit = true;
                }
            }
            if (currentGameState == GameState.MainMenu)
            {
                mainMenuScreen.updateScreen(gameTime);
            }
            if (currentGameState == GameState.Menu)
            {
                menuScreen.updateScreen(gameTime);
            }
            if (currentGameState == GameState.PlayingCutscene)
            {
                if (kstate.IsKeyDown(Keys.Escape) && oldkstate.IsKeyUp(Keys.Escape))
                    VideoManager.Container[VideoManager.currentlyPlaying].stop();
            }

            oldkstate = kstate;
        }

        public void Draw(GameTime gameTime) 
        {
            if (currentGameState == GameState.MainMenu)
            {
                mainMenuScreen.drawScreen(spriteBatch);
            }
            if (currentGameState == GameState.InGame)
            {
                currentLevel.Draw();

                if (reallyWantToQuit)
                {
                    quitScreen.drawScreen(spriteBatch);
                }
            }
            if (currentGameState == GameState.Menu)
            {
                menuScreen.drawScreen(spriteBatch);
            }
            if (currentGameState == GameState.PlayingCutscene)
            {
                spriteBatch.Begin();
                if(VideoManager.VideoFrame != null)
                    spriteBatch.Draw(VideoManager.VideoFrame, new Rectangle(0, 0, (int)GameSettings.Default.resolutionWidth, (int)GameSettings.Default.resolutionHeight), Color.White);
                spriteBatch.End();
            }
        }

        public void NewGame()
        {
            currentLevel = Level.LoadLevelFile("12345");
            currentLevel.Initialize();
            currentLevel.LoadContent();
            currentGameState = GameState.InGame;
        }
    }
}
