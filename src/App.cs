using Raylib_cs;
using TinyDialogsNet;

class App
{
	public static void Run()
	{
		Raylib.SetTraceLogLevel(TraceLogLevel.Warning);
		Raylib.InitWindow(800, 600, "Custom portal gun maker");

		Start();
		while (!Raylib.WindowShouldClose())
		{
			Update();
			Render();
		}
		CleanUp();
	}

	private static void Start()
	{

	}

	private static void Update()
	{
		ElevatorGenerator.Update();
	}

	private static void Render()
	{
		Raylib.BeginDrawing();
		Raylib.ClearBackground(Color.Magenta);

		ElevatorGenerator.Render();

		Raylib.EndDrawing();
	}

	private static void CleanUp()
	{
		Raylib.CloseWindow();
	}
}