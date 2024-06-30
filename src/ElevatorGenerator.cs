using System.Numerics;
using Raylib_cs;

class ElevatorGenerator
{
	public static void Update()
	{

	}

	public static void Render()
	{

	}

	public static void Test()
	{
		// Load in the elevator image, and the image
		// that we want to put on the elevator
		Texture2D elevatorTexture = Raylib.LoadTexture("./assets/game/round_elevator_sheet_3.png");
		Texture2D overlayTexture = Raylib.LoadTexture("./assets/test.png");

		// Make a new render texture so that we can
		// draw/edit the elevator image
		RenderTexture2D canvas = Raylib.LoadRenderTexture(elevatorTexture.Width, elevatorTexture.Height);
		
		// Draw the overlay image onto the canvas
		//! These values kinda need to be hardcoded
		Raylib.BeginTextureMode(canvas);
		Raylib.DrawTexture(elevatorTexture, 0, 0, Color.White);
		Raylib.DrawTexturePro(
			overlayTexture,
			new Rectangle(0, 0, overlayTexture.Width, overlayTexture.Height),
			new Rectangle(234f, 795f, 676f, 180f),
			Vector2.Zero, 0f, Color.White
		);
		Raylib.EndTextureMode();

		// Turn the render texture into an
		// image, flip it, then save it 
		//? Flip it because OpenGL draws upside down
		Image finalTexture = Raylib.LoadImageFromTexture(canvas.Texture);
		Raylib.ImageFlipVertical(ref finalTexture);
		Raylib.ExportImage(finalTexture, "test.png");

		// Unload everything
		Raylib.UnloadImage(finalTexture);
		Raylib.UnloadTexture(elevatorTexture);
		Raylib.UnloadTexture(overlayTexture);
	}
}