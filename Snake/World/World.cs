using SnakeGame;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public class World : Panel
    {
        private int worldWidth;
        private int worldHeight;

        private int[][] grid;

        public World()
        {
            foods = new Dictionary<int, Food>();
            snakes = new Dictionary<int, Snake>();

            // Setting this property to true prevents flickering
            this.DoubleBuffered = true;
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

        public void drawWalls(PaintEventArgs e)
        {
            //// If we don't have a reference to the world yet, nothing to draw.
            //if (world == null)
            //    return;

            using (SolidBrush drawBrush = new SolidBrush(Color.Black))
            {

                // Turn on anti-aliasing for smooth round edges
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Draw the top wall
                Rectangle topWall = new Rectangle(0, 0, worldWidth * DrawWorld.pixelsPerCell, DrawWorld.pixelsPerCell);
                e.Graphics.FillRectangle(drawBrush, topWall);

                // Draw the right wall
                Rectangle rightWall = new Rectangle((worldWidth - 1) * DrawWorld.pixelsPerCell, 0, DrawWorld.pixelsPerCell, worldHeight * DrawWorld.pixelsPerCell);
                e.Graphics.FillRectangle(drawBrush, rightWall);

                // Draw the bottom wall
                Rectangle bottomWall = new Rectangle(0, (worldHeight - 1) * DrawWorld.pixelsPerCell, worldWidth * DrawWorld.pixelsPerCell, DrawWorld.pixelsPerCell);
                e.Graphics.FillRectangle(drawBrush, bottomWall);

                // Draw the left wall
                Rectangle leftWall = new Rectangle(0, 0, DrawWorld.pixelsPerCell, worldHeight * DrawWorld.pixelsPerCell);
                e.Graphics.FillRectangle(drawBrush, leftWall);

            }
        }

    }
}
