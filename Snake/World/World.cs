using MyPoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public class World
    {

        public static string playerName;
        public static int playerID;

        public static int worldSizeX;
        public static int worldSizeY;


        public static Dictionary<int,Point> foods;
        private Dictionary<int, List<Point>> snakes;

        private Dictionary<int, string> snakeNames;

        // Example of world method might be...

        /// <summary>
        /// Updates the game world for one time tick (frame).
        /// Moves snakes forward in their direction.
        /// Eats food.
        /// Kills snakes, and recycles them.
        /// </summary>
        public void Update()
        {
            //Update location for one frame
            //Check for collision of some sort
            //Action based on collision or not
        }

        public void addFood(Food food)
        {
            foods[food.ID] = food.Point;
        }

        public void addSnake(Snake snake)
        {
            snakes[snake.ID] = snake.vertices;
            snakeNames[snake.ID] = snake.name;
        }


    }
}
