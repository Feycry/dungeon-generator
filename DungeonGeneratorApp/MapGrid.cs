using System;

namespace DungeonGeneratorApp;

public class MapGrid
{
    private const int roomWallCost = 100;
    private const int roomInsideCost = 0;

    int width;
    int height;
    Tile[,] tiles;

    public MapGrid(int width, int height)
    {
        this.width = width;
        this.height = height;

        tiles = new Tile[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tiles[x, y] = new Tile(x, y);
            }
        }
    }

    public Tile GetTile(int x, int y)
    {
        return tiles[x, y];
    }

    public void AddPath(List<Tile> path)
    {
        foreach (var tile in path) {
            if (tile.type == TileType.Empty)
                tile.type = TileType.Path;
        }
    }

    public bool AddRoom((int x, int y, int w, int h) room)
    {
        var toFill = new List<(int, int)>();

        for (int i = room.x; i < room.x + room.w && i < width; i++)
        {
            for (int j = room.y; j < room.y + room.h && j < height; j++)
            {
                if (i < 0 || j < 0) continue;
                if (tiles[i, j].type != TileType.Empty)
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
            tiles[i, j].type = TileType.Room;
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
                    tiles[x, y].Cost = roomWallCost;
                else
                    tiles[x, y].Cost = roomInsideCost;
            }
        }
    }

    public List<Tile> GetNeighbours(Tile tile)
    {
        var ret = new List<Tile>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = tile.X + x;
                int checkY = tile.Y + y;

                if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
                {
                    ret.Add(tiles[checkX, checkY]);
                }
            }
        }

        return ret;
    }

    public void Print()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Console.Write(tiles[x, y].type != TileType.Empty ? '#' : '.');
            }
            Console.WriteLine();
        }
    }
}