using DungeonGeneratorApp;

namespace DungeonGeneratorTest;

[TestClass]
public sealed class KruskalTest
{
    [TestMethod]
    public void TestKruskalUnionFind()
    {
        //Arrange
        var nodes = new List<(double, double)> { (0, 0), (1, 1), (2, 2) };
        var uf = new UnionFind(nodes);

        //Assert
        Assert.AreEqual((0, 0), uf.Find((0, 0)));
        Assert.AreNotEqual(uf.Find((0, 0)), uf.Find((1, 1)));

        //Act
        uf.Union((0, 0), (1, 1));

        //Assert more :D
        Assert.AreEqual(uf.Find((0, 0)), uf.Find((1, 1)));
        Assert.AreNotEqual(uf.Find((0, 0)), uf.Find((2, 2)));
    }

    [TestMethod]
    public void TestKruskalSimple()
    {
        //Arrange
        var nodes = new List<(double, double)> { (0.0, 0.0), (1.0, 0.0), (0.0, 1.0) };
        var edges = new List<Edge>
        {
            new Edge((0.0, 0.0), (1.0, 0.0)),
            new Edge((1.0, 0.0), (0.0, 1.0)),
            new Edge((0.0, 1.0), (0.0, 0.0))
        };

        //Act
        var kruskalMST = new MinimumSpanningTree(edges, nodes);
        var kruskalResult = kruskalMST.MST();

        var primMST = new PrimMST(nodes, edges);
        var primResult = primMST.MST();

        //Assert
        for (int i = 0; i < primResult.Count; i++)
        {
            Assert.AreEqual(kruskalResult[i], primResult[i]);
        }
    }

    [TestMethod]
    public void TestKruskalRandom()
    {
        //Arrange
        //Create random nodes and edges
        int nodeCount = 200;
        double edgeChance = 0.1;

        var rand = new Random();
        var nodes = new List<(double, double)>();
        for (int i = 0; i < nodeCount; i++)
        {
            nodes.Add((rand.NextDouble() * 100, rand.NextDouble() * 100));
        }

        var edges = new List<Edge>();
        var edgeSet = new HashSet<(int, int)>();
        
        //First ensure connectivity by creating a spanning tree
        for (int i = 0; i < nodeCount - 1; i++)
        {
            edges.Add(new Edge(nodes[i], nodes[i + 1]));
            edgeSet.Add((i, i + 1));
        }
        
        //Then add random edges
        for (int i = 0; i < nodeCount; i++)
        {
            for (int j = i + 1; j < nodeCount; j++)
            {
                if (rand.NextDouble() < edgeChance)
                {
                    var key = (i, j);
                    if (!edgeSet.Contains(key))
                    {
                        edges.Add(new Edge(nodes[i], nodes[j]));
                        edgeSet.Add(key);
                    }
                }
            }
        }

        //Act
        var kruskalMST = new MinimumSpanningTree(edges, nodes);
        var kruskalResult = kruskalMST.MST();

        var primMST = new PrimMST(nodes, edges);
        var primResult = primMST.MST();

        //Assert
        var kruskalSet = new HashSet<Edge>(kruskalResult);
        var primSet = new HashSet<Edge>(primResult);
        
        Assert.AreEqual(kruskalSet.Count, primSet.Count);
        Assert.IsTrue(kruskalSet.SetEquals(primSet));
    }
}