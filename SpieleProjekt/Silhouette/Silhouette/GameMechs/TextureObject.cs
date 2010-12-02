using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Silhouette;
using Silhouette.Engine;
using Silhouette.Engine.Manager;
using Silhouette.GameMechs;
using System.IO;

//Physik-Engine Klassen
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;

namespace Silhouette.GameMechs
{
    [Serializable]
    public class TextureObject : DrawableLevelObject
    {
        [NonSerialized]
        public Texture2D texture;

        Matrix transform;
        Rectangle boundingBox;
        Vector2[] polygon;

        public TextureObject(string path)
        {
            this.fullPath = path;
            this.assetName = Path.GetFileNameWithoutExtension(path);
            this.scale = Vector2.One;
            this.rotation = 0f;
            this.origin = Vector2.Zero;
            this.polygon = new Vector2[4];
        }

        public override void Initialise() {}

        public override void LoadContent()
        {
            texture = GameLoop.gameInstance.Content.Load<Texture2D>("Sprites/" + assetName);
        }

        public override void Update(GameTime gameTime) {}

        public override void Draw(SpriteBatch spriteBatch)
        {
            Color color = Color.White;
            if (mouseOn) color = new Color(255, 0, 0, 150);
            spriteBatch.Draw(texture, position, null, color, rotation, origin, scale, SpriteEffects.None, 1);
        }


        public override void loadContentInEditor(GraphicsDevice graphics)
        {
            if (texture == null)
            {
                FileStream file = FileManager.LoadConfigFile(fullPath);
                texture = Texture2D.FromStream(graphics, file);
            }

            transformed();
        }

        public override string getPrefix()
        {
            return "TextureObject_";
        }

        public override LevelObject clone()
        {
            throw new NotImplementedException();
        }

        public override void transformed()
        {
            if (texture == null)
                return;

            transform =
                Matrix.CreateTranslation(new Vector3(origin.X, origin.Y, 0.0f)) *
                Matrix.CreateScale(scale.X, scale.Y, 1) *
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateTranslation(new Vector3(position, 0.0f));

            Vector2 leftTop = new Vector2(0, 0);
            Vector2 rightTop = new Vector2(texture.Width, 0);
            Vector2 leftBottom = new Vector2(0, texture.Height);
            Vector2 rightBottom = new Vector2(texture.Width, texture.Height);

            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            polygon[0] = leftTop;
            polygon[1] = rightTop;
            polygon[3] = leftBottom;
            polygon[2] = rightBottom;

            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                                      Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                                      Vector2.Max(leftBottom, rightBottom));

            boundingBox = new Rectangle((int)min.X, (int)min.Y,
                                 (int)(max.X - min.X), (int)(max.Y - min.Y));
        }

        public override bool contains(Vector2 worldPosition)
        {
            return boundingBox.Contains((int)worldPosition.X, (int)worldPosition.Y);
        }

        public override void drawSelectionFrame(SpriteBatch spriteBatch, Matrix matrix)
        {
            Primitives.Instance.drawPolygon(spriteBatch, polygon, Color.Yellow, 2);
            foreach (Vector2 p in polygon)
            {
                Primitives.Instance.drawCircleFilled(spriteBatch, p, 4, Color.Yellow);
            }
            Vector2 origin = new Vector2(position.X + texture.Width / 2, position.Y + texture.Height / 2);
            Primitives.Instance.drawBoxFilled(spriteBatch, origin.X - 5, origin.Y - 5, 10, 10, Color.Yellow);
        }
    }
}
