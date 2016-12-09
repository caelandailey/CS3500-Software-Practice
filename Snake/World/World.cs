/// Caelan Dailey 
/// Karina Biancone
/// 12/8/2016
/// Snake Game
/// CS 3500 

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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

        // Store information of world

        public Dictionary<int, Food> foods; // holds all the food in the world
        public Dictionary<int, Snake> snakes; // holds all the snakes in the world
        public Dictionary<Point, Food> foodPoint; // Holds the points of all food in the world. Same as foods but has points as the key
        private int?[,] worldGrid;

        // Locks for dictionary

        private Object foodLock = new object(); // Lock for food
        private Object snakeLock = new object(); // Lock for snakes
        private object gridLock = new object(); // Lock for the world Grid
        private object foodPointLock = new object(); // Lock for the food points

        // Track food

        private int foodCount; // Track amount of food on board. Need this to track food in order to not make too much food.
        private int foodCreated; // Track amount of food ever created. FoodCreated = food ID

        // Settings from the XML
        private int startingSnakeLength;
        private double recycleRate;
        private int gameMode;

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
            worldGrid = new int?[,] { };
            width = 0;
            height = 0;
            foodCount = 0;
            // Setting this property to true prevents flickering
            this.DoubleBuffered = true;
        }

        /// <summary>
        /// Constructor. Create empty world used by the SERVER. Takes in world settings!
        /// </summary>
        public World(int _width, int _height, double _recycleRate, int _startingSnakeLength, int _gameMode)
        {
            foods = new Dictionary<int, Food>();
            foodPoint = new Dictionary<Point, Food>();
            snakes = new Dictionary<int, Snake>();
            worldGrid = new int?[_width, _height];
            width = _width;
            height = _height;
            foodCount = 0;
            foodCreated = 0;
            startingSnakeLength = _startingSnakeLength;
            gameMode = _gameMode;
            recycleRate = _recycleRate;
            // Setting this property to true prevents flickering
            this.DoubleBuffered = true;
        }


        //Removes a food from foodPoint. 
        public void removeFoodPoint(Point point)
        {
            lock (foodPointLock)
            {
                if (foodPoint.ContainsKey(point)) // if it contains it
                {
                    foodPoint.Remove(point); // Then remove it
                }
            }
        }

        /// <summary>
        /// Creates a snake when the client sends a player name. Takes in a snake name and a client ID. 
        /// Snake is randomly generated at a random location
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int createSnake(string name, int ID)
        {

            // GENERATE VARIABLES

            // Look for open spot
            Random rnd = new Random();
            bool foundOpenSpot = false;
            Point head = null;
            Point tail = null;
            int randomDirection = 0;

            // CHECK FOR OPEN SLOT

            while (foundOpenSpot == false)
            {
                //create random x,y coordinates for tail

                int x = rnd.Next(startingSnakeLength * 2, width - startingSnakeLength * 2);
                int y = rnd.Next(startingSnakeLength * 2, height - startingSnakeLength * 2);
                head = new Point(x, y);
                tail = new Point(x, y);

                // Loop through snakes positions
                randomDirection = rnd.Next(1, 5);

                // Loop Through each position
                for (int i = 0; i <= startingSnakeLength; i++)
                {
                    // Get random direction

                    int newX = x;
                    int newY = y;
                    switch (randomDirection)
                    {
                        case 1:
                            newY = newY - i;
                            break;
                        case 2:
                            newX = newX + i;
                            break;
                        case 3:
                            newY = newY + i;
                            break;
                        case 4:
                            newX = newX - i;
                            break;
                    }
                    Point point = new Point(newX, newY);
                    if (!EmptyPoint(point)) // If empty start over
                    {
                        continue;
                    }
                    if (i == startingSnakeLength) // if is point
                    {
                        tail = point;
                        foundOpenSpot = true;
                    }
                }
            }

            // CREATE SNAKE AND ADD TO OPEN SPOT

            // Create snake object and set values

            Snake snake = new Snake(); // Make snake object
            snake.name = name; // Set name
            snake.ID = ID; // Set id
            snake.length = startingSnakeLength;
            List<Point> snakeVertices = new List<Point>(); // Create list to hold snake head and tail
            snakeVertices.Add(head); // Add head THESE ARE SWITCHED BECAUSE THE TAIL IS IN THE FRONT OF THE SNAKE
            snakeVertices.Add(tail); // Add tail

            snake.vertices = snakeVertices; // Add head and tail to the snake object
            AddSnake(snake); // Add snake

            // ADD SNAKE POINTS TO WORLD GRID

            // Add snake to world grid

            for (int i = 0; i <= startingSnakeLength; i++)
            {
                lock (gridLock)
                {
                    switch (randomDirection)
                    {
                        case 1:
                            worldGrid[head.x, head.y - i] = randomDirection;
                            break;

                        case 2:

                            worldGrid[head.x + i, head.y] = randomDirection;
                            break;

                        case 3:
                            worldGrid[head.x, head.y + i] = randomDirection;
                            break;

                        case 4:
                            worldGrid[head.x - i, head.y] = randomDirection;
                            break;
                    }
                }
            }

            return randomDirection; // Returns random direction in order to store in server
        }

        /// <summary>
        /// Creates food. Takes in foodDensity and creates food based on ratio. if food count is less then # of snakes * density then make food. 
        /// </summary>
        /// <param name="foodDensity"></param>
        public void createFood(int foodDensity)
        {
            if (gameMode == 1) // Game made does not make food!!!
            {
                return;
            }
            Random rnd = new Random();
            while (foodCount < snakes.Keys.Count * foodDensity)            // While not enough food, make food
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
                MakeFood(point); // If empty make food
            }
        }

        /// <summary>
        /// Takes in a point and checs if the world grid has an open spot there
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private bool EmptyPoint(Point point)
        {
            if (worldGrid[point.x, point.y].HasValue)
            {
                return false;
            }
            return true;
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

        /// <summary>
        /// Moves the snake by adding to the head in direction of choice and removing the tail
        /// Takes in a snake object and direction. Checks for collisions
        /// </summary>
        /// <param name="snake"></param>
        /// <param name="direction"></param>
        public void MoveSnake(Snake snake, int direction)
        {
            int headX = snake.vertices[snake.vertices.Count - 1].x; // Track head 
            int headY = snake.vertices[snake.vertices.Count - 1].y;

            int oldHeadX = headX; // Track old head
            int oldHeadY = headY;

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

            // Get new position and then check for collisions

            if (headX == width - 1 || headY == width - 1 || headY == 0 || headX == 0) // If it's a wall
            {
                KillSnake(snake); // Kill snek and return
                return;
            }

            if (worldGrid[headX, headY].HasValue) // Check if position has a value
            {
                if (worldGrid[headX, headY] == 0) // If it is equal to '0' then it is FOOD
                {
                    lock (gridLock) // Update direction 
                    {
                        worldGrid[headX, headY] = direction; // Update grid
                    }
                    snake.length = snake.length++; // Increase snake length

                    Point newHead = new Point(headX, headY); // Create new head

                    if (worldGrid[oldHeadX, oldHeadY] == worldGrid[headX, headY]) // Check if direction of new head and old head is the same
                    {
                        snake.vertices.RemoveAt(snake.vertices.Count - 1); // If it's the same then remove the last head then add new head
                    }

                    // NOTE* IF same direction then remove last vertice and add new one. Don't want 2 vertices when same direction
                    // If not then it is TURNING. Add new vertices don't remove last one.

                    snake.vertices.Add(newHead); // Add head
                    lock (foodPointLock) // Lock food
                    {
                        if (foodPoint.ContainsKey(newHead)) // If there is food. There 100% should be food. Unless bug
                        {
                            Food deadFood = foodPoint[newHead]; // Create dead food
                            deadFood.loc = new Point(-1, -1); // Client reads a (-1, -1) to know its a dead food
                            foodPoint[deadFood.loc] = deadFood; // Set food to a dead food
                            //foodPoint.Remove(newHead);
                            foodCount--;
                        }
                    }
                    AddSnake(snake);
                }
                else // If value not == to 0 then there's a snake in that location. Kill the snake
                {
                    KillSnake(snake); // Remove snek
                    return;
                }
            }
            else // SPACE IS EMPTY MOVE SNAKE HEAD
            {
                lock (gridLock)
                {
                    worldGrid[headX, headY] = direction;
                }
                Point newHead = new Point(headX, headY);

                //check if direction of new head and old head is the same
                if (worldGrid[oldHeadX, oldHeadY] == worldGrid[headX, headY])
                {
                    snake.vertices.RemoveAt(snake.vertices.Count - 1); // Removes last position
                }
                lock (gridLock)
                {
                    worldGrid[oldHeadX, oldHeadY] = direction;
                }
                snake.vertices.Add(newHead); // Add new head

                AddSnake(snake);

                NewTail(snake);

            }

            //snakes[snake.ID] = snake;


        }

        // <summary>
        // Update new tail vertice
        // </summary>
        // <param name = "snake" ></ param >
        public void NewTail(Snake snake)
        {
            if (gameMode == 1) // Game mode does not delete the tail!!
            {
                return;
            }

            //current tail
            int tailX = snake.vertices[0].x;            // get tail
            int tailY = snake.vertices[0].y;

            int oldTailX = tailX;
            int oldTailY = tailY;

            int direction = 0;
            lock (gridLock)
            {
                direction = (int)worldGrid[tailX, tailY];
            }
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

            if (worldGrid[oldTailX, oldTailY] == worldGrid[tailX, tailY]) // snake turned
            {
                Point newTail = new Point(tailX, tailY);
                snake.vertices.Insert(0, newTail);
            }
            AddSnake(snake);
            lock (gridLock)
            {
                worldGrid[oldTailX, oldTailY] = null;
            }
        }


        /// <summary>
        /// If the snake hits another snake then kill it. If it collides with a wall or itself it dies.
        /// When a snake dies add random food to where it died at. 
        /// </summary>
        /// <param name="snake"></param>
        private void KillSnake(Snake snake)
        {
            lock (gridLock)
            {
                if (gameMode == 1) // If game mode. Kill snakes thats it. No food. 
                {
                    Snake deadSnake = snake;

                    List<Point> vertices = new List<Point>();
                    vertices.Add(new Point(-1, -1));
                    vertices.Add(new Point(-1, -1));
                    deadSnake.vertices = vertices;
                    AddSnake(deadSnake);
                    return;
                }
                //determines wether or not to keep converting the snake to food
                bool isLooping = true;

                //coordinates of the tail of the snake
                int x = snake.vertices.First().x;
                int y = snake.vertices.First().y;
                //int snakeHeadX = snake.vertices.Last().x;
                //int snakeHeadY = snake.vertices.Last().y;

                Point nextPoint = new Point(x, y);
                //create a random object
                Random random = new Random();

                while (isLooping == true)
                {

                    int direction = 0;

                    direction = (int)worldGrid[x, y];

                    if (random.NextDouble() < recycleRate) // Randomly creates food based on recycleRate.
                    {
                        //add food to the world
                        MakeFood(new Point(x, y));

                    }
                    else
                    {
                        //make that point empty in the world
                        lock (gridLock)
                        {
                            worldGrid[x, y] = null;
                        }
                    }

                    //go to the next point on the snake by using the last direction read
                    switch (direction)
                    {
                        //up
                        case 1:
                            nextPoint.y = nextPoint.y - 1;
                            break;
                        //right
                        case 2:
                            nextPoint.x = nextPoint.x + 1;
                            break;
                        //down
                        case 3:
                            nextPoint.y = nextPoint.y + 1;
                            break;
                        //left
                        case 4:
                            nextPoint.x = nextPoint.x - 1;
                            break;
                    }

                    //check if the grid space is another snake 
                    if (worldGrid[nextPoint.x, nextPoint.y] > 0)
                    {
                        //check the head
                        if (snake.vertices.Last().x != nextPoint.x || snake.vertices.Last().y != nextPoint.y)
                        {
                            x = nextPoint.x;
                            y = nextPoint.y;
                        }
                        else // if it is a head then stop looping
                        {
                            isLooping = false;
                            if (random.NextDouble() < recycleRate)
                            {
                                //add food to the world
                                MakeFood(new Point(nextPoint.x, nextPoint.y));

                            }
                            else
                            {
                                //make that point empty in the world

                                worldGrid[nextPoint.x, nextPoint.y] = null;

                            }
                        }
                    }
                    else
                    {
                        isLooping = false;
                    }
                }
            }

            Snake newSnake = snake; // Copy snake

            List<Point> verticeList = new List<Point>(); // Create list for new vertices
            verticeList.Add(new Point(-1, -1)); // Add dead snake. Dead snakes are represented by two (-1, -1) points.
            verticeList.Add(new Point(-1, -1));
            newSnake.vertices = verticeList; // Set vertices
            AddSnake(newSnake); // add snake. Should just overwrite old one. 
        }

        /// <summary>
        /// Makes food. Takes in a point and adds food at that position
        /// </summary>
        /// <param name="point"></param>
        private void MakeFood(Point point)
        {
            if (gameMode == 1) // game mode does not make food!!!
            {
                return;
            }
            int x = point.x;
            int y = point.y;
            // Create food object

            Food food = new Food();

            // Set food location to x and y that were randomly generated
            //Point foodLoc = point;
            food.loc = new Point(x, y);
            food.ID = foodCreated;
            foodCreated++;

            // Add food to list of foods

            lock (gridLock)
            {
                // Add food to world grid to track

                worldGrid[x, y] = 0;
            }

            lock (foodPointLock)
            {
                foodPoint[new Point(x, y)] = food;
                foodCount++;
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
