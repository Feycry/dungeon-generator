
using System;

namespace DungeonGeneratorApp;

public enum TileType
{
    Empty,
    Room,
    Path
}

public class Tile
{
    public int X;
    public int Y;
    public int GCost;
    public int HCost;
    public int FCost => GCost + HCost;
    public TileType type = TileType.Empty;
    public int Cost;
    public Tile Parent;

    public Tile(int x, int y)
    {
        X = x;
        Y = y;
        Cost = 0;
        Parent = this;
    }
}