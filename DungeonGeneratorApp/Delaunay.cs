using System;

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
        triangulation.Add(new Triangle(
            (-maxX, 1.1 * maxY),
            (2 * maxX, 1.1 * maxY),
            (maxX, -1.9 * maxY)
        ));

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