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

using Silhouette.Engine.Effects;

namespace Silhouette.Engine
{
    public partial class Layer
    {
        public Level level;

        public void initializeInEditor() { }

        public void loadContentInEditor(GraphicsDevice graphics, ContentManager content)
        {
            //particleRenderer = new ParticleRenderer();
            Rt = new RenderTarget2D(graphics, graphics.Viewport.Width, graphics.Viewport.Height);

            foreach (LevelObject lo in loList)
            {
                if (lo is DrawableLevelObject)
                {
                    DrawableLevelObject dlo = (DrawableLevelObject)lo;
                    dlo.loadContentInEditor(graphics);
                }
            }

            Effects = new List<EffectObject>();

            foreach (EffectObject eo in Effects)
            {
                eo.Initialise();
                eo.loadContentInEditor(graphics, content);
            }
        }

        public void drawInEditor(SpriteBatch spriteBatch)
        {
            if (!isVisible)
                return;

            foreach (LevelObject lo in loList)
            {
                if (lo is DrawableLevelObject)
                {
                    DrawableLevelObject dlo = (DrawableLevelObject)lo;
                    if (dlo.isVisible)
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
