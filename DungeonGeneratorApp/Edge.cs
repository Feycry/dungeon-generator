using System;

namespace DungeonGeneratorApp;

public class Edge
{
    public (double x, double y) A { get; }
    public (double x, double y) B { get; }

    //Euclidean distance between points
    public double Weight => Math.Sqrt(
        Math.Pow(B.x - A.x, 2) +
        Math.Pow(B.y - A.y, 2));

    public Edge((double x, double y) point1, (double x, double y) point2)
    {
        //Ensure consistent ordering for comparison
        if (point1.x < point2.x || (point1.x == point2.x && point1.y < point2.y))
        {
            A = point1;
            B = point2;
        }
        else
        {
            A = point2;
            B = point1;
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Edge other) return false;
        return A.Equals(other.A) && B.Equals(other.B);
    }

    public override int GetHashCode() => HashCode.Combine(A, B);

    public override string ToString() => $"Edge: ({A.x}, {A.y}) - ({B.x}, {B.y}) (Weight: {Weight})";
    
    //DEBUG
    public (double x1, double y1, double x2, double y2) GetLine()
    {
        return (A.x, A.y, B.x, B.y);
    }
}