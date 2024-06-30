using System.Diagnostics;
using System.Numerics;
using Raylib_cs;
using TinyDialogsNet;

class ElevatorGenerator
{
	private static string status = "Press ctrl+o to select the file";

	public static void Update()
	{
		// Check for if they do ctrl+o
		if (Raylib.IsKeyDown(KeyboardKey.LeftControl) && Raylib.IsKeyDown(KeyboardKey.O))
		{
			// Get the path
			string path = OpenFileDialogue();
			if (path == null) return;

			// Make the image
			status = "Generating image rn...";
			Generate(path);
			status = "Done!";
		}
	}

	public static void Render()
	{
		Raylib.DrawText(status, 10, 10, 30, Color.White);
	}

	private static string OpenFileDialogue()
	{
		// Specify the files that we want to accept,
		// and also get the path to the default picture
		// folder on the persons computer
		FileFilter filter = new FileFilter("png image", ["*.png"]);
		string picturesFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "/";

		// TODO: Don't use var (stink)
		// Get the files
		var (cancelled, paths) = TinyDialogs.OpenFileDialog("Elevator image", picturesFolder, false, filter);
		if (cancelled || paths == null) return null;

		// Stupid enumerator thingy bruh tf even is this
		// (tf wrong with normal string fr)
		using (IEnumerator<string> enumerator = paths.GetEnumerator())
		{
			if (enumerator.MoveNext()) return enumerator.Current;
		}

		// No path or something idk
		return null;
	}

	private static void Generate(string overlayPath)
	{
		// Generate and save the image to a temporary
		// file somewhere
		string texturePath = GenerateImage(overlayPath);

		// Turn the image into a vtf file
		texturePath = ConvertToVtf(texturePath);

		// Get the folders and whatnot
		string steamGamesFolderPath = Program.SteamPath;
		string portalFolder = Path.Join(steamGamesFolderPath, @"Portal\portal\");

		// Make the file structure for the actual thing
		string fullPath = Path.Join(portalFolder, @"custom\customElevator\materials\models\props\");
		if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);

		// Put the vtf file in it
		File.Move(texturePath, Path.Join(fullPath, "round_elevator_sheet_3.vtf"), true);
	}

	private static string GenerateImage(string overlayPath)
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

		// Generate a temporary file path for the image
		// to be stored before its converted into vtf
		string filePath = Path.GetTempFileName();
		filePath = filePath.Replace(".tmp", ".png");

		// Turn the render texture into an
		// image, flip it, then save it 
		//? Flip it because OpenGL draws upside down
		Image finalTexture = Raylib.LoadImageFromTexture(canvas.Texture);
		Raylib.ImageFlipVertical(ref finalTexture);
		Raylib.ExportImage(finalTexture, filePath);

		// Unload everything
		Raylib.UnloadImage(finalTexture);
		Raylib.UnloadTexture(elevatorTexture);
		Raylib.UnloadTexture(overlayTexture);

		// Return the path to the file
		return filePath;
	}

	private static string ConvertToVtf(string filePath)
	{
		//! Maybe don't do this
		string pathToVtfCmd = @"./lib/VTFCmd.exe";

		// Make the process
		ProcessStartInfo processStartInfo = new ProcessStartInfo()
		{
			FileName = pathToVtfCmd,
			Arguments = $"-file {filePath} -resize -exportFormat \"vtf\"",
			CreateNoWindow = true,
			UseShellExecute = false
		};

		// Run the process to convert the file
		using (Process process = new Process())
		{
			// Set the process, then start it
			process.StartInfo = processStartInfo;
			process.Start();

			// Wait for it to fully run (convert the file)
			process.WaitForExit();
		}

		// Once the file has been generated, we
		// can delete the old png input file
		File.Delete(filePath);

		// Rename the file to have the correct name
		string newFileName = Path.Join(Path.GetDirectoryName(filePath), "round_elevator_sheet_3.vtf");
		filePath = filePath.Replace(".png", ".vtf");
		File.Move(filePath, newFileName, true);
		
		// Give back the new path to the vtf image
		return newFileName;
	}
}