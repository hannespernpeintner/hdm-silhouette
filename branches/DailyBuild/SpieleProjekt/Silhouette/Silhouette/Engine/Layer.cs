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

using Silhouette.GameMechs;

namespace Silhouette.Engine.Screens
{
    [Serializable]
    class Layer
    {
        string name;
        bool isVisible;
        Vector2 scrollSpeed;

        List<LevelObject> loList;
        List<DrawableLevelObject> dloList;

        public void initialiseLayer()
        {
            loList = new List<LevelObject>();
            dloList = new List<DrawableLevelObject>();
        }

        public void loadLayer()
        {
            foreach (LevelObject lo in loList)
            {
                lo.LoadContent();
            }
            foreach (DrawableLevelObject dlo in dloList)
            {
                dlo.LoadContent();
            }
        }

        public void updateLayer(GameTime gameTime)
        {
            foreach (LevelObject lo in loList)
            {
                lo.Update(gameTime);
            }
            foreach (DrawableLevelObject dlo in dloList)
            {
                dlo.Update(gameTime);
            }
        }

        public void drawLayer(SpriteBatch spriteBatch)
        {
            if (!isVisible)
                return;

            foreach (DrawableLevelObject dlo in dloList)
            {
                dlo.Draw(spriteBatch);
            }
        }
    }
}
