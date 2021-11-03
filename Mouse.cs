using System;
using CatsAndMice;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CatsAndMiceEnun
{
    class Mouse:GameItem,IMovableItem
    {
        public Mouse()
        {
            this.Coords = null;
            this.MyValue = 0;

        }
        public Mouse(ItemCoordinates coordenadas, int valor)
        {
            this.Coords = coordenadas;
            this.MyValue = valor;

        }
        public Direction CurrentDirection { get; set; }
        public virtual void ChangeDirection()
        {
           
        }
    }
    }

