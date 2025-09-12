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

    public void AddRoom((int x, int y, int w, int h) room)
    {
        for (int i = room.x; i < room.x + room.w && i < width; i++)
        {
            for (int j = room.y; j < room.y + room.h && j < height; j++)
            {
                if (i >= 0 && j >= 0)
                {
                    grid[i, j] = true;
                }
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