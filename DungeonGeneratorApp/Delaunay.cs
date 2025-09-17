using System;

namespace DungeonGeneratorApp;

public class Triangle {
    public (double x, double y) A { get; set; }
    public (double x, double y) B { get; set; }
    public (double x, double y) C { get; set; }

    public Triangle((double x, double y) a, (double x, double y) b, (double x, double y) c)
    {
        A = a;
        B = b;
        C = c;
    }

    //For unit tests
    public bool ContainsPoint((double x, double y) point)
    {
        var v0 = (C.x - A.x, C.y - A.y);
        var v1 = (B.x - A.x, B.y - A.y);
        var v2 = (point.x - A.x, point.y - A.y);

        var dot00 = v0.Item1 * v0.Item1 + v0.Item2 * v0.Item2;
        var dot01 = v0.Item1 * v1.Item1 + v0.Item2 * v1.Item2;
        var dot02 = v0.Item1 * v2.Item1 + v0.Item2 * v2.Item2;
        var dot11 = v1.Item1 * v1.Item1 + v1.Item2 * v1.Item2;
        var dot12 = v1.Item1 * v2.Item1 + v1.Item2 * v2.Item2;

        var invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
        var u = (dot11 * dot02 - dot01 * dot12) * invDenom;
        var v = (dot00 * dot12 - dot01 * dot02) * invDenom;

        return (u >= 0) && (v >= 0) && (u + v <= 1);
    }

    //DEBUG
    public List<(double x1, double y1, double x2, double y2)> GetLines()
    {
        return new List<(double x1, double y1, double x2, double y2)>
        {
            (A.x, A.y, B.x, B.y),
            (B.x, B.y, C.x, C.y),
            (C.x, C.y, A.x, A.y)
        };
    }
}

//Bowyer–Watson algorithm implementation
public class Delaunay
{
    double maxX;
    double maxY;
    private List<(double x, double y)> nodes = new List<(double x, double y)>();
    
    //For unit tests
    public Triangle? SuperTriangle { get; private set; }

    public Delaunay(List<(double x, double y)> nodes)
    {
        if (nodes == null || nodes.Count < 3)
            throw new ArgumentException("At least three nodes are required for triangulation");

        this.nodes = nodes;

        maxX = Math.Ceiling(nodes.Max(n => n.x));
        maxY = Math.Ceiling(nodes.Max(n => n.y));
    }

    public List<Edge> Triangulate()
    {
        var delaunayEdges = new List<Edge>();
        var triangulation = new List<Triangle>();

        //Add super triangle
        SuperTriangle = new Triangle(
            (-0.6 * maxX, 1.2 * maxY),
            (1.6 * maxX, 1.2 * maxY),
            (0.6 * maxX, -1.2 * maxY)
        );
        triangulation.Add(SuperTriangle);

        //DEBUG: Create snapshot of super triangle
        DebugSnapshotManager.Instance.SetCategory("delaunay triangulation");
        var triangleLines = new List<(double x1, double y1, double x2, double y2)>();
        var trianglePoints = new List<(double x, double y)>();
        // Add all nodes (room centers) to the points for visualization
        trianglePoints.AddRange(nodes);
        foreach (var triangle in triangulation)
        {
            triangleLines.AddRange(triangle.GetLines());
        }
        DebugSnapshotManager.Instance.AddSnapshot(trianglePoints, triangleLines);

        

        return delaunayEdges;
    }
}