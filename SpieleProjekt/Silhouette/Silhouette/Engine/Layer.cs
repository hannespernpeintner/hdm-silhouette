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

namespace Silhouette.Engine
{
    [Serializable]
    public partial class Layer
    {
        public string name;
        public bool isVisible;

        [NonSerialized]
        private Vector2 scrollSpeed;

        [DisplayName("ScrollSpeed"), Category("General")]
        [Description("The Scroll Speed relative to the main camera. The X and Y components are interpreted as factors, " +
        "so Vector2.One means same scrolling speed as the main camera. To be used for parallax scrolling.")]
        public Vector2 ScrollSpeed
        {
            get { return scrollSpeed; }
            set { scrollSpeed = value; }
        }

        private List<LevelObject> _loList;

        [DisplayName("Level Objects"), Category("Objects")]
        [Description("The objects of the Layer.")]
        public List<LevelObject> loList { get { return _loList; } }

        [NonSerialized]
        public Texture2D[,] layerTexture;

        public string[,] assetName;
        public int width, height;

        public Layer()
        {
            scrollSpeed = Vector2.One;
            isVisible = true;
            _loList = new List<LevelObject>();
        }

        public void initializeLayer()
        {
            layerTexture = new Texture2D[width, height];
        }

        public void loadLayer()
        {
            for(int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    if(assetName[x,y] != null) 
                        layerTexture[x,y] = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/Layer/" + assetName[x,y]);
                }

            foreach (LevelObject lo in loList)
            {
                lo.LoadContent();
            }
        }

        public void updateLayer(GameTime gameTime)
        {
            foreach (LevelObject lo in loList)
            {
                lo.Update(gameTime);
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
            foreach (LevelObject lo in loList)
            {
                if (lo is DrawableLevelObject)
                {
                    DrawableLevelObject dlo = (DrawableLevelObject)lo;
                    dlo.Draw(spriteBatch);
                }
            }
        }
    }
}
