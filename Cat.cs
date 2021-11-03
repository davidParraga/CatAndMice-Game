using System;
using CatsAndMice;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CatsAndMiceEnun
{
    class Cat : GameItem, IMovableItem

    {
        int i = 1;
        int a = 1;
        public Cat() {
            this.Coords = null;
            MyValue = 0;
        }
        public Cat(ItemCoordinates coordenadas3, int valor3) {
            this.Coords = coordenadas3;
            this.MyValue = valor3; 
        }

        public void ChangeDirection()
        {
            if (a % 2 ==0)
            {
                this.CurrentDirection = Direction.Down;
                if (Coords.Fila > 25) { Coords.Fila = 0; }
            }
            else
            {
                if (i <= 20)
                {
                    this.CurrentDirection = Direction.Rigth;
                    if (Coords.Columna > 50) { Coords.Columna = 0; }
                }
                if (i > 20)
                {
                    this.CurrentDirection = Direction.Left;
                    if (Coords.Columna < 0) { Coords.Columna = 50; }
                }
                i++;
                if (i > 40) { i = 1; }
            }
            a++;
        }
            public Direction CurrentDirection { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}

