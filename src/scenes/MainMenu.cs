using Raylib_cs;

class MainMenu : Scene
{
	public override void Update()
	{
		// Check for if we're doing a shortcut
		if (!Raylib.IsKeyDown(KeyboardKey.LeftControl)) return;

		if (Raylib.IsKeyDown(KeyboardKey.E)) App.SetScene(new ElevatorGenerator());
		if (Raylib.IsKeyDown(KeyboardKey.One)) App.SetScene(new Portal1PortalGunGenerator());
	}

	public override void Render()
	{
		Raylib.DrawText("main menu\n\npress one to go\n\n\nctrl+e\televator\n\nctrl+1\tportal 1 portal gun\n\nctrl+2\tportaal 2 portal gun", 10, 10, 30, Color.White);
	}
}