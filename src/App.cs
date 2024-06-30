using Raylib_cs;

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
		ElevatorGenerator.Test();
	}

	private static void Update()
	{

	}

	private static void Render()
	{
		Raylib.BeginDrawing();
		Raylib.ClearBackground(Color.Magenta);

		Raylib.DrawText("elevator", 10, 10, 30, Color.White);

		Raylib.EndDrawing();
	}

	private static void CleanUp()
	{
		Raylib.CloseWindow();
	}
}