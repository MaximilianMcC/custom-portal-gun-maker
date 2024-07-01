using System.Diagnostics;
using TinyDialogsNet;

class Scene
{
	public virtual void Start() {}
	public virtual void Update() {}
	public virtual void Render() {}
	public virtual void CleanUp() {}

	protected static string OpenFileDialogueGetImage(string name)
	{
		// Specify the files that we want to accept,
		// and also get the path to the default picture
		// folder on the persons computer
		FileFilter filter = new FileFilter("png image", ["*.png"]);
		string picturesFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "/";

		// TODO: Don't use var (stink)
		// Get the files
		var (cancelled, paths) = TinyDialogs.OpenFileDialog(name, picturesFolder, false, filter);
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

	protected static string ConvertToVtf(string filePath, string fileName)
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
		string newFileName = Path.Join(Path.GetDirectoryName(filePath), fileName);
		filePath = filePath.Replace(".png", ".vtf");
		File.Move(filePath, newFileName, true);
		
		// Give back the new path to the vtf image
		return newFileName;
	}
}