using System;

public class Edge
{
    public Room Room1 { get; }
    public Room Room2 { get; }

    //Euclidean distance between room centers
    public double Weight => Math.Sqrt(
        Math.Pow(Room1.CenterX - Room2.CenterX, 2) +
        Math.Pow(Room1.CenterY - Room2.CenterY, 2));

    public Edge(Room room1, Room room2)
    {
        //Ensure consistent ordering for comparison
        if (room1.Id < room2.Id)
        {
            Room1 = room1;
            Room2 = room2;
        }
        else
        {
            Room1 = room2;
            Room2 = room1;
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Edge other) return false;
        return Room1.Id == other.Room1.Id && Room2.Id == other.Room2.Id;
    }

    public override int GetHashCode() => HashCode.Combine(Room1.Id, Room2.Id);

    public override string ToString() => $"Edge: {Room1.Id} - {Room2.Id} (Weight: {Weight})";
    
    //DEBUG
    public (double x1, double y1, double x2, double y2) GetLine()
    {
        return (Room1.CenterX, Room1.CenterY, Room2.CenterX, Room2.CenterY);
    }
}