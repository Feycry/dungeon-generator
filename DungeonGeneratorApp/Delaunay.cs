using System;

//Bowyer–Watson algorithm implementation
public class Delaunay
{
    private List<(double x, double y)> nodes = new List<(double x, double y)>();

    public Delaunay(List<(double x, double y)> nodes)
    {
        this.nodes = nodes;
    }

    public List<Edge> Triangulate()
    {
        var triangulation = new List<Edge>();

        return triangulation;
    }
}