using System;

namespace DungeonGeneratorApp;

public class PathFinder
{
    private const int straightCost = 10;
    //private const int diagonalCost = 14;
    private int diagonalCost = 14;
    private MapGrid grid;
    private bool allowDiagonals;

    public PathFinder(MapGrid grid, bool allowDiagonals = true)
    {
        this.grid = grid;
        this.allowDiagonals = allowDiagonals;

        diagonalCost = 500000; //Way higher than 10, but nowhere near the integer limit
        }

    //A* pathfinding
    public void FindPath((int x, int y) start, (int x, int y) end)
    {
        Tile startTile = grid.GetTile(start.x, start.y);
        Tile endTile = grid.GetTile(end.x, end.y);

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
                RetracePath(startTile, endTile);
                return;
            }

            foreach (var neighbour in grid.GetNeighbours(currentTile, allowDiagonals))
            {
                if (closedSet.Contains(neighbour))
                    continue;

                int newMovementCost = currentTile.GCost + GetDistance(currentTile, neighbour) + neighbour.Cost;

                if (newMovementCost < neighbour.GCost || !openSet.Contains(neighbour))
                {
                    neighbour.GCost = newMovementCost;
                    neighbour.HCost = GetDistance(neighbour, endTile);
                    neighbour.Parent = currentTile;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
        
        Console.WriteLine("Couldn't find path!");
        Console.WriteLine();
        return;
    }

    private void RetracePath(Tile start, Tile end)
    {
        List<Tile> path = new List<Tile>();
        Tile currentTile = end;

        while (currentTile != start)
        {
            path.Add(currentTile);
            currentTile = currentTile.Parent;
        }

        //path.Reverse();

        grid.AddPath(path);
    }

    private int GetDistance(Tile a, Tile b)
    {
        int distX = Math.Abs(a.X - b.X);
        int distY = Math.Abs(a.Y - b.Y);

        if (distX > distY)
        {
            return diagonalCost * distY + straightCost * (distX - distY);
        }

        return diagonalCost * distX + straightCost * (distY - distX);
    }
}