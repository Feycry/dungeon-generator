# dungeon-generator

Määrittelydokumentti, viikkoraportit, testausdokumentti ja toteutusdokumentti "Documentation" kansiossa.

## Prerequisites

Before running this project, you need to have .NET 8 SDK installed on your system.

### Installing .NET 8 SDK

1. **Windows**: Download and install from [Microsoft .NET Downloads](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
2. **macOS**: Use Homebrew: `brew install dotnet` or download from the official site
3. **Linux**: Follow the distribution-specific instructions on the [official .NET page](https://docs.microsoft.com/en-us/dotnet/core/install/linux)

To verify your installation, run:
```bash
dotnet --version
```

## Running the Project

1. Navigate to the project directory:
   ```bash
   cd DungeonGeneratorApp
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Build the project:
   ```bash
   dotnet build
   ```

4. Run the application:
   ```bash
   dotnet run
   ```

You must create the images folder manually in order to see debug snapshots of the generation process.

The program can also be run from inside an IDE like VSCode.

## Running the Visual UI

The project also includes a visual UI which I recommend using.

1. Navigate to the app directory:
   ```bash
   cd DungeonVisualizerAvaloniaApp
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Build the project:
   ```bash
   dotnet build
   ```

4. Run the visual application:
   ```bash
   dotnet run
   ```

This provides an interactive GUI where you can visualize the dungeon generation process and use sliders to adjust parameters.

## Running Tests

Running unit tests from the main program is not currently implemented.

To run the unit tests:

1. Navigate to the test project directory:
   ```bash
   cd DungeonGeneratorTest
   ```

2. Run tests:
   ```bash
   dotnet test
   ```