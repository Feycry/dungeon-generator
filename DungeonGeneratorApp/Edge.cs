using System;

namespace DungeonGeneratorApp;

/// <summary>
/// Represents an edge between two points with a calculated weight.
/// </summary>
public class Edge
{
    /// <summary>
    /// First endpoint of the edge.
    /// </summary>
    public (double x, double y) A { get; }
    
    /// <summary>
    /// Second endpoint of the edge.
    /// </summary>
    public (double x, double y) B { get; }

    /// <summary>
    /// Euclidean distance between the two endpoints.
    /// </summary>
    public double Weight => Math.Sqrt(
        Math.Pow(B.x - A.x, 2) +
        Math.Pow(B.y - A.y, 2));

    /// <summary>
    /// Creates an edge between two points with consistent ordering.
    /// </summary>
    /// <param name="point1">First point.</param>
    /// <param name="point2">Second point.</param>
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
    
    /// <summary>
    /// Returns the edge as line coordinates for visualization.
    /// </summary>
    /// <returns>Line segment as (x1, y1, x2, y2) tuple.</returns>
    public (double x1, double y1, double x2, double y2) GetLine()
    {
        return (A.x, A.y, B.x, B.y);
    }
}