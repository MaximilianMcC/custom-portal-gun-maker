using Raylib_cs;

class MainMenu : Scene
{
	private Button testButton;

	public override void Start()
	{
		testButton = new Button("./assets/test.png", new Rectangle(20, 20, 100, 100), (() => Console.WriteLine("clicked")), true);
	}

	public override void Update()
	{
		testButton.Update();
	}

	public override void Render()
	{
		testButton.Render();
	}

	public override void CleanUp()
	{
		testButton.CleanUp();
	}
}