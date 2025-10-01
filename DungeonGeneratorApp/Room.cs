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

    public List<(int, int)> Exits { get; }

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
        Exits = new List<(int, int)>();
    }

    //This function sometimes results in duplicates, not elegant but doesn't really matter
    public void PlanExits(Random random)
    {
        //Top
        int exitCount = random.Next(1, 3);
        for (int i = 0; i < exitCount; i++)
        {
            Exits.Add((random.Next(X, X + Width), Y));
        }

        //Bottom
        exitCount = random.Next(1, 3);
        for (int i = 0; i < exitCount; i++)
        {
            Exits.Add((random.Next(X, X + Width), Y + Height - 1));
        }

        //Left
        exitCount = random.Next(1, 3);
        for (int i = 0; i < exitCount; i++)
        {
            Exits.Add((X, random.Next(Y, Y + Height)));
        }

        //Right
        exitCount = random.Next(1, 3);
        for (int i = 0; i < exitCount; i++)
        {
            Exits.Add((X + Width - 1, random.Next(Y, Y + Height)));
        }
    }

    public override string ToString() => $"Room {Id}: ({X},{Y},{Width},{Height})";

    public static void ResetIdCounter() { nextId = 0; }
    
    public (double x, double y) GetCenter()
    {
        return (CenterX, CenterY);
    }
}