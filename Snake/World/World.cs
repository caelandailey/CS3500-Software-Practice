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
        public Dictionary<Point, Food> foodPoint;
        //private Dictionary<Point, int> foodLoc;

        private Object foodLock; // Lock for food

        private Object snakeLock; // Lock for snakes

        private int foodCount;

        private Dictionary<Point, int> verticeDirection;

        private int snakeCount;
        private int foodCreated = 0;
        private int?[,] worldGrid;

        public int createSnake(string name, int ID)
        {
            // Look for open spot
            Random rnd = new Random();
            bool foundOpenSpot = false;
            Point head = null;
            Point tail = null;
            int randomDirection = 0;

            while (foundOpenSpot == false)
            {
                //create random x,y coordinates for tail

                int x = rnd.Next(30, width - 30);
                int y = rnd.Next(30, height - 30);
                head = new Point(x, y);
                tail = new Point(x, y);

                // Loop through snakes positions
                randomDirection = rnd.Next(1, 4);


                for (int i = 0; i<=15; i ++)
                {
                    Point point = new Point(x,y);

                    // Get random direction

                    switch (randomDirection)
                    {
                        case 1:
                            point = new Point(x, y-i);
                            break;
                        case 2:
                            point = new Point(x+i, y );
                            break;
                        case 3:
                            point = new Point(x, y -i);
                            break;
                        case 4:
                            point = new Point(x-i,y);
                            break;
                    }
                    
                    if (!EmptyPoint(point))
                    {
                        continue;
                    }
                    if (i == 15) // last point
                    {
                        tail = point;
                        foundOpenSpot = true;
                    }
                }  
            }
            
            // Create snake object and set values

            Snake snake = new Snake(); // Make snake object
            snake.name = name; // Set name
            snake.ID = ID; // Set id
            List<Point> snakeVertices = new List<Point>(); // Create list to hold snake head and tail
            snakeVertices.Add(head); // Add head THESE ARE SWITCHED BECAUSE THE TAIL IS IN THE FRONT OF THE SNAKE
            snakeVertices.Add(tail); // Add tail
            snake.vertices = snakeVertices; // Add head and tail to the snake object
            AddSnake(snake); // Add snake

            // Add snake to world grid

            for (int i = 0; i <= 15; i++)
            {
                switch (randomDirection)
                {
                    case 1:
                        worldGrid[head.x, head.y-i] = randomDirection;
                    break;

                    case 2:
                        
                        worldGrid[head.x + i, head.y] = randomDirection;
                        break;

                    case 3:
                        worldGrid[head.x, head.y-i] = randomDirection;
                        break;

                    case 4:
                        worldGrid[head.x-i, head.y] = randomDirection;
                        break;
                }
            }

            return randomDirection;
            //snakeDirection[snake.ID] = direction; // Set direction to snake ID
            //AddSnake(snake); // Add snake
            //snakeCount++;
            //update dictionary of vertices
            //world.createVertice(tail, direction);
            //world.createVertice(head, direction);
        }

        public void createFood(int foodDensity)
        {

            // While not enough food, make food
            Random rnd = new Random();
            while (foodCount < snakes.Keys.Count * foodDensity) 
            {
                // Generate random cord

                
                int x = rnd.Next(1, width - 2);
                int y = rnd.Next(1, height - 2);
                Point point = new Point(x, y);

                // Check if food is at that location

                if (EmptyPoint(point) == false)
                {
                    // Return to while loop if location is taken

                    continue;
                }

                MakeFood(point);

                
            }

        }
        
        private bool EmptyPoint(Point point)
        {
            if (worldGrid[point.x, point.y].HasValue)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Constructor. Create empty world
        /// </summary>
        public World()
        {
            foods = new Dictionary<int, Food>();
            foodPoint = new Dictionary<Point, Food>();
            snakes = new Dictionary<int, Snake>();
            snakeLock = new Object();
            foodLock = new Object();
            verticeDirection = new Dictionary<Point, int>();
            worldGrid = new int?[,] { };
            width = 0;
            height = 0;
            foodCount = 0;
            // Setting this property to true prevents flickering
            this.DoubleBuffered = true;
        }

        /// <summary>
        /// Constructor. Create empty world
        /// </summary>
        public World(int _width, int _height)
        {
            foods = new Dictionary<int, Food>();
            foodPoint = new Dictionary<Point, Food>();
            snakes = new Dictionary<int, Snake>();
            snakeLock = new Object();
            foodLock = new Object();
            verticeDirection = new Dictionary<Point, int>();
            worldGrid = new int?[_width,_height];
            width = _width;
            height = _height;
            foodCount = 0;
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

            if (snake.vertices.Count == 0)
            {
                return;
            }
            //***********************HEAD*************************

            int headX = snake.vertices[snake.vertices.Count - 1].x;
            int headY = snake.vertices[snake.vertices.Count - 1].y;
            
            int oldHeadX = headX;
            int oldHeadY = headY;
            
            // Track old head

            switch (direction)      // Get point of new head       
            {
                case 1:
                    headY = headY - 1;
                    break;
                case 2:
                    headX = headX + 1;
                    break;
                case 3:
                    headY = headY + 1;
                    break;
                case 4:
                    headX = headX - 1;
                    break;

            }
            if (headX == 149 || headY == 149 || headY == 0 || headX == 0) // Wall
            {
                // remove snek
                KillSnake(snake);
                Snake newSnake = snake;
                
                List<Point> verticeList = new List<Point>();
                verticeList.Add(new Point(-1, -1));
                newSnake.vertices = verticeList;
                AddSnake(newSnake);
                return;

            }
            //check that updated coordinate is in an empty grid space
            if (worldGrid[headX, headY].HasValue)
            {
                //collision
                if (worldGrid[headX, headY] == 0) // there's food
                {
                    worldGrid[headX, headY] = direction;
                    
                    Point newHead = new Point(headX, headY);
                    //check if direction of new head and old head is the same
                    if (worldGrid[oldHeadX, oldHeadY] == worldGrid[headX, headY])
                    {
                        snake.vertices.RemoveAt(snake.vertices.Count - 1); // Removes last position
                    }
                    worldGrid[oldHeadX, oldHeadY] = direction;
                    snake.vertices.Add(newHead);
                    lock (foodLock)
                    {
                        if (foodPoint.ContainsKey(newHead))
                        {

                            Food deadFood = foodPoint[newHead];
                            deadFood.loc = new Point(-1, -1);
                            foodPoint[deadFood.loc] = deadFood;
                            foodPoint.Remove(newHead);
                            foodCount--;
                        }
                    }
                    
                }
                
            }
            else
            {
                worldGrid[headX, headY] = direction;
                Point newHead = new Point(headX, headY);
                //check if direction of new head and old head is the same
                if(worldGrid[oldHeadX, oldHeadY] == worldGrid[headX, headY])
                {
                    snake.vertices.RemoveAt(snake.vertices.Count - 1); // Removes last position
                }
                worldGrid[oldHeadX, oldHeadY] = direction;
                snake.vertices.Add(newHead);
                
                //calculate what the new tail vertice is
                NewTail(snake);
            }

            snakes[snake.ID] = snake;
        }

        // <summary>
        // Update new tail vertice
        // </summary>
        // <param name = "snake" ></ param >
        public void NewTail(Snake snake)
        {
            //current tail
            int tailX = snake.vertices[0].x;            // get tail
            int tailY = snake.vertices[0].y;

            int oldTailX = tailX;
            int oldTailY = tailY;

            //the direction the tail is going
            int direction = worldGrid[tailX, tailY].Value;

            //update new point for the tail
            switch (direction)
            {
                case 1:
                    tailY = tailY - 1;
                    break;
                case 2:
                    tailX = tailX + 1;
                    break;
                case 3:
                    tailY = tailY + 1;
                    break;
                case 4:
                    tailX = tailX - 1;
                    break;
            }

            snake.vertices.RemoveAt(0); // remove old tail since snake turned at next vertice

            //update dictionary and snake
            if (worldGrid[oldTailX, oldTailY] == worldGrid[tailX, tailY]) // snake turned
            {
               
                Point newTail = new Point(tailX, tailY);
                snake.vertices.Insert(0, newTail);
            }
           
            snakes[snake.ID] = snake;
            worldGrid[oldTailX, oldTailY] = null;
            
        }

        private void KillSnake(Snake snake)
        {            
            for(int i = 0; i < snake.vertices.Count-1; i ++)
            {
                Point vertice = snake.vertices[i]; 
                Point nextVertice = snake.vertices[i+1];
                if(vertice.x == nextVertice.x)
                {
                    Random rnd = new Random();                    
                    int x = vertice.x;
                    if (vertice.y < nextVertice.y)
                    {
                        for(int k = vertice.y; k <= nextVertice.y; k++)
                        {
                            int random = rnd.Next(0, nextVertice.y - vertice.y);
                            if (random == 1)
                            {
                                Point point = new Point(x, k);
                                MakeFood(point);
                            }
                            else
                            {
                                worldGrid[x, k] = null;
                            }
                        }                        
                    }
                    else
                    {
                        for(int k = nextVertice.y; k <= vertice.y; k++)
                        {
                            int random = rnd.Next(0, vertice.y - nextVertice.y);
                            if(random == 1)
                            {
                                Point point = new Point(x, k);
                                MakeFood(point);
                            }
                            else
                            {
                                worldGrid[x, k] = null;
                            }
                        }
                    }
                }
                else
                {
                    Random rnd = new Random();
                    int y = vertice.y;
                    if (vertice.x < nextVertice.x)
                    {
                        for (int k = vertice.x; k <= nextVertice.x; k++)
                        {
                            int random = rnd.Next(0, nextVertice.x - vertice.x);
                            if (random == 1)
                            {
                                Point point = new Point(k,y);
                                MakeFood(point);
                            }
                            else
                            {
                                worldGrid[k,y] = null;
                            }
                        }
                    }
                    else
                    {
                        for (int k = nextVertice.x; k <= vertice.x; k++)
                        {
                            int random = rnd.Next(0, vertice.x - nextVertice.y);
                            if (random == 1)
                            {
                                Point point = new Point(k,y);
                                MakeFood(point);
                            }
                            else
                            {
                                worldGrid[k,y] = null;
                            }
                        }
                    }
                }
                
            }
            
        }

        private void MakeFood(Point point)
        {
            // Create food object

            Food food = new Food();

            // Set food location to x and y that were randomly generated
            Point foodLoc = point;
            food.loc = foodLoc;
            food.ID = foodCreated;
            foodCreated++;

            // Add food to list of foods

            lock (foodLock) // Changing food, lock it
            {
                foodPoint[foodLoc] = food;
            }
            foodCount++;

            // Add food to world grid to track

            worldGrid[food.loc.x, food.loc.y] = 0;
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
