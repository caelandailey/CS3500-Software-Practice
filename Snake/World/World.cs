/// Caelan Dailey 
/// Karina Biancone
/// 11/22/2016
/// Snake Game
/// CS 3500 

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

    /// <summary>
    /// Class that represents our world/panel. It handles the data from the panel and draws it. Handles data of the snake and food. Handles the location of everything on the panel
    /// </summary>
    public class World : Panel
    {

        // Determines the size in pixels of each grid cell in the world
        public const int pixelsPerCell = 5;

        public Dictionary<int, Food> foods; // holds all the food in the world
        public Dictionary<int, Snake> snakes; // holds all the snakes in the world

        private Dictionary<Point, int> foodLoc;

        private Object foodLock; // Lock for food

        private Object snakeLock; // Lock for snakes

        private int foodCount = 0;

        private Dictionary<Point, int> verticeDirection;

        public void createFood(int foodDensity)
        {
            while (foodCount < snakes.Keys.Count * foodDensity)
            {

                Random rnd = new Random();

                int x = rnd.Next(1, width - 1);
                int y = rnd.Next(1, height - 1);
                Point cords = new Point(rnd.Next(1, height - 1), rnd.Next(1, width - 1));
                // check if food already exists there
                if (foodLoc.ContainsKey(cords))
                {
                    continue;
                }

                Food food = new Food();
                food.loc = new Point(x, y);

                AddFood(food);
                foodLoc[cords] = 1; // 1 means theres food
            }

        }
        //if the space is open
        // no snakes or food there



        /// <summary>
        /// Constructor. Create empty world
        /// </summary>
        public World()
        {
            foods = new Dictionary<int, Food>();
            snakes = new Dictionary<int, Snake>();
            snakeLock = new Object();
            foodLock = new Object();
            verticeDirection = new Dictionary<Point, int>();

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

        /// <summary>
        /// Method to add food. When client recieves food data at it to the database
        /// </summary>
        /// <param name="food"></param>
        public void AddFood(Food food)
        {
            lock (foodLock) // Changing food, lock it
            {
                foods[food.ID] = food;
            }
        }

        /// <summary>
        /// Method to add snakes. When client recieves snake data add it to the database
        /// </summary>
        /// <param name="snake"></param>
        public void AddSnake(Snake snake)
        {
            lock (snakeLock) // Changing snake, lock it. Dont want to make changes while already changing it
            {
                snakes[snake.ID] = snake;
            }

        }

        /// <summary>
        /// Remove the snake if it deads
        /// </summary>
        /// <param name="id"></param>
        public void RemoveSnake(int id)
        {
            lock (snakeLock)
            {
                if (snakes.ContainsKey(id)) // Check if already a snake
                {
                    snakes.Remove(id);
                }

            }
        }

        /// <summary>
        /// Remove food if eaten
        /// </summary>
        /// <param name="id"></param>
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

        public void createVertice(Point p, int direction)
        {
            verticeDirection[p] = direction;
        }

        public bool detectCollision()
        {
            return false;
        }

        public void MoveSnake(Snake snake, int direction)
        {
            //remove tail
            //add to head in direction of choice
            // if not same direction add vertice
            // if same direction increase last point?



            //snake.vertices.RemoveAt(0);


            //***********************HEAD*************************

            Point head = new Point(snake.vertices[snake.vertices.Count - 1].x, snake.vertices[snake.vertices.Count - 1].y);
            
            int oldHeadX = head.x;
            int oldHeadY = head.y;
            Point oldHead = new Point(oldHeadX, oldHeadY);
            // Track old head

            switch (direction)      // Get point of new head       
            {
                case 1:
                    head.y = head.y - 1;
                    break;
                case 2:
                    head.x = head.x + 1;
                    break;
                case 3:
                    head.y = head.y + 1;
                    break;
                case 4:
                    head.x = head.x - 1;
                    break;

            }
            Point newHead = new Point(head.x, head.y);
            verticeDirection[newHead] = direction; // Add head to snakes direction

            // set new head to 
            
            
            if (verticeDirection[oldHead]  == verticeDirection[newHead])
            {
                verticeDirection.Remove(oldHead); // If same direction remove old head
            }
            else // if not same direction
            {

                verticeDirection[oldHead] = direction; // If not set new direction to that vertice

            }

            snake.vertices.Add(head);

            //*********************************TAIL**************************
            //calculate what the new tail vertice is
            newTail(snake);

        }

        // <summary>
        // Update new tail vertice
        // </summary>
        // <param name = "snake" ></ param >
        public void newTail(Snake snake)
        {
            //current tail
            Point tail = snake.vertices[0];            // get tail
            int oldTailX = tail.x;
            int oldTailY = tail.y;
            Point oldTail = new Point(oldTailX, oldTailY); // save old tail since we're updating with new tail

            //the direction the tail is going
            int direction = verticeDirection[tail];

            //update new point for the tail
            switch (direction)
            {
                case 1:
                    tail.y = tail.y - 1;
                    break;
                case 2:
                    tail.x = tail.x + 1;
                    break;
                case 3:
                    tail.y = tail.y + 1;
                    break;
                case 4:
                    tail.x = tail.x - 1;
                    break;
            }


            //update dictionary and snake
            if (verticeDirection.ContainsKey(tail)) // snake turned
            {

                snake.vertices.Remove(oldTail); // remove old tail since snake turned at next vertice
                verticeDirection.Remove(oldTail);

            }

            //the tail needs to become a new vertice
            else // new tail is in same direction as old tail
            {

                verticeDirection.Remove(oldTail);
                verticeDirection[tail] = direction;

                //update snake
                snake.vertices.Remove(oldTail);
                snake.vertices.Insert(0, tail); //
            }
        }


        /// <summary>
        /// Override the behavior when the panel is redrawn
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
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
                lock (snakeLock) // Lock it dont want to change if alreadying changing
                {
                    foreach (KeyValuePair<int, Snake> snake in snakes) // Loop through snakes
                    {
                        using (SolidBrush drawBrusher = new SolidBrush(snake.Value.color)) // set color
                        {
                            Point lastPoint = null;
                            foreach (Point p in snake.Value.vertices) // Loop through points and draw it
                            {


                                if (ReferenceEquals(lastPoint, null)) // if first vertice
                                {
                                    lastPoint = p; // Get vertice and then continue to obtain 2 points.
                                    continue;
                                }
                                if (lastPoint == p) // if last point dont draw.
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

                                lastPoint = p; // Set last point
                            }
                        }
                    }
                }
            }
        }
    }
}
