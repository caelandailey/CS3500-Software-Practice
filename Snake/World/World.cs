using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class World
    {

        public static int playerID;
        public static string playerName;
        public static int worldSizeX;
        public static int worldSizeY;

        private int[][] grid;



        private static Dictionary<int, Food> foods;
        private static Dictionary<int, Snake> snakes;

        // Example of world method might be...

        /// <summary>
        /// Updates the game world for one time tick (frame).
        /// Moves snakes forward in their direction.
        /// Eats food.
        /// Kills snakes, and recycles them.
        /// </summary>
        public static void Update()
        {
            
        }

        public static void AddFood(Food food)
        {
            foods.Add(food.ID, food);
        }

        public static void AddSnake(Snake snake)
        {
            snakes.Add(snake.ID, snake);
        }

        public static void RemoveSnake(int id)
        {
            if (snakes.ContainsKey(id))
            {
                snakes.Remove(id);
            }
        }
        
        public static void RemoveFood(int id)
        {
            if (foods.ContainsKey(id))
            {
                foods.Remove(id);
            }
        }


    }
}
