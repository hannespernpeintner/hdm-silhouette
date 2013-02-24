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

using Silhouette.GameMechs;


namespace Silhouette.Engine.Screens
{
    public enum MenuPage
    {
        MainPage,
        HowToPlayPage
    }

    public enum MenuState
    {
        NewGame,
        Continue,
        HowToPlay,
        Credits,
        Exit
    }

    public class MainMenuScreen
    {
        MenuPage menuPage;
        MenuState menuState;

        Texture2D[] textures;
        Vector2[] positions;

        public SoundObject mainMenuTheme;

        KeyboardState kstate;


        public void initializeScreen()
        {
            menuPage = MenuPage.MainPage;
            menuState = MenuState.NewGame;

            textures = new Texture2D[15];
            positions = new Vector2[15];
        }

        public void loadScreen()
        {
            textures[0] = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/Menu/Menu_1");
            textures[1] = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/Menu/Menu_2");
            textures[2] = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/Menu/Menu_3");
            textures[3] = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/Menu/Menu_4_off");
            textures[4] = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/Menu/Menu_4_on");
            textures[5] = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/Menu/Menu_5_off");
            textures[6] = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/Menu/Menu_5_on");
            textures[7] = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/Menu/Menu_6_off");
            textures[8] = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/Menu/Menu_6_on");
            textures[9] = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/Menu/Menu_7_off");
            textures[10] = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/Menu/Menu_7_on");
            textures[11] = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/Menu/Menu_8_off");
            textures[12] = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/Menu/Menu_8_on");
            textures[13] = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/Menu/Menu_9");

            textures[14] = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/Menu/HowToPlay");

            positions[0] = new Vector2(0, 0);
            positions[1] = new Vector2(textures[0].Width, 0);
            positions[2] = new Vector2(textures[0].Width + textures[1].Width, 0);
            positions[3] = new Vector2(textures[0].Width, textures[1].Height);
            positions[4] = new Vector2(textures[0].Width, textures[1].Height);
            positions[5] = new Vector2(textures[0].Width, textures[1].Height + textures[3].Height);
            positions[6] = new Vector2(textures[0].Width, textures[1].Height + textures[3].Height);
            positions[7] = new Vector2(textures[0].Width, textures[1].Height + textures[3].Height + textures[5].Height);
            positions[8] = new Vector2(textures[0].Width, textures[1].Height + textures[3].Height + textures[5].Height);
            positions[9] = new Vector2(textures[0].Width, textures[1].Height + textures[3].Height + textures[5].Height + textures[7].Height);
            positions[10] = new Vector2(textures[0].Width, textures[1].Height + textures[3].Height + textures[5].Height + textures[7].Height);
            positions[11] = new Vector2(textures[0].Width, textures[1].Height + textures[3].Height + textures[5].Height + textures[7].Height + textures[9].Height);
            positions[12] = new Vector2(textures[0].Width, textures[1].Height + textures[3].Height + textures[5].Height + textures[7].Height + textures[9].Height);
            positions[13] = new Vector2(textures[0].Width, textures[1].Height + textures[3].Height + textures[5].Height + textures[7].Height + textures[9].Height + textures[11].Height);

            positions[14] = new Vector2(0, 0);

            mainMenuTheme = new SoundObject(Path.Combine(Directory.GetCurrentDirectory(), "Content", "Audio", "Menu_ForAllThat.ogg"));
            mainMenuTheme.looped = true;

        }

        public void playMenuMusic()
        {
            if (!mainMenuTheme.isPlaying())
            {
                mainMenuTheme.Play();
            }

        }
        public void updateScreen(GameTime gameTime)
        {
            kstate = GameStateManager.kstate;
            KeyboardState oldkstate = GameStateManager.oldkstate;

            if (menuPage == MenuPage.MainPage)
            {
                if (kstate.IsKeyUp(Keys.Down) && oldkstate.IsKeyDown(Keys.Down))
                {
                    if (menuState == MenuState.Exit)
                        menuState = MenuState.NewGame;
                    else
                    {
                        menuState++;
                    }
                }

                if (kstate.IsKeyUp(Keys.Up) && oldkstate.IsKeyDown(Keys.Up))
                {
                    if (menuState == MenuState.NewGame)
                        menuState = MenuState.Exit;
                    else
                    {
                        menuState--;
                    }
                }

                if (kstate.IsKeyDown(Keys.Enter) && oldkstate.IsKeyUp(Keys.Enter))
                {
                    if (menuState == MenuState.NewGame)
                    {
                        mainMenuTheme.fadeDown(3);
                        GameStateManager.Default.NewGame();
                    }
                    if (menuState == MenuState.Continue)
                    {

                    }
                    if (menuState == MenuState.HowToPlay)
                    {
                        menuPage = MenuPage.HowToPlayPage;
                    }
                    if (menuState == MenuState.Credits)
                    {

                    }
                    if (menuState == MenuState.Exit)
                        GameLoop.gameInstance.Exit();
                }
            }

            if (menuPage == MenuPage.HowToPlayPage)
            {
                if (kstate.IsKeyUp(Keys.Escape) && oldkstate.IsKeyDown(Keys.Escape))
                {
                    menuPage = MenuPage.MainPage;
                }
            }


        }

        public void drawScreen(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (menuPage == MenuPage.MainPage)
            {
                spriteBatch.Draw(textures[0], positions[0], Color.White);
                spriteBatch.Draw(textures[1], positions[1], Color.White);
                spriteBatch.Draw(textures[2], positions[2], Color.White);
                spriteBatch.Draw(textures[13], positions[13], Color.White);

                if (menuState == MenuState.NewGame)
                    spriteBatch.Draw(textures[4], positions[4], Color.White);
                else
                    spriteBatch.Draw(textures[3], positions[3], Color.White);

                if (menuState == MenuState.Continue)
                    spriteBatch.Draw(textures[6], positions[6], Color.White);
                else
                    spriteBatch.Draw(textures[5], positions[5], Color.White);

                if (menuState == MenuState.HowToPlay)
                    spriteBatch.Draw(textures[8], positions[8], Color.White);
                else
                    spriteBatch.Draw(textures[7], positions[7], Color.White);

                if (menuState == MenuState.Credits)
                    spriteBatch.Draw(textures[10], positions[10], Color.White);
                else
                    spriteBatch.Draw(textures[9], positions[9], Color.White);

                if (menuState == MenuState.Exit)
                    spriteBatch.Draw(textures[12], positions[12], Color.White);
                else
                    spriteBatch.Draw(textures[11], positions[11], Color.White);
            }

            if (menuPage == MenuPage.HowToPlayPage)
            {
                spriteBatch.Draw(textures[14], positions[14], Color.White);
            }
            spriteBatch.End();
        }
    }
}
