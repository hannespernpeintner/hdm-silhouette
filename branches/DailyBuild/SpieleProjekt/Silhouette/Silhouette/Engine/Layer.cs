﻿using System;
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
        private string _name;
        [DisplayName("Name"), Category("General")]
        [Description("The name of the layer.")]
        public string name { get { return _name; } set { _name = value; } }

        private bool _isVisible;
        [DisplayName("Visible"), Category("General")]
        [Description("Defines wether or not the layer is visible.")]
        public bool isVisible { get { return _isVisible; } set { _isVisible = value; } }

        private Vector2 scrollSpeed;
        [DisplayName("ScrollSpeed"), Category("General")]
        [Description("The Scroll Speed relative to the main camera. The X and Y components are interpreted as factors, " +
        "so Vector2.One means same scrolling speed as the main camera. To be used for parallax scrolling.")]
        public Vector2 ScrollSpeed { get { return scrollSpeed; } set { scrollSpeed = value; } }

        private List<LevelObject> _loList;
        [DisplayName("Level Objects"), Category("Objects")]
        [Description("The objects of the Layer.")]
        public List<LevelObject> loList { get { return _loList; } }

        public Layer()
        {
            scrollSpeed = Vector2.One;
            isVisible = true;
            _loList = new List<LevelObject>();
        }

        public void initializeLayer() { }

        public void loadLayer()
        {
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
