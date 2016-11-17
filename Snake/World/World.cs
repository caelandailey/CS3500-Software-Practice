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

        private static int foodCount = 0;
        public static int worldSizeX;
        public static int worldSizeY;

        private static Dictionary<Point,int> grid;


        private static Dictionary<int, Food> foods;
        private static Dictionary<int, Snake> snakes;

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

        public static void addFood(Food food)
        {
            foods.Add(foodCount, food);
        }

        public static void addSnake(Snake snake)
        {
            snakes.Add(playerID, snake);
        }

        


    }
}
