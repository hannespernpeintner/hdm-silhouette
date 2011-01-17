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

        GameState currentGameState;

        MainMenuScreen mainMenuScreen;
        MenuScreen menuScreen;
        Level currentLevel;

        public GameStateManager()
        {
            spriteBatch = new SpriteBatch(GameLoop.gameInstance.GraphicsDevice);

            currentGameState = GameState.InGame;
            mainMenuScreen = new MainMenuScreen();
            menuScreen = new MenuScreen();
        }

        public void Initialize() 
        {
            currentLevel = Level.LoadLevelFile("12345");
            currentLevel.Initialize();
        }

        public void LoadContent() 
        {
            currentLevel.LoadContent();  
        }

        public void Update(GameTime gameTime) 
        {
            if (currentGameState == GameState.InGame)
            {
                currentLevel.Update(gameTime);
            }
            if (currentGameState == GameState.MainMenu)
            {
                mainMenuScreen.updateScreen(gameTime);
            }
            if (currentGameState == GameState.Menu)
            {
                menuScreen.updateScreen(gameTime);
            }

            if (VideoManager.IsPlaying)
                currentGameState = GameState.PlayingCutscene;
            else
                currentGameState = GameState.InGame;
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
    }
}
