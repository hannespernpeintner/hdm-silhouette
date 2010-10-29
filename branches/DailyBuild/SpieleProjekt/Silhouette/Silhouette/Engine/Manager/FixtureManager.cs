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
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;

namespace Silhouette.Engine.Manager
{
    public static class FixtureManager
    {
        public static List<Fixture> TextureToPolygon(Texture2D texture, float density)
        {
            uint[] data = new uint[texture.Width * texture.Height];
            texture.GetData(data);
            Vertices vertices = PolygonTools.CreatePolygon(data, texture.Width, texture.Height, true);
            Vector2 scale = new Vector2(0.07f, 0.07f);
            vertices.Scale(ref scale);

            List<Vertices> tempList = BayazitDecomposer.ConvexPartition(vertices);
            List<Fixture> combine = FixtureFactory.CreateCompoundPolygon(Level.Physics, tempList, density);
            return combine;
        }
    }
}
