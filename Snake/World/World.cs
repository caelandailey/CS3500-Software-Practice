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
    public class World : Panel
    {

        // Determines the size in pixels of each grid cell in the world
        public const int pixelsPerCell = 5;

        private Dictionary<int, Food> foods;
        private Dictionary<int, Snake> snakes;

        private World world;
        public World()
        {
            foods = new Dictionary<int, Food>();
            snakes = new Dictionary<int, Snake>();
            width = 150;
            height = 150;

            // Setting this property to true prevents flickering
            this.DoubleBuffered = true;
        }

        public World(int w, int h) // Constructor for world
        {
            foods = new Dictionary<int, Food>();
            snakes = new Dictionary<int, Snake>();
            width = w;
            height = h;

            // Setting this property to true prevents flickering
            this.DoubleBuffered = true;
        }

        /// <summary>
        /// Pass in a reference to the world, so we can draw the objects in it
        /// </summary>
        /// <param name="_world"></param>
        public void SetWorld(World _world)
        {
            world = _world;
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

        


        /// <summary>
        /// Override the behavior when the panel is redrawn
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            //// If we don't have a reference to the world yet, nothing to draw.
            //if (drawWorld == null)
            //   return;

            using (SolidBrush drawBrush = new SolidBrush(Color.Black))
            {

                // Turn on anti-aliasing for smooth round edges
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Walls

                // Draw the top wall
                Rectangle topWall = new Rectangle(0, 0, width * pixelsPerCell, pixelsPerCell);
                e.Graphics.FillRectangle(drawBrush, topWall);

                // Draw the right wall
                Rectangle rightWall = new Rectangle((width - 1) * pixelsPerCell, 0, pixelsPerCell, height * pixelsPerCell);
                e.Graphics.FillRectangle(drawBrush, rightWall);

                // Draw the bottom wall
                Rectangle bottomWall = new Rectangle(0, (height - 1) * pixelsPerCell, width * pixelsPerCell, pixelsPerCell);
                e.Graphics.FillRectangle(drawBrush, bottomWall);

                // Draw the left wall
                Rectangle leftWall = new Rectangle(0, 0, pixelsPerCell, height * pixelsPerCell);
                e.Graphics.FillRectangle(drawBrush, leftWall);

                // Food
                foreach (KeyValuePair<int, Food> food in foods)
                {
                    Rectangle circle = new Rectangle(food.Value.loc.x, food.Value.loc.y, pixelsPerCell, pixelsPerCell);
                    e.Graphics.FillEllipse(drawBrush, circle);
                }
            }


            // Snake
            foreach (KeyValuePair<int, Snake> snake in snakes)
            {
                using (SolidBrush drawBrusher = new SolidBrush(snake.Value.color))
                {
                    foreach (Point p in snake.Value.vertices)
                    {
                        Point lastPoint = null;

                        if (ReferenceEquals(lastPoint, null)) // if first vertice
                        {
                            lastPoint = p;
                            continue;
                        }
                        if (lastPoint == p)
                        {
                            break;
                        }

                        Rectangle segment = new Rectangle(lastPoint.x, lastPoint.y, pixelsPerCell * (lastPoint.x - p.x), pixelsPerCell * (lastPoint.y - p.y));
                        e.Graphics.FillRectangle(drawBrusher, segment);

                        lastPoint = p;
                    }
                }


            }

            // Draw e??

        }
    }
}
