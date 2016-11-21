using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class World
    {
        private int worldWidth;
        private int worldHeight;

        private int[][] grid;

        public World()
        {
            foods = new Dictionary<int, Food>();
            snakes = new Dictionary<int, Snake>();
        }

        public static Dictionary<int, Food> foods;
        public static Dictionary<int, Snake> snakes;

        // Example of world method might be...

        /// <summary>
        /// Updates the game world for one time tick (frame).
        /// Moves snakes forward in their direction.
        /// Eats food.
        /// Kills snakes, and recycles them.
        /// </summary>
        public void Update()
        {
            
        }

        public void AddFood(Food food)
        {
            //foods.Add(food.ID, food);
            foods[food.ID] = food;
        }

        public void AddSnake(Snake snake)
        {
            if (object.ReferenceEquals(null, snake))
            {
                return;
            }
            snakes[snake.ID] = snake;
            
        }

        public void RemoveSnake(int id)
        {
            if (snakes.ContainsKey(id))
            {
                snakes.Remove(id);
            }
        }
        
        public void RemoveFood(int id)
        {
            if (foods.ContainsKey(id))
            {
                foods.Remove(id);
            }
        }

        public void setWorldWidth(int width)
        {
            worldWidth = width;
        }
       
        public void setWorldID(int ID)
        {

        }

        public void setWorldHeight(int height)
        {

        }

        public void draw()
        {

        }

    }
}
