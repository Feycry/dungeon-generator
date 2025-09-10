using System;

class Program
{
	static void Main(string[] args)
	{
		string[] options = { "Generate dungeon", "Run unit tests", "Exit" };
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
		Console.WriteLine("[Dungeon generation logic goes here]\n");
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
}
