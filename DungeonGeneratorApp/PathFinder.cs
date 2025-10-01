using System;

namespace DungeonGeneratorApp;

public class PathFinder
{
    private MapGrid grid;

    public PathFinder(MapGrid grid)
    {
        this.grid = grid;
    }

    //A* pathfinding
    public void FindPath(Room start, Room end)
    {
        Tile startTile = new Tile(start.X, start.Y);
        Tile endTile = new Tile(end.X, end.Y);

        List<Tile> openSet = new List<Tile>();
        HashSet<Tile> closedSet = new HashSet<Tile>();

        openSet.Add(startTile);

        while (openSet.Count > 0)
        {
            Tile currentTile = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentTile.FCost || (openSet[i].FCost == currentTile.FCost && openSet[i].HCost < currentTile.HCost))
                {
                    currentTile = openSet[i];
                }
            }

            openSet.Remove(currentTile);
            closedSet.Add(currentTile);

            if (currentTile == endTile)
            {
                return;
            }


        }
    }
}