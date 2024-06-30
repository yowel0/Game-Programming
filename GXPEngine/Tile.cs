using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    internal class Tile : Sprite
    {
        public Tile() : base("Tile.png")
        {
            SetScaleXY(2);
        }
    }
}
