using DrawingPanel;
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
    public class World
    {
       
        // Determines the size in pixels of each grid cell in the world
        public const int pixelsPerCell = 5;

        public static Dictionary<int, Food> foods;
        public static Dictionary<int, Snake> snakes;

        

        public World(int w, int h)
        {
            foods = new Dictionary<int, Food>();
            snakes = new Dictionary<int, Snake>();
            width = w;
            height = h;

            // Setting this property to true prevents flickering
            this.DoubleBuffered = true;
        }

        // Width of the world in cells (not pixels)
        public int width
        {
            get;
            private set;
        }

        // Height of the world in cells (not pixels)
        public int height
        {
            get;
            private set;
        }

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
       
        public void setWorldID(int ID)
        {
            
        }

    

        public void drawWalls(PaintEventArgs e)
        {
            //// If we don't have a reference to the world yet, nothing to draw.
            if (drawWorld == null)
                return;

            using (SolidBrush drawBrush = new SolidBrush(Color.Black))
            {

                // Turn on anti-aliasing for smooth round edges
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Draw the top wall
                Rectangle topWall = new Rectangle(0, 0, width * DrawWorld.pixelsPerCell, DrawWorld.pixelsPerCell);
                e.Graphics.FillRectangle(drawBrush, topWall);

                // Draw the right wall
                Rectangle rightWall = new Rectangle((width - 1) * DrawWorld.pixelsPerCell, 0, DrawWorld.pixelsPerCell, height * DrawWorld.pixelsPerCell);
                e.Graphics.FillRectangle(drawBrush, rightWall);

                // Draw the bottom wall
                Rectangle bottomWall = new Rectangle(0, (height - 1) * DrawWorld.pixelsPerCell, width * DrawWorld.pixelsPerCell, DrawWorld.pixelsPerCell);
                e.Graphics.FillRectangle(drawBrush, bottomWall);

                // Draw the left wall
                Rectangle leftWall = new Rectangle(0, 0, DrawWorld.pixelsPerCell, height * DrawWorld.pixelsPerCell);
                e.Graphics.FillRectangle(drawBrush, leftWall);

            }

            drawWorld.Draw(e);
        }
    }
}
