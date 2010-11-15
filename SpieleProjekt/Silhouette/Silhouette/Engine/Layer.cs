using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Silhouette.GameMechs;

namespace Silhouette.Engine
{
    public partial class Layer
    {
        [XmlAttribute()]
        public string name;
        [XmlAttribute()]
        public bool isVisible;

        public Vector2 scrollSpeed;

        public List<LevelObject> loList;
        public List<DrawableLevelObject> dloList;

        public Texture2D[,] layerTexture;
        public string[,] assetName;
        public int width, height;

        public Layer()
        {
            scrollSpeed = Vector2.One;
            isVisible = true;
            loList = new List<LevelObject>();
            dloList = new List<DrawableLevelObject>();
        }

        public void initializeLayer()
        {
            layerTexture = new Texture2D[width, height];
            assetName = new string[width, height];
        }

        public void loadLayer()
        {
            for(int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    layerTexture[x,y] = GameLoop.gameInstance.Content.Load<Texture2D>(assetName[x,y]);
                }

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

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    if(layerTexture[x,y] != null)
                        spriteBatch.Draw(layerTexture[x, y], new Vector2(x * GameSettings.Default.resolutionWidth, y * GameSettings.Default.resolutionHeight), Color.White);
                }
            foreach (DrawableLevelObject dlo in dloList)
            {
                dlo.Draw(spriteBatch);
            }
        }
    }
}
