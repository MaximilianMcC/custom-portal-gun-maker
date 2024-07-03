using System.Numerics;
using Raylib_cs;

class MainMenu : Scene
{
	private Button portal1GunButton;
	private Button elevatorButton;
	private Button portal2GunButton;

	public override void Start()
	{
		// Menu button spacing calculations
		Vector2 buttonSizes = new Vector2(250, 300);
		int buttonCount = 3;
		float totalWidth = buttonSizes.X * buttonCount;
		float xOffset = (Raylib.GetScreenWidth() - totalWidth) / 2;
		float y = (Raylib.GetScreenHeight() - buttonSizes.Y) / 3;

		// Make the menu buttons
		portal1GunButton = new Button("./assets/ui/button-p1-gun.png", new Rectangle(xOffset, y, buttonSizes), (() => App.SetScene(new P1PortalgunGenerator())), true);
		elevatorButton = new Button("./assets/ui/button-p1-elevator.png", new Rectangle(xOffset + buttonSizes.X, y, buttonSizes), (() => App.SetScene(new ElevatorGenerator())), true);
		portal2GunButton = new Button("./assets/ui/button-p2-gun.png", new Rectangle(xOffset + (buttonSizes.X * 2), y, buttonSizes), (() => App.SetScene(new P1PortalgunGenerator())), true);

		// TODO: Make other generators
		// portal1GunButton.Disabled = true;
		portal2GunButton.Disabled = true;
	}

	public override void Update()
	{
		// Update the menu buttons
		portal1GunButton.Update();
		elevatorButton.Update();
		portal2GunButton.Update();
	}

	public override void Render()
	{
		Raylib.ClearBackground(Color.RayWhite);

		// Render the menu buttons
		portal1GunButton.Render();
		elevatorButton.Render();
		portal2GunButton.Render();
	}

	public override void CleanUp()
	{
		// Clean up all the menu buttons
		portal1GunButton.CleanUp();
		elevatorButton.CleanUp();
		portal2GunButton.CleanUp();
	}
}