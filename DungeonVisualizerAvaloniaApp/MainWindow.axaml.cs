using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using DungeonGeneratorApp;

namespace DungeonVisualizerAvaloniaApp;

public partial class MainWindow : Window
{
    private bool[,]? currentDungeonMap;
    private bool isInitialized = false;
    private Image? dungeonImage;

    public MainWindow()
    {
        InitializeComponent();
        Loaded += Window_Loaded;
    }

    private void Window_Loaded(object? sender, RoutedEventArgs e)
    {
        isInitialized = true;
        GenerateDungeon();
    }

    private void Parameter_Changed(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        // Don't auto-generate, wait for button click
        // This just validates parameters
        if (isInitialized)
        {
            ValidateParameters();
        }
    }

    private void CellSize_Changed(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        // Redraw with new cell size if we have a dungeon
        if (isInitialized && currentDungeonMap != null)
        {
            DrawDungeon(currentDungeonMap);
        }
    }

    private void ValidateParameters()
    {
        bool isValid = true;
        string errorMessage = "";

        // Validate min/max room size
        if (MinRoomSizeSlider.Value > MaxRoomSizeSlider.Value)
        {
            isValid = false;
            errorMessage = "Min room size must be <= max room size";
        }

        // Validate seed
        if (!int.TryParse(SeedTextBox.Text, out int seed))
        {
            isValid = false;
            errorMessage = "Seed must be a valid integer";
        }

        if (isValid)
        {
            StatusText.Text = "Parameters valid. Click Generate to create dungeon.";
            StatusText.Foreground = new SolidColorBrush(Colors.Green);
            GenerateButton.IsEnabled = true;
        }
        else
        {
            StatusText.Text = $"Error: {errorMessage}";
            StatusText.Foreground = new SolidColorBrush(Colors.Red);
            GenerateButton.IsEnabled = false;
        }
    }

    private void GenerateButton_Click(object? sender, RoutedEventArgs e)
    {
        GenerateDungeon();
    }

    private void GenerateDungeon()
    {
        try
        {
            StatusText.Text = "Generating dungeon...";
            StatusText.Foreground = new SolidColorBrush(Colors.Blue);

            // Get parameters from UI
            int width = (int)WidthSlider.Value;
            int height = (int)HeightSlider.Value;
            int roomCount = (int)RoomCountSlider.Value;
            int minRoomSize = (int)MinRoomSizeSlider.Value;
            int maxRoomSize = (int)MaxRoomSizeSlider.Value;
            double roomSizeMean = RoomSizeMeanSlider.Value;
            double roomSizeVariance = RoomSizeVarianceSlider.Value;
            bool allowDiagonals = AllowDiagonalsCheckBox.IsChecked ?? false;

            if (!int.TryParse(SeedTextBox.Text, out int seed))
            {
                seed = -1;
            }

            // Create and run generator
            var generator = new DungeonGenerator(
                width,
                height,
                seed,
                allowDiagonals,
                null, // No fixed rooms for now
                roomCount,
                minRoomSize,
                maxRoomSize,
                roomSizeMean,
                roomSizeVariance
            );

            generator.Generate();

            // Get the boolean map
            currentDungeonMap = generator.GetBooleanMap();

            // Draw the dungeon
            DrawDungeon(currentDungeonMap);

            StatusText.Text = $"Dungeon generated successfully! ({width}x{height})";
            StatusText.Foreground = new SolidColorBrush(Colors.Green);
        }
        catch (Exception ex)
        {
            StatusText.Text = $"Error: {ex.Message}";
            StatusText.Foreground = new SolidColorBrush(Colors.Red);
        }
    }

    private void DrawDungeon(bool[,] map)
    {
        DungeonCanvas.Children.Clear();

        int width = map.GetLength(0);
        int height = map.GetLength(1);
        int cellSize = (int)CellSizeSlider.Value;

        // Debug output
        int trueCount = 0;
        int falseCount = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y]) trueCount++;
                else falseCount++;
            }
        }
        Console.WriteLine($"Drawing dungeon: {width}x{height}, True cells: {trueCount}, False cells: {falseCount}");

        // Set canvas size
        DungeonCanvas.Width = width * cellSize;
        DungeonCanvas.Height = height * cellSize;

        // Create a WriteableBitmap for efficient rendering
        int pixelWidth = width * cellSize;
        int pixelHeight = height * cellSize;
        
        var bitmap = new WriteableBitmap(
            new PixelSize(pixelWidth, pixelHeight),
            new Vector(96, 96),
            PixelFormat.Bgra8888,
            AlphaFormat.Opaque);

        using (var buffer = bitmap.Lock())
        {
            unsafe
            {
                uint* ptr = (uint*)buffer.Address;
                int stride = buffer.RowBytes / 4; // 4 bytes per pixel

                // Define colors (BGRA format)
                uint floorColor = 0xFFC8C8FF; // Light blue for floor (ARGB: 0xFFFFC8C8)
                uint wallColor = 0xFF282828;  // Dark gray for walls

                // Fill the pixel array
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        uint color = map[x, y] ? floorColor : wallColor;

                        // Fill the cell area (cellSize x cellSize pixels)
                        for (int px = 0; px < cellSize; px++)
                        {
                            for (int py = 0; py < cellSize; py++)
                            {
                                int pixelX = x * cellSize + px;
                                int pixelY = y * cellSize + py;
                                
                                if (pixelX < pixelWidth && pixelY < pixelHeight)
                                {
                                    ptr[pixelY * stride + pixelX] = color;
                                }
                            }
                        }
                    }
                }
            }
        }

        // Create or update the Image control
        if (dungeonImage == null)
        {
            dungeonImage = new Image();
        }

        // Always add it back since we cleared the canvas
        if (!DungeonCanvas.Children.Contains(dungeonImage))
        {
            DungeonCanvas.Children.Add(dungeonImage);
        }

        dungeonImage.Source = bitmap;
        dungeonImage.Width = pixelWidth;
        dungeonImage.Height = pixelHeight;
        Canvas.SetLeft(dungeonImage, 0);
        Canvas.SetTop(dungeonImage, 0);
    }
}
