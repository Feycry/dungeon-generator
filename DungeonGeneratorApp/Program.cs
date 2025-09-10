using System;

class Program
{
	static void Main(string[] args)
	{
		string[] options = { "Option 1", "Option 2", "Option 3", "Option 4" };
		while (true)
		{
			Console.WriteLine("Pick a number from the list below:");
			for (int i = 0; i < options.Length; i++)
			{
				Console.WriteLine($"{i + 1}. {options[i]}");
			}
			Console.Write("Enter your choice (1-4): ");
			string input = Console.ReadLine();
			if (int.TryParse(input, out int choice) && choice >= 1 && choice <= options.Length)
			{
				Console.WriteLine($"You picked: {options[choice - 1]}\n");
			}
			else
			{
				Console.WriteLine("Invalid choice. Please try again.\n");
			}
		}
	}
}
