using System;

namespace DungeonGeneratorApp;

/// <summary>
/// Generates procedural dungeons with rooms and hallways.
/// </summary>
public class DungeonGenerator
{
    private int width;
    private int height;
    private int seed;
    private bool allowDiagonals;

    private List<(int x, int y, int w, int h)> fixedRooms = new List<(int, int, int, int)>();
    private int roomCount;
    private int minRoomSideSize = 1;
    private int maxRoomSideSize = 9;
    private double roomSideSizeMean = 4.0;
    private double roomSideSizeVariance = 1.6;

    private List<Room> rooms = new List<Room>();
    private MapGrid grid = new MapGrid(1, 1);
    private Random random = new Random(0);
    private List<(double x, double y)> nodes = new List<(double, double)>();
    private List<Edge> delaunayEdges = new List<Edge>();
    private List<Edge> hallwayEdges = new List<Edge>();

    /// <summary>
    /// Creates a new dungeon generator with the specified parameters.
    /// </summary>
    /// <param name="width">Map width in cells.</param>
    /// <param name="height">Map height in cells.</param>
    /// <param name="seed">Random seed for generation. Use -1 for random seed.</param>
    /// <param name="allowDiagonals">Whether to allow diagonal pathfinding.</param>
    /// <param name="fixedRooms">Optional list of rooms to always generate at specific locations.</param>
    /// <param name="roomCount">Number of random rooms. Defaults to floor(0.1*(width+height)).</param>
    /// <param name="minRoomSideSize">Minimum room side length in cells.</param>
    /// <param name="maxRoomSideSize">Maximum room side length in cells.</param>
    /// <param name="roomSideSizeMean">Mean value for room size normal distribution.</param>
    /// <param name="roomSideSizeVariance">Variance for room size normal distribution.</param>
    public DungeonGenerator(
        int width,
        int height,
        int seed,
        bool allowDiagonals,
        List<(int x, int y, int w, int h)>? fixedRooms = null,
        int? roomCount = null,
        int minRoomSideSize = 1,
        int maxRoomSideSize = 9,
        double roomSideSizeMean = 4.0,
        double roomSideSizeVariance = 1.6)
    {
        this.width = width;
        this.height = height;
        this.seed = seed;
        this.allowDiagonals = allowDiagonals;

        if (fixedRooms != null)
            this.fixedRooms = new List<(int, int, int, int)>(fixedRooms);

        foreach (var room in this.fixedRooms)
        {
            if (room.x < 0 || room.y < 0 || room.x + room.w > width || room.y + room.h > height)
            {
                throw new ArgumentException($"Fixed room at ({room.x}, {room.y}, {room.w}, {room.h}) is out of map bounds.");
            }
        }

        this.roomCount = roomCount ?? (int)Math.Floor(0.1 * (width + height));
        this.minRoomSideSize = minRoomSideSize;
        this.maxRoomSideSize = maxRoomSideSize;
        this.roomSideSizeMean = roomSideSizeMean;
        this.roomSideSizeVariance = roomSideSizeVariance;
    }
    
    /// <summary>
    /// Generates the dungeon by placing rooms, creating triangulation, and connecting with hallways.
    /// </summary>
    public void Generate()
    {
        Console.WriteLine("Generation started");

        //DEBUG: Set dungeon bounds for debug snapshot manager
        DebugSnapshotManager.Instance.SetDungeonBounds(width, height);

        rooms = new List<Room>();
        grid = new MapGrid(width, height);
        nodes = new List<(double, double)>();
        delaunayEdges = new List<Edge>();
        hallwayEdges = new List<Edge>();


        if (seed == -1)
        {
            //Random seed if seed = -1
            seed = Environment.TickCount;
            random = new Random(seed);
            Console.WriteLine($"Using random seed: {seed}");
        }
        else
        {
            random = new Random(seed);
            Console.WriteLine($"Using provided seed: {seed}");
        }

        PlaceRooms();
        Triangulate();
        PlanHallways();
        SetGridCosts();
        CreateHallways();

        grid.Print();
    }

    /// <summary>
    /// Places fixed and random rooms on the map grid.
    /// </summary>
    private void PlaceRooms()
    {
        //Place initial rooms
        foreach (var room in this.fixedRooms)
        {
            if (grid.AddRoom(room))
            {
                rooms.Add(new Room(room.x, room.y, room.w, room.h));
            }
            else
            {
                throw new ArgumentException($"Fixed room ({room.x}, {room.y}, {room.w}, {room.h}) could not be placed (possibly overlapping)");
            }
        }

        for (int i = 0; i < roomCount; i++)
        {
            CreateRandomRoom();
        }

        foreach (var r in rooms)
        {
            var center = r.GetCenter();
            //Apply small offset to avoid collinear cases in triangulation
            var offsetCenter = (
                center.x + (random.NextDouble() - 0.5) * 1e-10,
                center.y + (random.NextDouble() - 0.5) * 1e-10
            );
            nodes.Add(offsetCenter);
        }

        //DEBUG: Add a single snapshot after all rooms have been placed
        DebugSnapshotManager.Instance.SetCategory("room placement");
        DebugSnapshotManager.Instance.AddSnapshot(nodes, new List<(double, double, double, double)>());
    }

    /// <summary>
    /// Creates a single random room with normally distributed size.
    /// </summary>
    /// <returns>True if the room was successfully placed.</returns>
    private bool CreateRandomRoom()
    {
        int w = Math.Max(minRoomSideSize, Math.Min(maxRoomSideSize, (int)Math.Round(NormalRandom(roomSideSizeMean, roomSideSizeVariance))));
        int h = Math.Max(minRoomSideSize, Math.Min(maxRoomSideSize, (int)Math.Round(NormalRandom(roomSideSizeMean, roomSideSizeVariance))));

        //Sample center coordinates uniformly
        double minCx = w / 2.0;
        double maxCx = width - w / 2.0;
        double minCy = h / 2.0;
        double maxCy = height - h / 2.0;

        double cx = minCx + random.NextDouble() * (maxCx - minCx);
        double cy = minCy + random.NextDouble() * (maxCy - minCy);

        int x = (int)Math.Round(cx - w / 2.0);
        int y = (int)Math.Round(cy - h / 2.0);

        if (!grid.AddRoom((x, y, w, h)))
        {
            return false;
        }

        var room = new Room(x, y, w, h);
        room.PlanExits(random);
        rooms.Add(room);

        return true;
    }

    /// <summary>
    /// Generates a random number from a normal distribution using Box-Muller transform.
    /// </summary>
    private double NormalRandom(double mean, double variance)
    {
        //Box-Muller transform
        double u1 = 1.0 - random.NextDouble();
        double u2 = 1.0 - random.NextDouble();
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        return mean + Math.Sqrt(variance) * randStdNormal;
    }

    /// <summary>
    /// Performs Delaunay triangulation on room centers.
    /// </summary>
    private void Triangulate()
    {
        var delaunay = new Delaunay(nodes);
        delaunayEdges = delaunay.Triangulate();
    }

    /// <summary>
    /// Plans hallway connections using MST and adds random extra connections.
    /// </summary>
    private void PlanHallways()
    {
        var minimumSpenningTree = new MinimumSpanningTree(delaunayEdges, nodes);
        hallwayEdges = minimumSpenningTree.MST();

        //DEBUG: Create snapshot of MST
        DebugSnapshotManager.Instance.SetCategory("minimum spanning tree");
        var hallwayLines = new List<(double x1, double y1, double x2, double y2)>();
        var hallwayPoints = new List<(double x, double y)>();
        //Add all nodes (room centers) to the points for visualization
        hallwayPoints.AddRange(nodes);
        foreach (var edge in hallwayEdges)
        {
            hallwayLines.Add(edge.GetLine());
        }
        DebugSnapshotManager.Instance.AddSnapshot(hallwayPoints, hallwayLines);

        //Add more hallways randomly to create a few loops
        foreach (var edge in delaunayEdges)
        {
            if (hallwayEdges.Contains(edge))
                continue;


            //Fixed 30% chance for now
            if (random.NextDouble() < .3)
                hallwayEdges.Add(edge);
        }

        //DEBUG: Create snapshot of added hallways
        DebugSnapshotManager.Instance.SetCategory("minimum spanning tree + additional hallways");
        hallwayLines = new List<(double x1, double y1, double x2, double y2)>();
        hallwayPoints = new List<(double x, double y)>();
        //Add all nodes (room centers) to the points for visualization
        hallwayPoints.AddRange(nodes);
        foreach (var edge in hallwayEdges)
        {
            hallwayLines.Add(edge.GetLine());
        }
        DebugSnapshotManager.Instance.AddSnapshot(hallwayPoints, hallwayLines);
    }

    /// <summary>
    /// Sets movement costs on the grid.
    /// </summary>
    private void SetGridCosts()
    {
        foreach (var room in rooms)
        {
            grid.AddRoomCosts(room);
        }
    }

    /// <summary>
    /// Creates hallways between rooms using pathfinding.
    /// </summary>
    private void CreateHallways()
    {
        var pathFinder = new PathFinder(grid, allowDiagonals);

        foreach (var edge in hallwayEdges)
        {
            int x1 = Convert.ToInt32(edge.A.x);
            int y1 = Convert.ToInt32(edge.A.y);
            int x2 = Convert.ToInt32(edge.B.x);
            int y2 = Convert.ToInt32(edge.B.y);

            pathFinder.FindPath((x1, y1), (x2, y2));

            //grid.Print();
            //Console.WriteLine();
        }
    }

    /// <summary>
    /// Returns the dungeon as a boolean array where true represents walkable tiles.
    /// </summary>
    public bool[,] GetBooleanMap()
    {
        return grid.GetBooleanMap();
    }
}