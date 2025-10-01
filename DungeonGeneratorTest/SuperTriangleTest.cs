using DungeonGeneratorApp;

namespace DungeonGeneratorTest;

[TestClass]
public sealed class SuperTriangleTest
{
    [TestMethod]
    public void TestTriangleContains()
    {
        var triangle = new Triangle((0, 0), (10, 0), (5, 10));

        //Points inside
        var point = (5, 2);
        Assert.IsTrue(triangle.ContainsPoint(point), $"{point} should be inside triangle");
        point = (3, 3);
        Assert.IsTrue(triangle.ContainsPoint(point), $"{point} should be inside triangle");

        //Points outside
        point = (-1, 0);
        Assert.IsFalse(triangle.ContainsPoint(point), $"{point} should be outside triangle");
        point = (15, 0);
        Assert.IsFalse(triangle.ContainsPoint(point), $"{point} should be outside triangle");
        point = (5, 15);
        Assert.IsFalse(triangle.ContainsPoint(point), $"{point} should be outside triangle");
    }

    [TestMethod]
    public void TestSuperTriangleAllNodesInside()
    {
        //Arrange
        var nodes = new List<(double x, double y)>
        {
            (10, 10),
            (20, 15),
            (15, 25),
            (5, 20),
            (25, 5),
            (0, 0),
            (30, 30)
        };

        var delaunay = new Delaunay(nodes);

        //Act
        delaunay.Triangulate();

        Assert.IsNotNull(delaunay.SuperTriangle, "super triangle can't be null");

        foreach (var n in nodes)
        {
            bool isInside = delaunay.SuperTriangle.ContainsPoint(n);
            Assert.IsTrue(isInside, $"({n.x}, {n.y}) should be inside super triangle");
        }
    }

    [TestMethod]
    public void TestSuperTriangleLargeCoordinates()
    {
        //Arrange
        var nodes = new List<(double x, double y)>
        {
            (10000000, 50000000),
            (15000000, 12000000),
            (80000000, 11000000)
        };

        var delaunay = new Delaunay(nodes);

        //Act
        delaunay.Triangulate();

        Assert.IsNotNull(delaunay.SuperTriangle);
        foreach (var n in nodes)
        {
            Assert.IsTrue(delaunay.SuperTriangle.ContainsPoint(n), $"({n.x}, {n.y}) should be inside super triangle");
        }
    }

    [TestMethod]
    public void TestSuperTriangleWide()
    {
        //Arrange
        var nodes = new List<(double x, double y)>
        {
            (10000000, 1),
            (15000000, 3),
            (80000000, 2)
        };

        var delaunay = new Delaunay(nodes);

        //Act
        delaunay.Triangulate();

        Assert.IsNotNull(delaunay.SuperTriangle);
        foreach (var n in nodes)
        {
            Assert.IsTrue(delaunay.SuperTriangle.ContainsPoint(n), $"({n.x}, {n.y}) should be inside super triangle");
        }
    }
    
    [TestMethod]
    public void TestSuperTriangleNarrow()
    {
        //Arrange
        var nodes = new List<(double x, double y)>
        {
            (4, 1209345),
            (2, 123513946),
            (5, 103947562340986)
        };

        var delaunay = new Delaunay(nodes);

        //Act
        delaunay.Triangulate();

        Assert.IsNotNull(delaunay.SuperTriangle);
        foreach (var n in nodes)
        {
            Assert.IsTrue(delaunay.SuperTriangle.ContainsPoint(n), $"({n.x}, {n.y}) should be inside super triangle");
        }
    }
}
