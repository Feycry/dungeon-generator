
using System;

namespace DungeonGeneratorApp;

public class Tile
{
    public int X;
    public int Y;
    public int GCost;
    public int HCost;
    public int FCost => GCost + HCost;
    public bool IsOccupied;
    public int Cost;

    public Tile(int x, int y)
    {
        X = x;
        Y = y;
        IsOccupied = false;
        Cost = 0;
    }
}