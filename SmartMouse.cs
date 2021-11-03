using System;
using CatsAndMice;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CatsAndMiceEnun;

namespace CatsAndMice
{
    class SmartMouse : Mouse, IGameObserver
    {
        GameEventArgs lastEvent; //Declaramos un GameEventArgs que en el que guardaremos lo último
                                 //que se ha añadido. 

        public SmartMouse(ItemCoordinates coordenadas5, int valor5)
        {
            this.Coords = coordenadas5;
            this.MyValue = valor5;
        }

        public override void ChangeDirection()
        {
            if (lastEvent != null)
            {
                foreach (var item in lastEvent)
                {

                    if (item is Fruit)
                    {
                        if (item.Coords.Fila < this.Coords.Fila)
                        {
                            this.CurrentDirection = Direction.Up;
                            break;
                        }
                        if (item.Coords.Fila > this.Coords.Fila)
                        {
                            this.CurrentDirection = Direction.Down;
                            break;
                        }
                        if (item.Coords.Columna < this.Coords.Columna)
                        {
                            this.CurrentDirection = Direction.Left;
                            break;
                        }
                        if (item.Coords.Columna > this.Coords.Columna)
                        {
                            this.CurrentDirection = Direction.Rigth;
                            break;
                        }
                    }
                }
            }
        }


        public void UpdateGame(object sender, GameEventArgs e)
        {
            lastEvent = e;
        }
    }
}

        
    

    

