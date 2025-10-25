using System;

namespace DungeonGeneratorApp;

/// <summary>
/// Union-Find data structure for efficient set operations.
/// </summary>
public class UnionFind
{
    Dictionary<(double, double), (double, double)> link = new Dictionary<(double, double), (double, double)>();
    Dictionary<(double, double), double> size = new Dictionary<(double, double), double>();

    /// <summary>
    /// Initializes the Union-Find structure with the given nodes.
    /// </summary>
    /// <param name="nodes">List of nodes to initialize.</param>
    public UnionFind(List<(double, double)> nodes)
    {
        foreach (var node in nodes)
        {
            link[node] = node;
            size[node] = 1;
        }
    }

    /// <summary>
    /// Finds the root representative of the set containing the given node.
    /// </summary>
    /// <param name="x">The node to find.</param>
    /// <returns>The root representative of the set.</returns>
    public (double, double) Find((double, double) x)
    {
        if (!link[x].Equals(x))
        {
            link[x] = Find(link[x]);
        }

        return link[x];
    }
    
    /// <summary>
    /// Merges the sets containing the two given nodes.
    /// </summary>
    /// <param name="a">First node.</param>
    /// <param name="b">Second node.</param>
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

/// <summary>
/// Computes the minimum spanning tree using Kruskal's algorithm.
/// </summary>
public class MinimumSpanningTree
{
    private List<Edge> edges = new List<Edge>();
    private List<(double, double)> nodes = new List<(double, double)>();

    /// <summary>
    /// Creates a minimum spanning tree calculator with the given edges and nodes.
    /// </summary>
    /// <param name="edges">List of edges to consider.</param>
    /// <param name="nodes">List of nodes in the graph.</param>
    public MinimumSpanningTree(List<Edge> edges, List<(double, double)> nodes)
    {
        if (edges == null || edges.Count < 1 || nodes.Count < 2)
            throw new ArgumentException("At least one edge and two nodes required for MST");

        this.edges = edges;
        this.nodes = nodes;

        edges.Sort((x, y) => x.Weight.CompareTo(y.Weight));

    }

    /// <summary>
    /// Computes the minimum spanning tree using Kruskal's algorithm.
    /// </summary>
    /// <returns>List of edges forming the MST, or empty list if construction fails.</returns>
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