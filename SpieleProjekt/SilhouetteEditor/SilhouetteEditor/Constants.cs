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
        private const int _CameraMovingSpeed = 10;
        public static int CameraMovingSpeed { get { return _CameraMovingSpeed; } }
        private static Color _ColorSelectionBox = new Color(255, 255, 255, 109);
        public static Color ColorSelectionBox { get { return _ColorSelectionBox; } }
        private static Color _ColorFixtures = new Color(0, 0, 192, 145);
        public static Color ColorFixtures { get { return _ColorFixtures; } }
    }
}
