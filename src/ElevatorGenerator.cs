using System.Numerics;
using Raylib_cs;
using TinyDialogsNet;

class ElevatorGenerator
{
	public static void Update()
	{
		if (Raylib.IsKeyPressed(KeyboardKey.O))
		{
			// Get the path
			string path = OpenFileDialogue();
			if (path == null) return;

			// Make the image
			Generate(path);
		}
	}

	public static void Render()
	{

	}

	private static string OpenFileDialogue()
	{
		// Specify the files that we want to accept,
		// and also get the path to the default picture
		// folder on the persons computer
		FileFilter filter = new FileFilter("Images", ["*.png*", "*.jpg", "*.jpeg"]);
		string picturesFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

		// TODO: Don't use var
		// Get the files
		var (cancelled, paths) = TinyDialogs.OpenFileDialog("Elevator image", picturesFolder, false, filter);
		if (cancelled || paths == null) return null;

		// Stupid enumerator thingy bruh tf even is this
		using (IEnumerator<string> enumerator = paths.GetEnumerator())
		{
			if (enumerator.MoveNext()) return enumerator.Current;
		}

		// No path or something idk
		return null;
	}

	private static void Generate(string overlayPath)
	{
		// Load in the elevator image, and the image
		// that we want to put on the elevator
		Texture2D elevatorTexture = Raylib.LoadTexture("./assets/game/round_elevator_sheet_3.png");
		Texture2D overlayTexture = Raylib.LoadTexture(overlayPath);

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