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
		layerHandler.Debug = true;

		// Load in the portalgun texture
		portalGunTexture = Raylib.LoadTexture("./assets/game/p1gun/v_portalgun.png");
	}

	public override void Update()
	{
		
		layerHandler.GetSelectedLayer();

		layerHandler.DragSelectedLayer();
		layerHandler.ResizeSelectedLayer();
		layerHandler.RotateSelectedLayer();


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
	}



	// TODO: Have the 3d preview on the right or something, then the editor on the left
	public override void Render()
	{
		// Render the portal gun texture and
		// all of the layers on top of it 
		Raylib.DrawTexture(portalGunTexture, 0, 0, Color.White);
		layerHandler.DrawLayersWithControls();
	}



	public override void CleanUp()
	{
		// Unload all the textures
		Raylib.UnloadTexture(portalGunTexture);
		layerHandler.CleanUp();
	}
}