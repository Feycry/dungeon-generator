using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DungeonGeneratorApp;

namespace DungeonVisualizerApp
{
    public partial class MainWindow : Window
    {
        private bool[,]? currentDungeonMap;
        private bool isInitialized = false;
        private Image? dungeonImage;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            isInitialized = true;
            GenerateDungeon();
        }

        private void Parameter_Changed(object sender, RoutedEventArgs e)
        {
            // Don't auto-generate, wait for button click
            // This just validates parameters
            if (isInitialized)
            {
                ValidateParameters();
            }
        }

        private void CellSize_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
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

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
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
                MessageBox.Show($"Failed to generate dungeon:\n{ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
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
            WriteableBitmap bitmap = new WriteableBitmap(
                pixelWidth,
                pixelHeight,
                96, 96,
                PixelFormats.Bgr32,
                null);

            // Define colors (BGR format for Bgr32)
            byte[] floorColor = { 200, 200, 255, 0 };  // Light blue for floor (B, G, R, padding)
            byte[] wallColor = { 40, 40, 40, 0 };      // Dark gray for walls

            // Create pixel array
            int stride = pixelWidth * 4; // 4 bytes per pixel (Bgr32)
            byte[] pixels = new byte[pixelHeight * stride];

            // Fill the pixel array
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    byte[] color = map[x, y] ? floorColor : wallColor;
                    
                    // Fill the cell area (cellSize x cellSize pixels)
                    for (int px = 0; px < cellSize; px++)
                    {
                        for (int py = 0; py < cellSize; py++)
                        {
                            int pixelX = x * cellSize + px;
                            int pixelY = y * cellSize + py;
                            int index = pixelY * stride + pixelX * 4;

                            if (index + 3 < pixels.Length)
                            {
                                pixels[index] = color[0];     // Blue
                                pixels[index + 1] = color[1]; // Green
                                pixels[index + 2] = color[2]; // Red
                                pixels[index + 3] = 0;        // Padding
                            }
                        }
                    }
                }
            }

            // Write pixels to bitmap
            bitmap.WritePixels(
                new Int32Rect(0, 0, pixelWidth, pixelHeight),
                pixels,
                stride,
                0);

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
}
