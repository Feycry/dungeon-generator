using System;

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
        this.roomCount = roomCount ?? (int)Math.Floor(0.1 * (width + height));
        this.minRoomSideSize = minRoomSideSize;
        this.maxRoomSideSize = maxRoomSideSize;
        this.roomSideSizeMean = roomSideSizeMean;
        this.roomSideSizeVariance = roomSideSizeVariance;
    }
    public void Generate()
    {
        Console.WriteLine("Generation started");

        PlaceRooms();
        Triangulate();
    }

    void PlaceRooms()
    {

    }

    void Triangulate()
    {

    }
    
    

}