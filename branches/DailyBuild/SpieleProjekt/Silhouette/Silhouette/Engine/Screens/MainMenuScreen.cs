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


namespace Silhouette.Engine.Screens
{
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
        MenuState menuState;

        Texture2D[] textures;
        Vector2[] positions;

        KeyboardState kstate;
        KeyboardState oldkstate;

        public void initializeScreen()
        {
            menuState = MenuState.NewGame;

            textures = new Texture2D[14];
            positions = new Vector2[14];
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

            positions[0] = new Vector2(0, 0);
            positions[1] = new Vector2(textures[0].Width, 0);
            positions[2] = new Vector2(textures[0].Width + textures[1].Width, 0);
            positions[3] = new Vector2(textures[0].Width, textures[1].Height);
            positions[4] = new Vector2(textures[0].Width, textures[1].Height);
            positions[5] = new Vector2(textures[0].Width, textures[1].Height + textures[3].Height);
            positions[6] = new Vector2(textures[0].Width, textures[1].Height + textures[3].Height);
            positions[7] = new Vector2(textures[0].Width, textures[1].Height + (2 * textures[3].Height));
            positions[8] = new Vector2(textures[0].Width, textures[1].Height + (2 * textures[3].Height));
            positions[9] = new Vector2(textures[0].Width, textures[1].Height + (3 * textures[3].Height));
            positions[10] = new Vector2(textures[0].Width, textures[1].Height + (3 * textures[3].Height));
            positions[11] = new Vector2(textures[0].Width, textures[1].Height + (4 * textures[3].Height));
            positions[12] = new Vector2(textures[0].Width, textures[1].Height + (4 * textures[3].Height));
            positions[13] = new Vector2(textures[0].Width, textures[1].Height + (5 * textures[3].Height));
        }

        public void updateScreen(GameTime gameTime) 
        {
            kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Down) && oldkstate.IsKeyUp(Keys.Down))
            {
                if (menuState == MenuState.Exit)
                    menuState = MenuState.NewGame;
                else
                {
                    menuState++;
                }
            }

            if (kstate.IsKeyDown(Keys.Up) && oldkstate.IsKeyUp(Keys.Up))
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
                
                }
                if (menuState == MenuState.Exit)
                    GameLoop.gameInstance.Exit();
            }

            oldkstate = kstate;
        }

        public void drawScreen(SpriteBatch spriteBatch) 
        {
            spriteBatch.Begin();
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

            spriteBatch.End();
        }
    }
}
