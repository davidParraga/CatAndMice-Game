using CatsAndMice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CatsAndMiceEnun
{
    class Fruit : GameItem
    {
        public Fruit()
        {
            this.Coords = null;
            this.MyValue = 0;

        }
        public Fruit(ItemCoordinates coordenadas, int valor)
        {
            this.Coords = coordenadas;
            this.MyValue = valor;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}

