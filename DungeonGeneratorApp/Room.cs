using System;

namespace DungeonGeneratorApp;

/// <summary>
/// Represents a rectangular room in the dungeon.
/// </summary>
public class Room
{
    private static int nextId = 0;

    /// <summary>
    /// Unique identifier for the room.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// X coordinate of the room's left edge.
    /// </summary>
    public int X { get; }
    
    /// <summary>
    /// Y coordinate of the room's top edge.
    /// </summary>
    public int Y { get; }
    
    /// <summary>
    /// Width of the room in tiles.
    /// </summary>
    public int Width { get; }
    
    /// <summary>
    /// Height of the room in tiles.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// List of exit positions on the room's walls.
    /// </summary>
    public List<(int, int)> Exits { get; }

    /// <summary>
    /// X coordinate of the room's center.
    /// </summary>
    public double CenterX => X + Width / 2.0;
    
    /// <summary>
    /// Y coordinate of the room's center.
    /// </summary>
    public double CenterY => Y + Height / 2.0;

    /// <summary>
    /// Creates a new room with the specified dimensions.
    /// </summary>
    /// <param name="x">X coordinate of the left edge.</param>
    /// <param name="y">Y coordinate of the top edge.</param>
    /// <param name="width">Width in tiles.</param>
    /// <param name="height">Height in tiles.</param>
    public Room(int x, int y, int width, int height)
    {
        Id = nextId++;
        X = x;
        Y = y;
        Width = width;
        Height = height;
        Exits = new List<(int, int)>();
    }

    /// <summary>
    /// Randomly generates exit positions on all four walls of the room.
    /// </summary>
    /// <param name="random">Random number generator to use.</param>
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

    /// <summary>
    /// Resets the room ID counter to 0.
    /// </summary>
    public static void ResetIdCounter() { nextId = 0; }
    
    /// <summary>
    /// Gets the center point of the room.
    /// </summary>
    /// <returns>Center coordinates as (x, y).</returns>
    public (double x, double y) GetCenter()
    {
        return (CenterX, CenterY);
    }
}