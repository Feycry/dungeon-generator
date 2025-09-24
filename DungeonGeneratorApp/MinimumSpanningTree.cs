using System;

namespace DungeonGeneratorApp;

public class UnionFind
{
    Dictionary<(double, double), (double, double)> link = new Dictionary<(double, double), (double, double)>();
    Dictionary<(double, double), double> size = new Dictionary<(double, double), double>();

    public UnionFind(List<(double, double)> nodes)
    {
        foreach (var node in nodes)
        {
            link[node] = node;
            size[node] = 1;
        }
    }

    public (double, double) Find((double, double) x)
    {
        if (!link[x].Equals(x))
        {
            link[x] = Find(link[x]);
        }

        return link[x];
    }
    
    public void Union((double, double) a, (double, double) b)
    {
        var rootA = Find(a);
        var rootB = Find(b);

        if (rootA.Equals(rootB))
            return;

        if (size[rootA] < size[rootB])
        {
            link[rootA] = rootB;
            size[rootB] += size[rootA];
        }
        else
        {
            link[rootB] = rootA;
            size[rootA] += size[rootB];
        }
    }
}

public class MinimumSpanningTree
{
    private List<Edge> edges = new List<Edge>();
    private List<(double, double)> nodes = new List<(double, double)>();

    public MinimumSpanningTree(List<Edge> edges, List<(double, double)> nodes)
    {
        if (edges == null || edges.Count < 1 || nodes.Count < 2)
            throw new ArgumentException("At least one edge and two nodes required for MST");

        this.edges = edges;
        this.nodes = nodes;

        edges.Sort((x, y) => x.Weight.CompareTo(y.Weight));

    }

    //Implementation of Kruskal's algorithm
    public List<Edge> MST()
    {
        var mst = new List<Edge>();
        var uf = new UnionFind(nodes);
        int edges_count = 0;

        foreach (var edge in edges)
        {
            if (uf.Find(edge.A) != uf.Find(edge.B))
            {
                uf.Union(edge.A, edge.B);
                mst.Add(edge);
                edges_count += 1;
            }
        }

        if (edges_count != nodes.Count() - 1)
        {
            //Failed, return empty list
            return new List<Edge>();
        }

        return mst;
    }

}