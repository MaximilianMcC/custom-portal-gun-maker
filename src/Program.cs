class Program
{
	public static string SteamPath;

	public static void Main(string[] args)
	{
		if (args.Length < 1)
		{
			// Tanty
			Console.WriteLine("Please provide the path to your steam folder as the first argument.\n");
			Console.WriteLine(@"Example: C:\Program Files (x86)\Steam\steamapps\common");
			Console.WriteLine(@"Example: D:\games\steam\steamapps\common");

			// Close
			Console.ReadKey();
			return;
		}

		// Assign the steam path then run
		SteamPath = args[0];
		App.Run();
	}
}