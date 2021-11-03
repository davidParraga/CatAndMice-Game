using System;
using CatsAndMice;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CatsAndMiceEnun
{
    class Poisson:GameItem 
    {
       public Poisson() {

            this.Coords=null;
            this.MyValue = 0;
        }
        public Poisson(ItemCoordinates coordenadas2, int valor2) {
            this.Coords = coordenadas2;
            this.MyValue = valor2;
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
