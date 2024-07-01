using Raylib_cs;

class App
{
	public static Scene Scene;

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
		Scene = new MainMenu();
		Scene.Start();
	}

	private static void Update()
	{
		Scene.Update();
	}

	private static void Render()
	{
		Raylib.BeginDrawing();
		Raylib.ClearBackground(Color.Magenta);

		Scene.Render();

		Raylib.EndDrawing();
	}

	private static void CleanUp()
	{
		Scene.CleanUp();
		Raylib.CloseWindow();
	}


	public static void SetScene(Scene newScene)
	{
		// Unload the previous scene
		Scene.CleanUp();

		// Load and assign the new scene
		Scene = newScene;
		Scene.Start();
	}
}