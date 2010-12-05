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
using System.ComponentModel;
using Microsoft.Xna.Framework.Media;
using Silhouette.GameMechs;
using Silhouette.Engine.Manager;

namespace Silhouette.Engine
{
    public partial class Layer
    {
        public void initializeInEditor()
        {
            layerTexture = new Texture2D[width, height];
            assetName = new string[width, height];
            assetFullPath = new string[width, height];
        }

        public void loadContentInEditor(GraphicsDevice graphics)
        {
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    if (assetName[x, y] != null)
                    {
                        FileStream file = FileManager.LoadConfigFile(assetFullPath[x,y]);
                        layerTexture[x, y] = Texture2D.FromStream(graphics, file);
                    }
                }

            foreach (LevelObject lo in loList)
            {
                if (lo is DrawableLevelObject)
                {
                    DrawableLevelObject dlo = (DrawableLevelObject)lo;
                    dlo.loadContentInEditor(graphics);
                }
            }
        }

        public void drawInEditor(SpriteBatch spriteBatch)
        {
            if (!isVisible)
                return;

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    if (layerTexture[x, y] != null)
                        spriteBatch.Draw(layerTexture[x, y], new Vector2(x * GameSettings.Default.resolutionWidth, y * GameSettings.Default.resolutionHeight), Color.White);
                }
            foreach (LevelObject lo in loList)
            {
                if (lo is DrawableLevelObject)
                {
                    DrawableLevelObject dlo = (DrawableLevelObject)lo;
                    dlo.drawInEditor(spriteBatch);
                }
            }
        }

        public LevelObject getItemAtPosition(Vector2 worldPosition)
        {
            foreach (LevelObject lo in loList)
            {
                if (lo.contains(worldPosition))
                    return lo;
            }
            return null;
        }

        public string getNextObjectNumber()
        { 
            int i = loList.Count() + 1;
            return i.ToString("0000");
        }
    }
}
