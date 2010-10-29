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


namespace Silhouette.Engine
{
    static class FontManager
    { 
        //Sascha: Diese Klasse dient dazu, den Zugriff auf Fonts global zu kapseln um Debugging zu vereinfachen
        public static SpriteFont Arial; //Sascha: Kann jetzt einfach über FontManager.Arial überall aufgerufen werden

        public static void loadFonts()
        {
            Arial = GameLoop.gameInstance.Content.Load<SpriteFont>("Fonts/Arial");
        }
    }
}
