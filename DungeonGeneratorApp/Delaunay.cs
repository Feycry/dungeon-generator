using System;
using System.Diagnostics;

namespace DungeonGeneratorApp;

public class Triangle {
    public (double x, double y) A { get; set; }
    public (double x, double y) B { get; set; }
    public (double x, double y) C { get; set; }

    public List<Edge> Edges = new List<Edge>();

    public Triangle((double x, double y) a, (double x, double y) b, (double x, double y) c)
    {
        var vertices = new List<(double x, double y)> { a, b, c };
        vertices.Sort((v1, v2) =>
        {
            int cmp = v1.x.CompareTo(v2.x);
            if (cmp == 0)
                return v1.y.CompareTo(v2.y);
            return cmp;
        });

        A = vertices[0];
        B = vertices[1];
        C = vertices[2];

        Edges.Add(new Edge(A, B));
        Edges.Add(new Edge(B, C));
        Edges.Add(new Edge(C, A));
    }

    //https://en.wikipedia.org/wiki/Circumcircle#Cartesian_coordinates
    public bool InCircumcircle((double x, double y) point)
    {
        var a = A;
        var b = B;
        var c = C;

        b = (b.x - a.x, b.y - a.y);
        c = (c.x - a.x, c.y - a.y);

        var d = 2 * (b.x * c.y - b.y * c.x);

        var ux = (c.y * (Math.Pow(b.x, 2) + Math.Pow(b.y, 2)) - b.y * (Math.Pow(c.x, 2) + Math.Pow(c.y, 2))) / d;
        var uy = (b.x * (Math.Pow(c.x, 2) + Math.Pow(c.y, 2)) - c.x * (Math.Pow(b.x, 2) + Math.Pow(b.y, 2))) / d;

        //Now U' = (ux, uy) is the circumcenter relative to A
        //The actual circumcenter U = U' + A = (ux + a.x, uy + a.y)
        var circumcenterX = ux + a.x;
        var circumcenterY = uy + a.y;

        //The radius of the circumcircle is
        var radius = Math.Sqrt(Math.Pow(ux, 2) + Math.Pow(uy, 2));

        //Calculate distance from point to the actual circumcenter
        var distance = Math.Sqrt(Math.Pow(point.x - circumcenterX, 2) + Math.Pow(point.y - circumcenterY, 2));

        //If it's nearer than the radius, it's inside the circumcircle
        return distance < radius;
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
        return Edges.Select(e => e.GetLine()).ToList();
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
            (-5 * maxX, 5 * maxY),
            (5 * maxX, 5 * maxY),
            (5 * maxX, -5 * maxY)
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


        foreach (var node in nodes)
        {
            var badTriangles = new List<Triangle>();

            foreach (var tri in triangulation)
            {
                if (tri.InCircumcircle(node))
                {
                    badTriangles.Add(tri);
                }
            }

            var polygon = new List<Edge>();

            foreach (var tri in badTriangles)
            {
                foreach (var edge in tri.Edges)
                {
                    //if edge is not shared by any other triangles in badTriangles
                    var shared = false;
                    foreach (var tri2 in badTriangles)
                    {
                        foreach (var edge2 in tri2.Edges)
                        {
                            if (tri != tri2 && edge.Equals(edge2)) shared = true;
                        }
                    }

                    if (!shared) polygon.Add(edge);
                }
            }

            foreach (var tri in badTriangles)
            {
                //Remove from triagulation
                triangulation.Remove(tri);
            }

            foreach (var edge in polygon)
            {
                //Add new triangle to triangulation
                triangulation.Add(new Triangle(
                    edge.A,
                    edge.B,
                    (node.x, node.y)
                ));
            }

        }

        //DEBUG: Create snapshot
        DebugSnapshotManager.Instance.SetCategory("delaunay triangulation before cleanup");
        triangleLines = new List<(double x1, double y1, double x2, double y2)>();
        trianglePoints = new List<(double x, double y)>();
        // Add all nodes (room centers) to the points for visualization
        trianglePoints.AddRange(nodes);
        foreach (var triangle in triangulation)
        {
            triangleLines.AddRange(triangle.GetLines());
        }
        DebugSnapshotManager.Instance.AddSnapshot(trianglePoints, triangleLines);

        //Clean up triangles that contain super triangle nodes
        var trianglesToRemove = new List<Triangle>();
        foreach (var tri in triangulation)
        {
            if (tri.A == SuperTriangle.A || tri.A == SuperTriangle.B || tri.A == SuperTriangle.C ||
                tri.B == SuperTriangle.A || tri.B == SuperTriangle.B || tri.B == SuperTriangle.C ||
                tri.C == SuperTriangle.A || tri.C == SuperTriangle.B || tri.C == SuperTriangle.C)
            {
                trianglesToRemove.Add(tri);
            }
        }

        foreach (var tri in trianglesToRemove)
        {
            triangulation.Remove(tri);
        }

        //Extract edges from remaining valid triangles
        foreach (var tri in triangulation)
        {
            foreach (var edge in tri.Edges)
            {
                if (!delaunayEdges.Contains(edge))
                    delaunayEdges.Add(edge);
            }
        }

        //DEBUG: Create snapshot of complete triangulation
        DebugSnapshotManager.Instance.SetCategory("delaunay triangulation done");
        triangleLines = new List<(double x1, double y1, double x2, double y2)>();
        trianglePoints = new List<(double x, double y)>();
        //Add all nodes (room centers) to the points for visualization
        trianglePoints.AddRange(nodes);
        //Add all edges from delaunayEdges for visualization
        triangleLines.AddRange(delaunayEdges.Select(e => e.GetLine()));
        DebugSnapshotManager.Instance.AddSnapshot(trianglePoints, triangleLines);

        return delaunayEdges;
    }
}