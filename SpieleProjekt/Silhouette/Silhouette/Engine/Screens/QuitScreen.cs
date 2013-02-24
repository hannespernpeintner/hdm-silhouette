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
    public enum Choice
    {
        Yes,
        No
    }

    public class QuitScreen
    {
        Choice wantToQuitChoice;

        Texture2D[] textures;
        Vector2[] positions;

        KeyboardState kstate;
        KeyboardState oldkstate;

        public void initializeScreen()
        {
            wantToQuitChoice = Choice.No;

            textures = new Texture2D[10];
            positions = new Vector2[10];
        }

        public void loadScreen()
        {
            textures[0] = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/Menu/reallyQuit_01");
            textures[1] = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/Menu/reallyQuit_02");
            textures[2] = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/Menu/reallyQuit_03");
            textures[3] = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/Menu/reallyQuit_04");
            textures[4] = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/Menu/reallyQuit_05");
            textures[5] = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/Menu/reallyQuit_06");
            textures[6] = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/Menu/reallyQuit_07");
            textures[7] = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/Menu/reallyQuit_08");
            textures[8] = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/Menu/reallyQuitOn_04");
            textures[9] = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/Menu/reallyQuitOn_05");

            positions[0] = new Vector2(0, 0);
            positions[1] = new Vector2(textures[0].Width, 0);
            positions[2] = new Vector2(0, textures[1].Height);
            positions[3] = new Vector2(textures[2].Width, textures[0].Height);
            positions[4] = new Vector2(textures[2].Width + textures[3].Width, textures[0].Height);
            positions[5] = new Vector2(textures[2].Width + textures[3].Width + textures[4].Width, textures[0].Height);
            positions[6] = new Vector2(textures[2].Width, textures[0].Height + textures[3].Height);
            positions[7] = new Vector2(textures[2].Width + textures[6].Width, textures[0].Height + textures[3].Height);
            positions[8] = new Vector2(textures[2].Width, textures[0].Height);
            positions[9] = new Vector2(textures[2].Width + textures[3].Width, textures[0].Height);
        }

        public void updateScreen(GameTime gameTime)
        {
            kstate = GameStateManager.kstate;
            KeyboardState oldkstate = GameStateManager.oldkstate;

            if (kstate.IsKeyDown(Keys.Left) && oldkstate.IsKeyUp(Keys.Left))
            {
                wantToQuitChoice = Choice.Yes;
            }

            if (kstate.IsKeyDown(Keys.Right) && oldkstate.IsKeyUp(Keys.Right))
            {
                wantToQuitChoice = Choice.No;
            }

            if (kstate.IsKeyDown(Keys.Enter) && oldkstate.IsKeyUp(Keys.Enter))
            {
                if (wantToQuitChoice == Choice.Yes)
                {
                    GameStateManager.Default.mainMenuScreen.mainMenuTheme.fadeUp(3);
                    GameStateManager.Default.currentGameState = GameState.MainMenu;
                    GameStateManager.Default.reallyWantToQuit = false;
                }
                else
                {
                    GameStateManager.Default.reallyWantToQuit = false;
                }
            }



        }

        public void drawScreen(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(textures[0], positions[0], Color.White);
            spriteBatch.Draw(textures[1], positions[1], Color.White);
            spriteBatch.Draw(textures[2], positions[2], Color.White);
            spriteBatch.Draw(textures[3], positions[3], Color.White);
            spriteBatch.Draw(textures[4], positions[4], Color.White);
            spriteBatch.Draw(textures[5], positions[5], Color.White);
            spriteBatch.Draw(textures[6], positions[6], Color.White);
            spriteBatch.Draw(textures[7], positions[7], Color.White);

            if (wantToQuitChoice == Choice.Yes)
                spriteBatch.Draw(textures[8], positions[8], Color.White);
            else
                spriteBatch.Draw(textures[9], positions[9], Color.White);

            spriteBatch.End();
        }
    }
}
