
using System;

namespace DungeonGeneratorApp;

/// <summary>
/// Represents the type of a tile in the dungeon.
/// </summary>
public enum TileType
{
    /// <summary>
    /// Empty, unwalkable tile.
    /// </summary>
    Empty,
    
    /// <summary>
    /// Tile inside a room.
    /// </summary>
    Room,
    
    /// <summary>
    /// Tile that is part of a hallway path.
    /// </summary>
    Path
}

/// <summary>
/// Represents a single tile in the map grid.
/// </summary>
public class Tile
{
    /// <summary>
    /// X coordinate of the tile.
    /// </summary>
    public int X;
    
    /// <summary>
    /// Y coordinate of the tile.
    /// </summary>
    public int Y;
    
    /// <summary>
    /// Cost from start node in pathfinding.
    /// </summary>
    public int GCost;
    
    /// <summary>
    /// Heuristic cost to end node in pathfinding.
    /// </summary>
    public int HCost;
    
    /// <summary>
    /// Total cost (GCost + HCost) for pathfinding.
    /// </summary>
    public int FCost => GCost + HCost;
    
    /// <summary>
    /// Type of the tile (Empty, Room, or Path).
    /// </summary>
    public TileType type = TileType.Empty;
    
    /// <summary>
    /// Movement cost for pathfinding through this tile.
    /// </summary>
    public int Cost;
    
    /// <summary>
    /// Parent tile in the pathfinding algorithm.
    /// </summary>
    public Tile Parent;

    /// <summary>
    /// Creates a new tile at the specified coordinates.
    /// </summary>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    public Tile(int x, int y)
    {
        X = x;
        Y = y;
        Cost = 10;
        Parent = this;
    }
}