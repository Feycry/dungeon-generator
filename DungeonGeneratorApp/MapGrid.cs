using System;

namespace DungeonGeneratorApp;

public class MapGrid
{
    private const int roomWallCost = 100;
    private const int roomInsideCost = 0;

    int width;
    int height;
    bool[,] grid;
    int[,] cost;

    public MapGrid(int width, int height)
    {
        this.width = width;
        this.height = height;

        grid = new bool[width, height];
        cost = new int[width, height];
    }

    public bool AddRoom((int x, int y, int w, int h) room)
    {
        var toFill = new List<(int, int)>();

        for (int i = room.x; i < room.x + room.w && i < width; i++)
        {
            for (int j = room.y; j < room.y + room.h && j < height; j++)
            {
                if (i < 0 || j < 0) continue;
                if (grid[i, j] == true)
                {
                    //Overlap found, cancel
                    return false;
                }

                toFill.Add((i, j));
            }
        }

        //No overlap, fill the room
        foreach (var (i, j) in toFill)
        {
            grid[i, j] = true;
        }

        return true;
    }

    public void AddRoomCosts(Room room)
    {
        for (int x = room.X; x < room.X + room.Width && x < width; x++)
        {
            for (int y = room.Y; y < room.Y + room.Height && y < height; y++)
            {
                if (x < 0 || y < 0) continue;

                bool isOuterEdge = (x == room.X || x == room.X + room.Width - 1 || y == room.Y || y == room.Y + room.Height - 1);

                if (isOuterEdge && !room.Exits.Contains((x, y)))
                    cost[x, y] = roomWallCost;
                else
                    cost[x, y] = roomInsideCost;
            }
        }
    }

    public void Print()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Console.Write(grid[x, y] ? '#' : '.');
            }
            Console.WriteLine();
        }
    }
}