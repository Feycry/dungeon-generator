using System;

namespace DungeonGeneratorApp;

public class Room
{
    private static int nextId = 0;

    public int Id { get; }

    public int X { get; }
    public int Y { get; }
    public int Width { get; }
    public int Height { get; }

    //Use doubles for the sake of the graph algorithms
    public double CenterX => X + Width / 2.0;
    public double CenterY => Y + Height / 2.0;

    public Room(int x, int y, int width, int height)
    {
        Id = nextId++;
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public override string ToString() => $"Room {Id}: ({X},{Y},{Width},{Height})";

    public static void ResetIdCounter() { nextId = 0; }
    
    public (double x, double y) GetCenter()
    {
        return (CenterX, CenterY);
    }
}