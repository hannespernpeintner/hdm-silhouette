using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Silhouette.Engine.Manager;
using Silhouette.Engine.Screens;
using System.ComponentModel;

//Physik-Engine Klassen
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;

namespace Silhouette.Engine
{
    public partial class EventLayer
    {
        private Vector2 _ScrollSpeed;

        [DisplayName("ScrollSpeed"), Category("General")]
        [Description("The Scroll Speed relative to the main camera. The X and Y components are interpreted as factors, " +
        "so Vector2.One means same scrolling speed as the main camera. To be used for parallax scrolling. The Event Layer should have the same scroll speed as the player layer.")]

        public Vector2 ScrollSpeed
        {
            get { return _ScrollSpeed; }
            set { _ScrollSpeed = value; }
        }
        public void Initialize()
        { 
        
        }

        public void LoadContent()
        { 
        
        }

        public void updateEventLayer(GameTime gameTime)
        { 
        
        }
    }
}
