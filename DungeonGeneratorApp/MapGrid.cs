using System;

namespace DungeonGeneratorApp;

/// <summary>
/// Represents the dungeon map as a grid of tiles.
/// </summary>
public class MapGrid
{
    private const int roomWallCost = 100;
    private const int roomInsideCost = 0;

    int width;
    int height;
    Tile[,] tiles;

    /// <summary>
    /// Creates a new map grid with the specified dimensions.
    /// </summary>
    /// <param name="width">Width of the grid in tiles.</param>
    /// <param name="height">Height of the grid in tiles.</param>
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

    /// <summary>
    /// Gets the tile at the specified coordinates.
    /// </summary>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    /// <returns>The tile at the specified position.</returns>
    public Tile GetTile(int x, int y)
    {
        return tiles[x, y];
    }

    /// <summary>
    /// Marks tiles along a path as walkable path tiles.
    /// </summary>
    /// <param name="path">List of tiles forming the path.</param>
    public void AddPath(List<Tile> path)
    {
        foreach (var tile in path) {
            //if (tile.type == TileType.Empty)
            //{
                tile.type = TileType.Path;
                tile.Cost = 0;
            //}

        }
    }

    /// <summary>
    /// Attempts to place a room on the grid.
    /// </summary>
    /// <param name="room">Room dimensions as (x, y, width, height).</param>
    /// <returns>True if the room was placed successfully without overlaps.</returns>
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

    /// <summary>
    /// Sets movement costs for tiles in a room based on walls and exits.
    /// </summary>
    /// <param name="room">The room to add costs for.</param>
    public void AddRoomCosts(Room room)
    {
        for (int x = room.X; x < room.X + room.Width && x < width; x++)
        {
            for (int y = room.Y; y < room.Y + room.Height && y < height; y++)
            {
                if (x < 0 || y < 0) continue;

                if (room.Height == 0 || room.Width == 0)
                {
                    tiles[x, y].Cost = roomInsideCost;
                    continue;
                }

                bool isOuterEdge = (x == room.X || x == room.X + room.Width - 1 || y == room.Y || y == room.Y + room.Height - 1);

                if (isOuterEdge && !room.Exits.Contains((x, y)))
                    tiles[x, y].Cost = roomWallCost;
                else
                    tiles[x, y].Cost = roomInsideCost;
            }
        }
    }

    /// <summary>
    /// Gets all neighboring tiles of the given tile.
    /// </summary>
    /// <param name="tile">The tile to get neighbors for.</param>
    /// <param name="allowDiagonals">Whether to include diagonal neighbors.</param>
    /// <returns>List of neighboring tiles.</returns>
    public List<Tile> GetNeighbours(Tile tile, bool allowDiagonals = true)
    {
        var ret = new List<Tile>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                // Skip diagonal neighbors if diagonals are not allowed
                if (!allowDiagonals && x != 0 && y != 0)
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

    /// <summary>
    /// Returns the map as a boolean array where true represents non-empty tiles.
    /// </summary>
    /// <returns>2D boolean array representing the map.</returns>
    public bool[,] GetBooleanMap()
    {
        var map = new bool[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = tiles[x, y].type != TileType.Empty;
            }
        }
        return map;
    }

    /// <summary>
    /// Prints the map to the console for debugging.
    /// </summary>
    public void Print()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                switch (tiles[x, y].type)
                {
                    case TileType.Empty:
                        Console.Write('.');
                        break;
                    case TileType.Path:
                        Console.Write('0');
                        break;
                    case TileType.Room:
                        Console.Write('#');
                        break;
                }

            }
            Console.WriteLine();
        }

        /*
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Console.Write($"{tiles[x, y].Cost,3} ");
            }
            Console.WriteLine();
        }
        */
    }
}