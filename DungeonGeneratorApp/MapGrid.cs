using System;

public class MapGrid
{
    int width;
    int height;
    bool[,] grid;

    public MapGrid(int width, int height)
    {
        this.width = width;
        this.height = height;

        grid = new bool[width, height];
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