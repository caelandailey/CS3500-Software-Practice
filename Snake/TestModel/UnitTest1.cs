using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SnakeGame;
using System.Collections.Generic;

namespace TestModel
{
    [TestClass]
    public class UnitTest1
    {
        int width = 150;
        int height = 150;
        int foodDensity = 2;
        double RecycleRate = 1;
        int StartingLength = 15;
        int gameMode = 0;

        [TestMethod]
        public void testNewSnakeDirection()
        {
            World world = new World(width, height, RecycleRate, StartingLength, gameMode);

            int direction = world.createSnake("bob", 1);

            Assert.IsTrue(direction.Equals(1) || direction.Equals(2) || direction.Equals(3) || direction.Equals(4));
        }

        [TestMethod]
        public void testNewSnakePositionTail()
        {
            World world = new World(width, height, RecycleRate, StartingLength, gameMode);

            int direction = world.createSnake("bob", 1);

            int x = world.snakes[1].vertices[1].x;
            int y = world.snakes[1].vertices[1].y;

            Assert.IsTrue(x != width - 15 && y != width - 15 && y != 15 && x != 15);

        }

        [TestMethod]
        public void testNewSnakePositionHead()
        {
            World world = new World(width, height, RecycleRate, StartingLength, gameMode);

            int direction = world.createSnake("bob", 1);

            int x = world.snakes[1].vertices[0].x;
            int y = world.snakes[1].vertices[0].y;

            Assert.IsTrue(x != width - 15 && y != width - 15 && y != 15 && x != 15);

        }

        [TestMethod]
        public void testNewSnakeCount()
        {
            World world = new World(width, height, RecycleRate, StartingLength, gameMode);

            int direction = world.createSnake("bob", 1);

            Assert.AreEqual(world.snakes.Keys.Count, 1);
        }

        [TestMethod]
        public void testNewSnakeCountMore()
        {
            World world = new World(width, height, RecycleRate, StartingLength, gameMode);
            for (int i = 1; i < 100; i++)
            {
                int direction = world.createSnake(i.ToString(), i);
            }


            Assert.AreEqual(world.snakes.Count, 99);
        }

        [TestMethod]
        public void testNewSnakePoints()
        {
            World world = new World(width, height, RecycleRate, StartingLength, gameMode);

            int direction = world.createSnake("n", 0);

            Assert.AreEqual(world.snakes[0].vertices.Count, 2);

        }

        [TestMethod]
        public void testNewSnakeLength()
        {
            World world = new World(width, height, RecycleRate, StartingLength, gameMode);

            int direction = world.createSnake("n", 0);

            Assert.AreEqual(world.snakes[0].length, 15); //assume starting snake length is 15
        }


        [TestMethod]
        public void addSnake()
        {
            World world = new World(width, height, RecycleRate, StartingLength, gameMode);

            Snake snake = new Snake();

            world.AddSnake(snake);

            Assert.IsTrue(world.snakes.Count == 1);
        }

        [TestMethod]
        public void createFood1()
        {
            World world = new World(width, height, RecycleRate, StartingLength, gameMode);

            int direction = world.createSnake("n", 0);

            world.createFood(foodDensity);

            Assert.AreEqual(world.foodPoint.Keys.Count, 2);
        }

        [TestMethod]
        public void createFoodMore()
        {
            World world = new World(width, height, RecycleRate, StartingLength, gameMode);

            for (int i = 1; i < 100; i++)
            {
                int direction = world.createSnake(i.ToString(), i);

                world.createFood(foodDensity);
            }

            Assert.AreEqual(world.foodPoint.Keys.Count, 198);
        }

       [TestMethod]
       public void killSnake()
        {
            World world = new World(width, height, RecycleRate, StartingLength, gameMode);

            Snake snake = new Snake();
            snake.ID = 2;

            world.AddSnake(snake);

            Assert.AreEqual(world.snakes.Keys.Count, 1);

            world.RemoveSnake(snake.ID);

            Assert.AreEqual(world.snakes.Keys.Count, 0);

        }

        [TestMethod]
        public void killNewSnake()
        {
            World world = new World(width, height, RecycleRate, StartingLength, gameMode);

            world.createSnake("y", 2);

            Assert.AreEqual(world.snakes.Keys.Count, 1);

            world.RemoveSnake(2);

            Assert.AreEqual(world.snakes.Keys.Count, 0);
        }

        [TestMethod]
        public void moveHeadAndTail()
        {
            World world = new World(width, height, RecycleRate, StartingLength, gameMode);

            world.createSnake("y", 0);
            Snake snake = new Snake();
            snake = world.snakes[0];            

            //get tail point
            int x = world.snakes[0].vertices[0].x;
            int y = world.snakes[0].vertices[0].y;

            //get head
            int headX = world.snakes[0].vertices[1].x;
            int headY = world.snakes[0].vertices[1].y;

            world.MoveSnake(snake, 2);

            Assert.AreEqual(x, world.snakes[0].vertices[0].x-1);
            Assert.AreEqual(headX, world.snakes[0].vertices[1].x-1);
        }

        [TestMethod]
        public void moveLength()
        {
            World world = new World(width, height, RecycleRate, StartingLength, gameMode);

            world.createSnake("y", 0);
            Snake snake = new Snake();
            snake = world.snakes[0];

            world.MoveSnake(snake, 2);

            Assert.AreEqual(snake.length, StartingLength);
        }

        [TestMethod]
        public void growSnake1()
        {
            World world = new World(width, height, RecycleRate, StartingLength, gameMode);
                      
            Snake snake = new Snake();
            snake.ID = 0;
            Point head = new Point(1, 1);
            Point tail = new Point(1, 16);
            List<Point> vertices = new List<Point>();
            vertices.Add(tail);
            vertices.Add(head);
            snake.vertices = vertices;
            
            world.AddSnake(snake);

            world.MoveSnake(snake, 1);
            
            Assert.AreEqual(world.snakes.Keys.Count, 1);
            
        }

        [TestMethod]
        public void growSnake2()
        {
            World world = new World(width, height, RecycleRate, StartingLength, gameMode);

            Snake snake = new Snake();
            snake.ID = 0;
            Point head = new Point(1, 16);
            Point tail = new Point(16, 16);
            List<Point> vertices = new List<Point>();
            vertices.Add(tail);
            vertices.Add(head);
            snake.vertices = vertices;

            world.AddSnake(snake);

            world.MoveSnake(snake, 2);

            Assert.AreEqual(world.snakes.Keys.Count, 1);

        }

        [TestMethod]
        public void growSnake3()
        {
            World world = new World(width, height, RecycleRate, StartingLength, gameMode);

            Snake snake = new Snake();
            snake.ID = 0;
            Point head = new Point(1, 1);
            Point tail = new Point(1, 16);
            List<Point> vertices = new List<Point>();
            vertices.Add(tail);
            vertices.Add(head);
            snake.vertices = vertices;

            world.AddSnake(snake);

            world.MoveSnake(snake, 3);

            Assert.AreEqual(world.snakes.Keys.Count, 1);

        }

        [TestMethod]
        public void growSnake()
        {
            World world = new World(width, height, RecycleRate, StartingLength, gameMode);

            world.createSnake("n", 1);
            Snake snake = world.snakes[1];

            world.MoveSnake(snake, 4);

            Assert.AreEqual(world.snakes[0].vertices[0], new Point(-1, -1));

        }

        [TestMethod]
        public void moveSnake1()
        {
            foodDensity = 48;
            width = 12;
            height = 12;

            World world = new World(width, height, RecycleRate, 1, gameMode);

            Snake snake = new Snake();
            snake.ID = 0;
            snake.name = "bob";
            Point head = new Point(5, 4);
            Point tail = new Point(5, 5);
            List<Point> vertices = new List<Point>();
            vertices.Add(tail);
            vertices.Add(head);
            snake.vertices = vertices;
            
            world.AddSnake(snake);
            Assert.IsTrue(world.snakes.Count > 0);

            world.createFood(foodDensity);
            try
            {
                world.createSnake("yes", 1);
                world.createSnake("yes", 2);
                world.createSnake("yes", 3);
                world.createSnake("yes", 4);
                world.createSnake("yes", 5);
                world.createSnake("yes", 6);
                world.createSnake("yes", 7);
                world.createSnake("yes", 8);
                
        
                world.foodPoint.Clear();
                
                world.snakes.Clear();
                world.removeFoodPoint(new Point(-1, -1));
                world.removeFoodPoint(new Point(5, 5));
                world.createSnake("yes", 4);
                world.createSnake("yes", 5);
                world.createSnake("yes", 6);
                world.createSnake("yes", 7);
                world.MoveSnake(world.snakes[0], 2);
                world.MoveSnake(world.snakes[0], 3);
                world.MoveSnake(world.snakes[0], 4);
                world.MoveSnake(world.snakes[0], 1);
                world.MoveSnake(world.snakes[1], 2);
                world.MoveSnake(world.snakes[3], 2);
                world.MoveSnake(world.snakes[3], 2);

                world.createSnake("yes", 9);


            }
            catch { }

            int foodCount = world.foodPoint.Count;

           // world.MoveSnake(snake, 1);
            //world.MoveSnake(snake, 1);
           // world.MoveSnake(snake, 1);

           // int foodCount2 = world.foodPoint.Count;
           // Assert.AreEqual(foodCount< foodCount2, true);
        
           

            //Assert.AreEqual(world.foodPoint.Keys.Count, 2);

            ///Assert.AreEqual(world.snakes.Keys.Count, 1);
            
        }

        [TestMethod]
        public void makeWorld()
        {
            World world = new World();
            Assert.AreEqual(world.width, 0);
        }

        [TestMethod]
        public void changeRecycle()
        {
            RecycleRate = .1;
            width = 15;
            height = 15;
            StartingLength = 2;
            World world = new World(width, height, RecycleRate, StartingLength, gameMode);

            world.createSnake("n", 0);
            Snake snake = world.snakes[0];
            while (world.snakes[0].vertices[0].x >= 1)
            {
                world.MoveSnake(snake, 1);
            }
          


            Assert.AreEqual(world.snakes[0].vertices[0], new Point(-1,-1));
            
        }

    }
}
