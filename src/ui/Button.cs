using System.Numerics;
using Raylib_cs;

class Button
{
	public Rectangle Rectangle;
	public Action OnClick;
	public bool Suicidal;

	private Texture2D image;
	private bool scheduledForDeath;
	private bool previouslyHovering;

	public Button(string imagePath, Rectangle rectangle, Action onClick, bool suicidal)
	{
		// Load in the image
		image = Raylib.LoadTexture(imagePath);

		// Assign the rectangle and click event. Also
		// determine if its suicidal (cleans up after clicked)
		Rectangle = rectangle;
		OnClick = onClick;
		Suicidal = suicidal;
	}

	public void Update()
	{
		// Check for if the mouse is hovering over
		Vector2 mousePosition = Raylib.GetMousePosition();
		bool currentlyHovering = Raylib.CheckCollisionPointRec(mousePosition, Rectangle);

		// Toggle the hand click hover cursor thingy
		if (currentlyHovering && !previouslyHovering) Raylib.SetMouseCursor(MouseCursor.PointingHand);
		if (!currentlyHovering && previouslyHovering) Raylib.SetMouseCursor(MouseCursor.Default);
		previouslyHovering = currentlyHovering;

		// Exit early if we're not on the button
		// (nothing for us to do)
		if (currentlyHovering == false) return;

		// Check for if the user tries to click on the button
		if (Raylib.IsMouseButtonPressed(MouseButton.Left)) Click();
	}

	public void Render()
	{
		// If we're scheduled for death then don't render anything
		// because the image has already been unloaded
		if (scheduledForDeath) return;

		// Draw the texture
		Raylib.DrawTexturePro(
			image,
			new Rectangle(0, 0, image.Width, image.Height),
			Rectangle,
			Vector2.Zero, 0f, Color.White
		);
	}

	public void CleanUp()
	{
		// Unload the image
		Raylib.UnloadTexture(image);
	}

	// Click the button
	public void Click()
	{
		// First, check for if we are suicidal. If we are
		// then run the clean up method then run the action
		if (Suicidal)
		{
			scheduledForDeath = true;
			CleanUp();
		}

		// Run the method
		OnClick.Invoke();
	}
}