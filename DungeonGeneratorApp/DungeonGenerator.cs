using System;

namespace DungeonGeneratorApp;

public class DungeonGenerator
{
    private int width;
    private int height;
    private int seed;

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


    /*
                        Syötteet:
                        - Kartan leveys, esim. 100 solua
                        - Kartan korkeus, esim. 150 solua
                        - Siemenluku (seed) satunnaisgenerointia varten
                        Vapaavalinnaiset syötteet:
                        - Lista huoneita, jotka aina generoidaan. Jokainen listassa oleva huone on tuple muotoa (x, y, leveys, korkeus). Lista voi olla tyhjä. Kartan ulkopuolelle (edes osittain) sijoittuvat huoneet johtavat virhetulosteeseen.
                            - x on huoneen vasemman laidan koordinaatti
                            - y on huoneen ylälaidan koordinaatti
                            - leveys on huoneen leveys soluina
                            - korkeus on huoneen korkeus soluina
                        - Huoneiden määrä, oletusarvo määräytyy lukuna suhteessa kartan kokoon (esim. floor(0.1*(leveys+korkeus)))
                        - Huoneen sivun pituuden minimi, oletusarvo 1 solua
                        - Huoneen sivun pituuden maksimi, oletusarvo 9 solua
                        - Huoneiden koon määrittävän normaalijakauman odotusarvo, oletusarvona 4 (solua)
                        - Huoneiden koon määrittävän normaalijakauman varianssi, oletusarvona 1.6
                    */

    public DungeonGenerator(
        int width,
        int height,
        int seed,
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
    public void Generate()
    {
        Console.WriteLine("Generation started");

        //DEBUG: Set dungeon bounds for debug snapshot manager
        DebugSnapshotManager.Instance.SetDungeonBounds(width, height);

        rooms = new List<Room>();
        grid = new MapGrid(width, height);
        nodes = new List<(double, double)>();


        if (seed == -1)
            //Random seed if seed = -1
            random = new Random();
        else
            random = new Random(seed);

        PlaceRooms();
        Triangulate();

        grid.Print();
    }

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
            nodes.Add(r.GetCenter());
        }

        //DEBUG: Add a single snapshot after all rooms have been placed
        DebugSnapshotManager.Instance.SetCategory("room placement");
        DebugSnapshotManager.Instance.AddSnapshot(nodes, new List<(double, double, double, double)>());
    }

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

        rooms.Add(new Room(x, y, w, h));

        return true;
    }

    //Maybe move this elsewhere?
    private double NormalRandom(double mean, double variance)
    {
        //Box-Muller transform
        double u1 = 1.0 - random.NextDouble();
        double u2 = 1.0 - random.NextDouble();
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        return mean + Math.Sqrt(variance) * randStdNormal;
    }

    private void Triangulate()
    {
        var delaunay = new Delaunay(nodes);
        delaunayEdges = delaunay.Triangulate();
    }
    
    

}