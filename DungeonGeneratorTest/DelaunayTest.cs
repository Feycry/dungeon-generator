using DungeonGeneratorApp;

namespace DungeonGeneratorTest;

[TestClass]
public class DelaunayTest
{
    [TestMethod]
    public void TestTriangulation()
    {
        //Arrange
        var nodes = new List<(double x, double y)>
        {
            (0, 0),
            (10, 0),
            (10, 10),
            (0, 10)
        };

        var delaunay = new Delaunay(nodes);

        //Act
        var edges = delaunay.Triangulate();

        //Assert
        Assert.AreEqual(5, edges.Count, "Square should produce 5 edges");
    }

    [TestMethod]
    public void TestTriangleTriangulation()
    {
        //Arrange
        var nodes = new List<(double x, double y)>
        {
            (0, 0),
            (10, 0),
            (5, 10)
        };

        var delaunay = new Delaunay(nodes);

        //Act
        var edges = delaunay.Triangulate();

        //Assert
        Assert.AreEqual(3, edges.Count, "Triangle should produce 3 edges");
        
        //Verify all original points are connected
        var points = edges.SelectMany(e => new[] { e.A, e.B }).Distinct().ToList();
        Assert.AreEqual(3, points.Count, "All 3 points should be connected");
    }

    [TestMethod]
    public void TestCollinearPoints()
    {
        //Arrange
        //Create points that are on the same line
        var nodes = new List<(double x, double y)>
        {
            (0, 0),
            (5, 0),
            (10, 0),
            (15, 0)
        };

        var delaunay = new Delaunay(nodes);

        //Act
        var edges = delaunay.Triangulate();

        //Assert
        Assert.IsTrue(edges.Count > 0, "Collinear points should produce some triangulation");
        
        //Verify edges connect existing points
        var allPoints = new HashSet<(double x, double y)>(nodes);
        foreach (var edge in edges)
        {
            Assert.IsTrue(allPoints.Contains(edge.A));
            Assert.IsTrue(allPoints.Contains(edge.B));
        }
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestInsufficientPoints()
    {
        //Arrange
        var nodes = new List<(double x, double y)>
        {
            (0, 0),
            (10, 10)
        };

        //Act
        //Assert
        var delaunay = new Delaunay(nodes);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestNullInput()
    {
        //Act
        //Assert
        var delaunay = new Delaunay(null!);
    }

    [TestMethod]
    public void TestDuplicatePoints()
    {
        //Arrange
        var nodes = new List<(double x, double y)>
        {
            (0, 0),
            (10, 0),
            (5, 10),
            (0, 0),
            (10, 0)
        };

        var delaunay = new Delaunay(nodes);

        //Act
        var edges = delaunay.Triangulate();

        //Assert
        Assert.IsTrue(edges.Count > 0, "Should handle duplicate points");
    }

    [TestMethod]
    public void TestLargeCoordinates()
    {
        //Arrange
        var nodes = new List<(double x, double y)>
        {
            (1000000, 2000000),
            (1000010, 2000000),
            (1000005, 2000010)
        };

        var delaunay = new Delaunay(nodes);

        //Act
        var edges = delaunay.Triangulate();

        //Assert
        Assert.AreEqual(3, edges.Count, "Large coordinates should work correctly");
    }

    [TestMethod]
    public void TestNarrowCoordinates()
    {
        //Arrange
        var nodes = new List<(double x, double y)>
        {
            (1, 2000000),
            (3, 2000000),
            (2, 2000010)
        };

        var delaunay = new Delaunay(nodes);

        //Act
        var edges = delaunay.Triangulate();

        //Assert
        Assert.AreEqual(3, edges.Count, "Narrow coordinates should work correctly");
    }

    [TestMethod]
    public void TestWideCoordinates()
    {
        //Arrange
        var nodes = new List<(double x, double y)>
        {
            (1000000, 3),
            (1000010, 1),
            (1000005, 2)
        };

        var delaunay = new Delaunay(nodes);

        //Act
        var edges = delaunay.Triangulate();

        //Assert
        Assert.AreEqual(3, edges.Count, "Wide coordinates should work correctly");
    }

    [TestMethod]
    public void TestNegativeCoordinates()
    {
        //Arrange
        var nodes = new List<(double x, double y)>
        {
            (-10, -10),
            (0, -5),
            (-5, 5),
            (10, 10)
        };

        var delaunay = new Delaunay(nodes);

        //Act
        var edges = delaunay.Triangulate();

        //Assert
        Assert.IsTrue(edges.Count > 0, "Negative coordinates should work");
        Assert.IsTrue(edges.Count >= 3, "Should have reasonable number of edges");
    }
}