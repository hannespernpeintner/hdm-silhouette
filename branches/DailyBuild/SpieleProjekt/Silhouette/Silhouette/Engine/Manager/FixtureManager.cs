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

//Physik-Engine Klassen
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;
using FarseerPhysics.Common.ConvexHull;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using Silhouette.GameMechs;

namespace Silhouette.Engine.Manager
{
    public static class FixtureManager
    {
        public static List<Fixture> TextureToPolygon(Texture2D texture, BodyType bodyType, Vector2 position, float density)
        {
            uint[] data = new uint[texture.Width * texture.Height];
            texture.GetData(data);
            Vertices vertices = PolygonTools.CreatePolygon(data, texture.Width, texture.Height, true);
            Vector2 scale = new Vector2(0.01f, 0.01f);
            vertices.Scale(ref scale);

            List<Vertices> tempList = EarclipDecomposer.ConvexPartition(vertices);
            List<Vertices> toRemove = new List<Vertices>();
            foreach (Vertices item in tempList) { if (item.Count == 0) { toRemove.Add(item); } }
            foreach (Vertices item in toRemove) { tempList.Remove(item); }     

            List<Fixture> combine = FixtureFactory.CreateCompoundPolygon(Level.Physics, tempList, 1); 
            combine[0].Body.BodyType = bodyType;
            combine[0].Body.Position = ToMeter(position);
            return combine;
        }

        
        public static List<List<Fixture>> AnimationToPolygons(Animation animation)
        {
            List<List<Fixture>> polygons = new List<List<Fixture>>();

            foreach (Texture2D picture in animation.pictures)
            {
                polygons.Add(FixtureManager.TextureToPolygon(picture, BodyType.Static, Vector2.Zero, 1.0f));
            }

            return polygons;
        }
        
        public static Fixture CreateRectangle(float width, float height, Vector2 position, BodyType bodyType,float  density)
        {
            Vector2 size = ToMeter(new Vector2(width, height));
            Fixture fixture = FixtureFactory.CreateRectangle(Level.Physics, (float)size.X, (float)size.Y, density);
            fixture.Body.BodyType = bodyType;
            fixture.Body.Position = ToWorld(size.X, size.Y, ToMeter(position));
            return fixture;
        }

        public static Fixture CreateCircle(float radius, Vector2 position, BodyType bodyType, float density)
        {
            radius /= Level.PixelPerMeter;
            Fixture fixture = FixtureFactory.CreateCircle(Level.Physics, radius, density);
            fixture.Body.BodyType = bodyType;
            fixture.Body.Position = ToWorld(radius * 2, radius * 2, ToMeter(position));
            return fixture;
        }

        public static Fixture CreatePolygon(Vector2[] vertices, Vector2 position, BodyType bodyType, float density)
        {
            Vector2[] temp = new Vector2[vertices.Length];
            for(int i = 0; i < vertices.Length; i++)
            {
                temp[i] = ToMeter(vertices[i]);
            }
            Vertices vert = new Vertices(temp);
            Fixture fixture = FixtureFactory.CreatePolygon(Level.Physics, vert, density);
            fixture.Body.BodyType = bodyType;
            fixture.Body.Position = ToMeter(position);
            return fixture;
        }

        public static Vector2 ToMeter(Vector2 position)
        {
            return new Vector2((float)(position.X / Level.PixelPerMeter), (float)(position.Y / Level.PixelPerMeter));
        }

        public static Vector2 ToPixel(Vector2 position)
        {
            return new Vector2((float)(position.X * Level.PixelPerMeter), (float)(position.Y * Level.PixelPerMeter));
        }

        public static Vector2 ToWorld(float width, float height, Vector2 position)
        {
            Vector2 offSet = new Vector2((float)(width / 2), (float)(height / 2));
            return (position + offSet);
        }
    }
}
