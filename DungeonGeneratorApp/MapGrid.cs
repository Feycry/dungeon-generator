

public class MapGrid
{
    int width;
    int height;
    int[,] grid;

    public MapGrid(int width, int height)
    {
        this.width = width;
        this.height = height;

        grid = new int[width, height];
    }
}