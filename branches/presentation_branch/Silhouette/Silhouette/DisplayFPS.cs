using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Silhouette.Engine;

namespace Silhouette
{
    public class DisplayFPS : Microsoft.Xna.Framework.DrawableGameComponent
    {
        /* Sascha:
         * Klasse zum Anzeigen der aktuellen Frames per Second (FPS). Klasse wurde als DrawableGameComponent realisiert,
         * weil die Integration zu Level so einfacher zu machen war.
        */

        private float updateInterval = 1.0f;
        private float timeSinceLastUpdate = 0.0f;
        private float frameCounter = 0.0f;
        private float _fps = 0.0f;

        SpriteBatch sb;

        public float fps
        {
            get { return _fps; }
            set { _fps = value; }
        }

        public DisplayFPS(Game game)
            : base(game)
        {

        }

        protected override void LoadContent()
        {
            sb = new SpriteBatch(GameLoop.gameInstance.GraphicsDevice);
            base.LoadContent();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            frameCounter++;
            timeSinceLastUpdate += elapsedTime;

            if (timeSinceLastUpdate > updateInterval)
            {
                fps = (int)(frameCounter / timeSinceLastUpdate);
                frameCounter = 0;
                timeSinceLastUpdate -= updateInterval;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            sb.Begin();
            sb.DrawString(FontManager.Arial, fps.ToString(), new Vector2(10, 10), Color.Black);
            sb.End();
            base.Draw(gameTime);
        }
    }
}
