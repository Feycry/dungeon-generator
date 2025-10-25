using DungeonGeneratorApp;

namespace DungeonGeneratorTest;

[TestClass]
public class PathFinderTest
{
    [TestMethod]
    public void TestLargeRandomGrid()
    {
        Random random = new Random();
        
        for (int i = 0; i < 100; i++)
        {
            // Arrange
            int width = random.Next(50, 200);
            int height = random.Next(50, 200);
            
            MapGrid grid = new MapGrid(width, height);
            
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Tile tile = grid.GetTile(x, y);
                    tile.Cost = random.Next(0, 200);
                }
            }
            
            PathFinder pathFinder = new PathFinder(grid, allowDiagonals: true);
            
            var start = (random.Next(0, width), random.Next(0, height));
            var end = (random.Next(0, width), random.Next(0, height));
            
            //Act
            pathFinder.FindPath(start, end);
            
            //Assert - verify end tile is marked as Path (unless start tile is same as end tile)
            if (start != end)
            {
                Assert.AreEqual(TileType.Path, grid.GetTile(end.Item1, end.Item2).type);
            }
        }
        
        Console.WriteLine("Pathfinder succeeded 100 times");
    }
}
