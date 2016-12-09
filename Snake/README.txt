Caelan Dailey & Karina Biancone

In a few methods in server we modified it to take a state in order to player disconnects.
We create an event that calls a method that clears the client out of the list of clients

A user can set the 'gamemode' == 1 in the settings.xml if they want to make the game mode SUPER SNEKS. Your tail never goes away. You will constantly grow.
 The idea of the game is to trap other sneks. No food is drawn. When you die no food is drawn. When you die your entire snake goes away. 

 The world takes in a settings file
 A player can control the starting length of the snake
 A player can control the game mode
 A player can control the food density
 A player can control the Recycle rate

 The starting length of the snake is how long the snake is when the snake is first created. Right when you spawn in the world that's how long you will start off as.
 You can still eat food and grow at the same rate.

 The food desnity makes more food spawn as there are more players in the world. Higher food density means more food for the sneks.

 The recycle rate is how much food is spawned when a snake dies. The value is between 0 and 1. 
 The amount of food may go over the density max since a snake dying creates a ton of food at that time.

 The mechanics of the game are as follows.
 Snakes can eat food
 World can create food
 Snakes can collide with walls, snakes, and food.
 Snakes can move
 Snakes can turn
 The world can create more snakes
 World can play different game modes

 The server can handle multiple clients. The server creates X food and sends the food and player objects across the network.
 The server code accepts the direction requests and move the player around the world.

 ISSUES with the program. 
 1. There seems to be issues with locking or threading. Food is sometimes not removed from the food list, but is removed from the world grid.
 Thus causing the snakes to not run into the food, but still drawing it. Attempts to SET both the food list and the world grid equal to each other and drawing/setting missing
 components has failed to make any change.

 2. As with above. This seems to do the same, but with snakes. Portions of snakes seems to not be completely removed. Snakes sometimes die randomly for no reason when there's nothing there.
 Cause of the issue seems to be the same as the first one. The snake is not being completely removed. It only happens with a high capacity of snakes. 

 3. Snakes will spawn over each other. The method that handles this checks if position in the grid in the direction that the snake is being created. 
 If there's is an object in the way, then it creates a new randomly generated spot and checks again. For some reason it doesnt check again and uses the wrong cord.
 When this happens the snake will spawn over another one and get 'cut' in half. The portion of the snake not hit is still alive. This causes snakes to spawn at a variety of lengths.

 Tests were not made for the server. Tests were made for the WORLD. Methods such as
1. public World(int _width, int _height, double _recycleRate, int _startingSnakeLength, int _gameMode)
2. public void removeFoodPoint(Point point)
3. public int createSnake(string name, int ID)
4. public void createFood(int foodDensity)
5. private bool EmptyPoint(Point point)
6. public void MoveSnake(Snake snake, int direction)
7. public void NewTail(Snake snake)
8. private void KillSnake(Snake snake)
9. private void MakeFood(Point point)

1. Constructor for the world. Creates world object for the server. Different than the client since it has world settings.
2. We decided to create a dictionary with food points. Found that it is easier to track a food by its point instead of an ID.
3. Creates a snake based on the name and client id from the server
4. Creates food. This is called whenever the server updates food. This generates a random cord thats open and then calls make food
5. Checks for an empty point
6. Moves the snakes and checks for collisions
7. Creates a new tail
8. Kills the snake. Called from move snake if collision
9. Makes food. Called from create food. Makes food takes in a point and makes food at the point. 

Overall: The idea of the program is to handle server information in the server class and add them to the world object. Then call on the world class to test objects in the world. 
This is how the server creates the laws of the world. 