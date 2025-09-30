// A C# program for Prim's Minimum Spanning Tree (MST)
// algorithm adapted to work with (double, double) nodes and Edge classes

using System;
using System.Collections.Generic;
using System.Linq;
using DungeonGeneratorApp;

namespace DungeonGeneratorTest;

public class PrimMST
{
    private List<(double, double)> nodes;
    private List<Edge> edges;

    public PrimMST(List<(double, double)> nodes, List<Edge> edges)
    {
        if (nodes == null || nodes.Count < 2)
            throw new ArgumentException("At least two nodes required for MST");
        if (edges == null || edges.Count < 1)
            throw new ArgumentException("At least one edge required for MST");

        this.nodes = nodes;
        this.edges = edges;
    }

    // Find the minimum weight edge from the cut
    private Edge? FindMinEdge(HashSet<(double, double)> inMST, HashSet<(double, double)> notInMST)
    {
        Edge? minEdge = null;
        double minWeight = double.MaxValue;

        foreach (var edge in edges)
        {
            // Check if edge connects MST to non-MST vertex
            bool aInMST = inMST.Contains(edge.A);
            bool bInMST = inMST.Contains(edge.B);

            // Valid cut edge: exactly one endpoint in MST
            if (aInMST != bInMST && edge.Weight < minWeight)
            {
                minEdge = edge;
                minWeight = edge.Weight;
            }
        }

        return minEdge;
    }

    // Implementation of Prim's algorithm
    public List<Edge> MST()
    {
        var mst = new List<Edge>();
        var inMST = new HashSet<(double, double)>();
        var notInMST = new HashSet<(double, double)>(nodes);

        // Start with the first node
        var startNode = nodes[0];
        inMST.Add(startNode);
        notInMST.Remove(startNode);

        // Build MST by adding minimum weight edges
        while (notInMST.Count > 0)
        {
            var minEdge = FindMinEdge(inMST, notInMST);
            
            if (minEdge == null)
            {
                // Graph is disconnected
                return new List<Edge>();
            }

            mst.Add(minEdge);

            // Add the new vertex to MST
            if (inMST.Contains(minEdge.A))
            {
                inMST.Add(minEdge.B);
                notInMST.Remove(minEdge.B);
            }
            else
            {
                inMST.Add(minEdge.A);
                notInMST.Remove(minEdge.A);
            }
        }

        return mst;
    }

    // Print the MST for debugging
    public void PrintMST(List<Edge> mst)
    {
        Console.WriteLine("Edge \t\tWeight");
        double totalWeight = 0;

        foreach (var edge in mst)
        {
            Console.WriteLine($"({edge.A.Item1:F2}, {edge.A.Item2:F2}) - ({edge.B.Item1:F2}, {edge.B.Item2:F2})\t{edge.Weight:F3}");
            totalWeight += edge.Weight;
        }

        Console.WriteLine($"Total Weight: {totalWeight:F3}");
    }
}