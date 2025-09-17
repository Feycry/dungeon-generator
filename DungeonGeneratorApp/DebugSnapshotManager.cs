using System;
using System.Collections.Generic;

public class DebugSnapshotManager
{
    private static DebugSnapshotManager? _instance;
    public static DebugSnapshotManager Instance => _instance ??= new DebugSnapshotManager();

    private string currentCategory = "default";
    private readonly Dictionary<string, List<DebugSnapshot>> snapshotsByCategory = new();

    // Dungeon bounds
    private double dungeonWidth = 0;
    private double dungeonHeight = 0;

    private DebugSnapshotManager() { }

    // Set the dungeon bounds (should be called before adding snapshots)
    public void SetDungeonBounds(double width, double height)
    {
        dungeonWidth = width;
        dungeonHeight = height;
    }

    public (double width, double height) GetDungeonBounds()
    {
        return (dungeonWidth, dungeonHeight);
    }

    // Set the current category for subsequent snapshots
    public void SetCategory(string category)
    {
        currentCategory = category;
        if (!snapshotsByCategory.ContainsKey(category))
            snapshotsByCategory[category] = new List<DebugSnapshot>();
    }

    // Add a snapshot: points for rooms, lines for edges
    public void AddSnapshot(List<(double x, double y)> points, List<(double x1, double y1, double x2, double y2)> lines)
    {
        var snapshot = new DebugSnapshot(points, lines, currentCategory);
        if (!snapshotsByCategory.ContainsKey(currentCategory))
            snapshotsByCategory[currentCategory] = new List<DebugSnapshot>();
        snapshotsByCategory[currentCategory].Add(snapshot);
    }

    // Retrieve snapshots for the current category
    public List<DebugSnapshot> GetSnapshots(string? category = null)
    {
        var cat = category ?? currentCategory;
        return snapshotsByCategory.TryGetValue(cat, out var list) ? list : new List<DebugSnapshot>();
    }

    // Retrieve all snapshots from all categories in chronological order
    public List<DebugSnapshot> GetAllSnapshots()
    {
        var allSnapshots = new List<DebugSnapshot>();
        foreach (var categorySnapshots in snapshotsByCategory.Values)
        {
            allSnapshots.AddRange(categorySnapshots);
        }
        return allSnapshots;
    }

    // Clear snapshots for the specified or current category
    public void ClearSnapshots(string? category = null)
    {
        var cat = category ?? currentCategory;
        if (snapshotsByCategory.ContainsKey(cat))
            snapshotsByCategory[cat].Clear();
    }

    // Clear all snapshots from all categories
    public void ClearAllSnapshots()
    {
        foreach (var list in snapshotsByCategory.Values)
        {
            list.Clear();
        }
    }
}

public class DebugSnapshot
{
    public List<(double x, double y)> Points { get; }
    public List<(double x1, double y1, double x2, double y2)> Lines { get; }
    public string Category { get; }

    public DebugSnapshot(List<(double x, double y)> points, List<(double x1, double y1, double x2, double y2)> lines, string category)
    {
        Points = new List<(double x, double y)>(points);
        Lines = new List<(double x1, double y1, double x2, double y2)>(lines);
        Category = category;
    }
}