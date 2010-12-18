using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Silhouette;
using Silhouette.Engine;
using Silhouette.Engine.Manager;
using Silhouette.GameMechs;
using SilhouetteEditor.Forms;
using System.IO;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SilhouetteEditor
{
    public static class Constants
    {
        private const int _CameraMovingSpeed = 1500;
        public static int CameraMovingSpeed { get { return _CameraMovingSpeed; } }
        private const int _DefaultPathItemLineWidth = 4;
        public static int DefaultPathItemLineWidth { get { return _DefaultPathItemLineWidth; } }
        private static Color _ColorSelectionBox = new Color(0, 255, 0, 50);
        public static Color ColorSelectionBox { get { return _ColorSelectionBox; } }
        private static Color _ColorFixtures = new Color(0, 0, 192, 145);
        public static Color ColorFixtures { get { return _ColorFixtures; } }
        private static Color _ColorPrimitives = new Color(0, 0, 0, 255);
        public static Color ColorPrimitives { get { return _ColorPrimitives; } }
        private static Color _ColorEvents = new Color(0, 200, 0, 145);
        public static Color ColorEvents { get { return _ColorEvents; } }
        private static Color _ColorMouseOn = new Color(255, 255, 0, 90);
        public static Color ColorMouseOn { get { return _ColorMouseOn; } }
    }
}
