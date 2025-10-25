using DungeonGeneratorApp;

namespace DungeonGeneratorTest;

[TestClass]
public class EndToEndTest
{
    [TestMethod]
    public void TestCompleteDungeonGeneration()
    {
        //Arrange
        int width = 30;
        int height = 30;
        int seed = 12345;
        bool allowDiagonals = false;
        
        var fixedRooms = new List<(int, int, int, int)>
        {
            (5, 5, 4, 4)
        };

        var generator = new DungeonGenerator(
            width,
            height,
            seed,
            allowDiagonals,
            fixedRooms,
            roomCount: 10
        );

        //Act
        generator.Generate();
        var dungeonMap = generator.GetBooleanMap();

        //Assert
        Assert.AreEqual(width, dungeonMap.GetLength(0), "Map width should match input");
        Assert.AreEqual(height, dungeonMap.GetLength(1), "Map height should match input");

        bool fixedRoomExists = true;
        for (int x = 5; x < 9; x++)
        {
            for (int y = 5; y < 9; y++)
            {
                if (!dungeonMap[x, y])
                {
                    fixedRoomExists = false;
                    break;
                }
            }
        }
        Assert.IsTrue(fixedRoomExists, "Fixed room should exist");

        int walkableTiles = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (dungeonMap[x, y])
                {
                    walkableTiles++;
                }
            }
        }
        Assert.IsTrue(walkableTiles > 0, "Dungeon should have walkable tiles");
        Assert.IsTrue(walkableTiles >= 16, "Dungeon should have at least the fixed room tiles");
    }

    [TestMethod]
    public void TestDungeonGenerationSmall()
    {
        //Arrange
        int width = 15;
        int height = 15;
        int seed = 22222;
        bool allowDiagonals = false;

        var fixedRooms = new List<(int, int, int, int)>
        {
            (1, 1, 3, 3),
            (6, 6, 3, 3),
            (11, 1, 3, 3)
        };

        var generator = new DungeonGenerator(
            width,
            height,
            seed,
            allowDiagonals,
            fixedRooms,
            roomCount: 0
        );

        //Act
        generator.Generate();
        var dungeonMap = generator.GetBooleanMap();

        //Assert
        Assert.AreEqual(width, dungeonMap.GetLength(0), "Map width should match input");
        Assert.AreEqual(height, dungeonMap.GetLength(1), "Map height should match input");
        
        //Verify that there are walkable tiles
        int walkableTiles = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (dungeonMap[x, y])
                {
                    walkableTiles++;
                }
            }
        }
        //3x3x3 = 27
        Assert.IsTrue(walkableTiles >= 27, "Map should have at least the fixed rooms");
    }
}
