
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
        

          

        private Object foodLock;

        private Object snakeLock;
  
        public World()
        {
            foods = new Dictionary<int, Food>();
            snakes = new Dictionary<int, Snake>();
            snakeLock = new Object();
            foodLock = new Object();
            
            width = 0;
            height = 0;

            // Setting this property to true prevents flickering
            this.DoubleBuffered = true;
        }

        // Width of the world in cells (not pixels)
        public int width
        {
            get;
            set;
        }

        // Height of the world in cells (not pixels)
        public int height
        {
            get;
            set;
        }

        // Example of world method might be...

        public void AddFood(Food food)
        {
            lock (foodLock)
            {
                foods[food.ID] = food;
            }
        }

        public void AddSnake(Snake snake)
        {
            lock (snakeLock)
            {
                snakes[snake.ID] = snake;
            }

        }

        public void RemoveSnake(int id)
        {
            lock (snakeLock)
            {
                if (snakes.ContainsKey(id))
                {
                    snakes.Remove(id);
                }

            }
        }

        public void RemoveFood(int id)
        {
            lock (foodLock)
            {
                if (foods.ContainsKey(id))
                {
                    foods.Remove(id);
                }
            }
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

                // Draw Food
                lock (foodLock)
                {
                    foreach (KeyValuePair<int, Food> food in foods)
                    {
                        Rectangle circle = new Rectangle(food.Value.loc.x * pixelsPerCell, food.Value.loc.y * pixelsPerCell, pixelsPerCell, pixelsPerCell);
                        e.Graphics.FillEllipse(drawBrush, circle);
                    }

                }

                // Draw Snake
                lock (snakeLock)
                {

                    foreach (KeyValuePair<int, Snake> snake in snakes)
                    {

                     //  if (!snakeColor.ContainsKey(snake.Key))
                       // {
                       //     Random random = new Random();
                        //    Color color = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
                        //    snakeColor[snake.Key] = color;
                        //}

                        
                        
                        using (SolidBrush drawBrusher = new SolidBrush(snake.Value.color))
                        {
                            Point lastPoint = null;
                            foreach (Point p in snake.Value.vertices)
                            {


                                if (ReferenceEquals(lastPoint, null)) // if first vertice
                                {
                                    lastPoint = p;
                                    continue;
                                }
                                if (lastPoint == p)
                                {
                                    break;
                                }
                                if ((lastPoint.x - p.x) == 0) // Going up or down
                                {
                                    if (lastPoint.y - p.y > 0) // going up
                                    {
                                        Rectangle segment = new Rectangle(p.x * pixelsPerCell, p.y * pixelsPerCell, pixelsPerCell, pixelsPerCell * (lastPoint.y - p.y) + pixelsPerCell);
                                        e.Graphics.FillRectangle(drawBrusher, segment);
                                    }
                                    else // Going down

                                    {
                                        Rectangle segment = new Rectangle(lastPoint.x * pixelsPerCell, lastPoint.y * pixelsPerCell, pixelsPerCell, pixelsPerCell * (p.y - lastPoint.y) + pixelsPerCell);
                                        e.Graphics.FillRectangle(drawBrusher, segment);
                                    }
                                }
                                else if ((lastPoint.y - p.y) == 0) // Going sideways
                                {
                                    if (lastPoint.x - p.x > 0) // Going Left
                                    {
                                        Rectangle segment = new Rectangle(p.x * pixelsPerCell, p.y * pixelsPerCell, pixelsPerCell * (lastPoint.x - p.x) + pixelsPerCell, pixelsPerCell);
                                        e.Graphics.FillRectangle(drawBrusher, segment);
                                    }
                                    else // Going right
                                    {
                                        Rectangle segment = new Rectangle(lastPoint.x * pixelsPerCell, lastPoint.y * pixelsPerCell, pixelsPerCell * (p.x - lastPoint.x) + pixelsPerCell, pixelsPerCell);
                                        e.Graphics.FillRectangle(drawBrusher, segment);
                                    }
                                }

                                lastPoint = p;
                            }
                        }


                    }
                }
            }
        }
    }
}
