using System;
using CatsAndMiceEnun;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsAndMice
{

    public class GameLogic : IGameLogic
    {

        public static readonly int MaxFila = 25;
        public static readonly int MaxColumna = 50;
        int i = 0;
        int i2 = 1;

        // Los elementos del juego se gestionan en varios objetos contenedores de datos 
        // que apuntan hacia datos comunes.
        // Ventaja: se facilita la impelemntación de la lógica del juego.
        // Inconveniente: Hay que mantener la coherencia entre los diferents contenedores, duplicando
        // las operaciones de inserción y borrado.

        // Panel del juego: contiene referencia a todos los datos.
        // Cada casilla o bien está vacía (contiene referencia a null) o bien
        // contiene una referencia al elemento del juego que está sobre ella.
        private IGameItem[,] board;

        // Lista de elementos del juego. Si se elimina o añade un elemnto en
        // esta lista también hay que elimanarlo/añadirlo en el panel de juego (board).
        private List<IGameItem> gameItems = new List<IGameItem>();

        // Personajes mouse 
        //private Mouse myMouse = new Mouse(new ItemCoordinates(20, 1), 0);
        private SmartMouse myMouse = new SmartMouse(new ItemCoordinates(20, 1), 0);

        // Gatos
        private List<Cat> myCats = new List<Cat>();

        private int stepCounter = 0;
        Boolean gameOver = false;



        // eventos del juego
        public event EventHandler<GameEventArgs> NewGameStepEventHandlers;
            
        /// <summary>
        /// Construye un tablero de filas x columnas
        /// </summary>
        /// <param name="filas"></param>
        /// <param name="columnas"></param>
        public GameLogic()
        {
            board = new GameItem[MaxFila, MaxColumna];
            AddItem(myMouse);
            FillBoard(4, 4);

            // REGISRTRO DEL MANEJADOR DE EVENTOS DEL MOUSE
            NewGameStepEventHandlers += myMouse.UpdateGame;
        }

        /// <summary>
        /// Devuelve true si en la (fila, columna) especificada no hay
        /// ningún elemento de juego.
        /// </summary>
        /// <param name="fila"></param>
        /// <param name="columna"></param>
        /// <returns></returns>
        public Boolean IsCellAvailable(int fila, int columna)
        {
            if (board[fila, columna] == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Añade un elemento al juego en la celda especificada en las coordenadas del
        /// argumento (item), siempre que (1) item != null, (2) la posición no esté ya
        /// ocupada por otro elemntoe y (3) el elemento no esté ya en el juego.
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(IGameItem item)
        {
            if (item != null && IsCellAvailable(item.Coords.Fila, item.Coords.Columna) == true)
            {
                board[item.Coords.Fila, item.Coords.Columna] = item;
                gameItems.Add(item);

                Console.WriteLine(item);

            }
        }


        /// <summary>
        /// Elimina un elemento del juego.
        /// </summary>
        /// <param name="item"></param>        
        public void RemoveItem(IGameItem item)
        {
            if (item is Fruit || item is Poisson)
            {
                board[item.Coords.Fila, item.Coords.Columna] = null;
                gameItems.Remove(item);
            }
        }
        /// <summary>
        /// Rellena el juego con un número determinado de frutas y venenos colocados en 
        /// posiciones aleatorias.
        /// </summary>
        public void FillBoard(int nFruits, int nPoissons)
        {
            for (int i = 0; i < nFruits; i++)
            {
                Random random = new Random();
                int num1 = random.Next(1, MaxFila);
                int num2 = random.Next(1, MaxColumna);
                ItemCoordinates aleatorias = new ItemCoordinates(num1, num2);
                Fruit newFruit = new Fruit(aleatorias, 1);
                if (IsCellAvailable(newFruit.Coords.Fila, newFruit.Coords.Columna) == true)
                {
                    AddItem(newFruit);
                } 
            }
            for (int i = 0; i < nPoissons; i++)
            {
                Random random = new Random();       
                int num1 = random.Next(1, MaxFila);
                int num2 = random.Next(1, MaxColumna);
                ItemCoordinates aleatorias = new ItemCoordinates(num1, num2);
                Poisson newPoisson = new Poisson(aleatorias, -1);
                if (IsCellAvailable(newPoisson.Coords.Fila, newPoisson.Coords.Columna) == true)
                {
                    AddItem(newPoisson);
                }
    
            }
        }

        /// <summary>
        /// Fija la dirección de movimiento del ratón.
        /// </summary>
        /// <param name="next"></param>
        public void SetMouseDirection(Direction next)
        {
            myMouse.CurrentDirection = next;

        }

        /// <summary>
        /// Devuelve un enumerador de los elementos del juego.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IGameItem> GetEnumerator()
        {
            foreach (var Item in gameItems)
            {
                yield return Item;
            }
        }

        /// <summary>
        /// Implementa la lógica del juego que se ejecuta en cada Tick de un temporizador.
        /// </summary>
        /// <returns>>= 0 si el juego puede continuar, un valor negativo si no se puede continuar</returns>
        public int ExecuteStep(GameMode mode)
        {
            // Actualiza posición del ratón de acuerdo con su dirección.

            ItemCoordinates salvarPosicion = myMouse.Coords;
            processCell();
            if (myMouse.MyValue < 0) { return -1; }
            if (mode == GameMode.AutonomousMouse)
            {
                NewGameStepEventHandlers(this, new GameEventArgs(gameItems));
                myMouse.ChangeDirection();
                updateMovablePosition(myMouse);
            }
            else
            {
                updateMovablePosition(myMouse);
            }
            int contador = 0;
            int contador2 = 0;
            foreach (var item in board)
            {
                if (item is Fruit)
                {
                    contador++;
                }
                if (item is Poisson)
                {
                    contador2++;
                }
            }
            if (contador < 4) { FillBoard(1, 0); }  //Siempre habrá 4 frutas
            if (contador2 < 4) { FillBoard(0, 1); } //Siempre habrá 4 venenos

            if (i2<=12) //añade gatos hasta un total de 12.
            {
                if (i == 20)
                {
                    Random random = new Random();
                    int num1 = random.Next(1, MaxFila);
                    int num2 = random.Next(1, MaxColumna);
                    ItemCoordinates aleatorias = new ItemCoordinates(num1, num2);
                    Cat newcat = new Cat(aleatorias, 0);
                    AddItem(newcat);
                    myCats.Add(newcat);
                    i = 0;
                    i2++;
                }
                i++;
            }

            foreach(var item in myCats)
            {
                     item.ChangeDirection();
                    updateMovablePosition(item);
            }

            return 0;
        }
    
        /// <summary>
        /// Actualiza posición del elemento moviéndolo una fila o columna dependiendo
        /// de la dirección de su movimiento.
        /// Cuando el elemento llega a un límite del tablero aparece por el lado contrario.
        /// </summary>
        /// <param name="item"></param>
        private void updateMovablePosition(IMovableItem item)
        {
            if (item.CurrentDirection == Direction.Up)
            {
                item.Coords.Fila = (item.Coords.Fila - 1);
                if (item.Coords.Fila <= 0) item.Coords.Fila = MaxFila - 1;
            }
            else if (item.CurrentDirection == Direction.Down)
            {
                item.Coords.Fila = (item.Coords.Fila + 1) % MaxFila;
            }
            else if (item.CurrentDirection == Direction.Rigth)
            {
                item.Coords.Columna = (item.Coords.Columna + 1) % MaxColumna;
            }
            else if (item.CurrentDirection == Direction.Left)
            {
                item.Coords.Columna = (item.Coords.Columna - 1);
                if (item.Coords.Columna <= 0) item.Coords.Columna = MaxColumna - 1;
            }
        }

        /// <summary>
        /// Actualiza el juego en función de lo que hay en la celda donde está el ratón.
        /// Si no hay nada, no hace nada.
        /// Si hay fruta o veneno, suma al "valor" del ratón el valor de la fruta (positivo) o del veneno (negativo) y
        /// elimina la fruta o el veneno.
        /// Si hay un gato pone el valor del ratón en -1.
        /// </summary>
        /// <returns> El valor almacenado en el ratón. </returns>

        private int processCell()
        {
            
            foreach (var item in gameItems)
            {
                if (myMouse.Coords.Fila==item.Coords.Fila && 
                    myMouse.Coords.Columna==item.Coords.Columna)
                {
                    if (item is Fruit)
                    {
                        myMouse.MyValue += item.MyValue;
                        RemoveItem(item);
                        break;
                    }
                    if (item is Poisson)
                    {
                        myMouse.MyValue += item.MyValue;
                        RemoveItem(item);
                        break;
                    }
                    if (item is Cat)
                    {
                        myMouse.MyValue = -1;
                        break;
                    }
                }
            }
            return myMouse.MyValue;
        }
    }
}
