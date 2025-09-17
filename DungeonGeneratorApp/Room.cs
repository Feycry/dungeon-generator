using System;

public class Room
{
    public int Id { get; }
    public int X { get; }
    public int Y { get; }
    public int Width { get; }
    public int Height { get; }
    
    //Use doubles for the sake of the graph algorithms
    public double CenterX => X + Width / 2.0;
    public double CenterY => Y + Height / 2.0;
    
    public Room(int id, int x, int y, int width, int height)
    {
        Id = id;
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }
    
    public override string ToString() => $"Room {Id}: ({X},{Y},{Width},{Height})";
}