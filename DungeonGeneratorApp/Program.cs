using System;

class Program
{
	static void Main(string[] args)
	{
		string[] options = { "Generate dungeon", "Run unit tests", "Draw generation steps (Windows only)", "Exit" };
		bool running = true;
		while (running)
		{
			Console.WriteLine("Pick a number from the list below:");
			for (int i = 0; i < options.Length; i++)
			{
				Console.WriteLine($"{i + 1}. {options[i]}");
			}
			Console.Write($"Enter your choice (1-{options.Length}): ");
			string? input = Console.ReadLine();
			if (!string.IsNullOrEmpty(input) && int.TryParse(input, out int choice) && choice >= 1 && choice <= options.Length)
			{
				switch (choice)
				{
					case 1:
						GenerateDungeon();
						break;
					case 2:
						RunUnitTests();
						break;
					case 3:
						RunDrawingTest();
						break;
					case 4:
						running = false;
						Console.WriteLine("Exiting application. Goodbye!");
						break;
				}
			}
			else
			{
				Console.WriteLine("Invalid choice. Please try again.\n");
			}
		}
	}

	static void GenerateDungeon()
	{
		try
		{
			// Clear debug snapshots before generation
			DebugSnapshotManager.Instance.ClearAllSnapshots(); // clear all categories
			DebugSnapshotManager.Instance.SetCategory("Triangulation"); // or whatever default you want

			var generator = new DungeonGenerator(
				30,
				30,
				-1,

				new List<(int, int, int, int)> {
					(4, 4, 4, 4)
				}
			);

			generator.Generate();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Generation failed: {ex.Message}\n");
		}

	}

	static void RunUnitTests()
	{
		string[] tests = { "test1", "test2", "test3" };
		Console.WriteLine("Select a unit test to run:");
		for (int i = 0; i < tests.Length; i++)
		{
			Console.WriteLine($"{i + 1}. {tests[i]}");
		}
		Console.Write("Enter your choice (1-3): ");
		string? input = Console.ReadLine();
		if (!string.IsNullOrEmpty(input) && int.TryParse(input, out int testChoice) && testChoice >= 1 && testChoice <= tests.Length)
		{
			switch (testChoice)
			{
				case 1:
					Test1();
					break;
				case 2:
					Test2();
					break;
				case 3:
					Test3();
					break;
			}
		}
		else
		{
			Console.WriteLine("Invalid test choice. Returning to main menu.\n");
		}
	}

	static void Test1()
	{
		Console.WriteLine("Dummy Test1 executed.\n");
	}

	static void Test2()
	{
		Console.WriteLine("Dummy Test2 executed.\n");
	}

	static void Test3()
	{
		Console.WriteLine("Dummy Test3 executed.\n");
	}

	static void RunDrawingTest()
	{
		try
		{
			// Draw a series of images for each snapshot
			var snapshots = DebugSnapshotManager.Instance.GetSnapshots();
			if (snapshots.Count == 0)
			{
				Console.WriteLine("No debug snapshots found. Please generate a dungeon first.\n");
				return;
			}

			int index = 0;
			foreach (var snapshot in snapshots)
			{
				string fileName = $"../../../../images/step_{index:D3}.png";
				Visualize.DrawGraph(snapshot.Points, snapshot.Lines, fileName, snapshot.Category, index);
				Console.WriteLine($"Saved snapshot {index} as {fileName}");
				index++;
			}
			Console.WriteLine($"{index} snapshot images created.\n");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Drawing test failed: {ex.Message}\n");
		}
	}
}
