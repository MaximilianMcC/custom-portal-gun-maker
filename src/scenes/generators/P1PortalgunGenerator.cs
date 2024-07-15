using System.Numerics;
using Raylib_cs;

class P1PortalgunGenerator : Scene
{
	private Texture2D portalGunTexture;
	private LayerHandler layerHandler;

	public override void Start()
	{
		// Make the layers setup stuff
		layerHandler = new LayerHandler();

		// Load in the portalgun texture
		portalGunTexture = Raylib.LoadTexture("./assets/game/p1gun/v_portalgun.png");

	}

	public override void Update()
	{
		// Debug toggle
		// TODO: Remove
		if (Raylib.IsKeyPressed(KeyboardKey.F3)) layerHandler.Debug = !layerHandler.Debug;

		layerHandler.GetSelectedLayer();
		layerHandler.TransformSelectedLayer();
		layerHandler.DeleteLayers();


		// If we press ctrl+n make a new layer
		if (Raylib.IsKeyDown(KeyboardKey.LeftControl) && Raylib.IsKeyDown(KeyboardKey.N))
		{
			// Get the path
			string path = OpenFileDialogueGetImage("layer");
			if (path == null) return;

			// Make the layer, then add
			// it to the list of layers
			Layer layer = new Layer()
			{
				Texture = Raylib.LoadTexture(path),
				// Position = Vector2.Zero,
				Position = new Vector2(25f),
				Rotation = 0f,
				Scale = Vector2.One,
				Index = layerHandler.Layers.Count
			};
			layerHandler.Layers.Add(layer);

			// Select the new layer
			layerHandler.SelectedIndex = layer.Index;
		}

		// If we press ctrl+e then export
		if (Raylib.IsKeyDown(KeyboardKey.LeftControl) && Raylib.IsKeyDown(KeyboardKey.E))
		{
			Generate();
		}
	}



	// TODO: Have the 3d preview on the right or something, then the editor on the left
	public override void Render()
	{
		// Render the portal gun texture and
		// all of the layers on top of it 
		Raylib.DrawTexture(portalGunTexture, 0, 0, Color.White);
		layerHandler.DrawLayersWithControls();
	}


	private void Generate()
	{
		// Generate the texture and save it to a temp
		// directory somewhere
		string texturePath = GenerateTexture();

		// Turn the image into a vtf file
		string textureFileName = "v_portalgun.vtf";
		texturePath = ConvertToVtf(texturePath, textureFileName);

		// Get the folders and whatnot
		string steamGamesFolderPath = Program.SteamPath;
		string portalFolder = Path.Join(steamGamesFolderPath, @"Portal\portal\");

		// Make the file structure for the actual thing
		string fullPath = Path.Join(portalFolder, @"custom\customPortalgun\materials\models\weapons\v_models\v_portalgun");
		if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);

		// Put the vtf file in it
		File.Move(texturePath, Path.Join(fullPath, textureFileName), true);
	}

	private string GenerateTexture()
	{
		// Make a new render texture canvas to
		// draw the textures onto
		RenderTexture2D canvas = Raylib.LoadRenderTexture(portalGunTexture.Width, portalGunTexture.Height);

		// Draw the design onto the portalgun
		Raylib.BeginTextureMode(canvas);
		Raylib.DrawTexture(portalGunTexture, 0, 0, Color.White);
		layerHandler.DrawLayers();
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

		// Return the path to the file
		return filePath;
	}


	public override void CleanUp()
	{
		// Unload all the textures
		Raylib.UnloadTexture(portalGunTexture);

		layerHandler.CleanUp();
	}
}