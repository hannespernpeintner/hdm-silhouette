﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silhouette.GameMechs
{
    public partial class Deco
    {
        public override string getPrefix()
        {
            return "Deco_";
        }

        public override bool contains(Microsoft.Xna.Framework.Vector2 worldPosition)
        {
            throw new NotImplementedException();
        }
    }
}
