using System.Numerics;
using Raylib_cs;

class Portal1PortalGunGenerator : Scene
{
	private List<Layer> layers;
	private Texture2D portalGunTexture;

	public override void Start()
	{
		// Make the layers list
		layers = new List<Layer>();

		// Load in the portal gun texture
		portalGunTexture = Raylib.LoadTexture("./assets/game/temp.png");
	}

	public override void Update()
	{
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
				Position = Vector2.Zero,
				Rotation = 0f,
				Scale = Vector2.One
			};
			layers.Add(layer);
		}
	}

	// TODO: Have the 3d preview on the right or something, then the editor on the left
	public override void Render()
	{
		// Render what the texture will look like
		{
			// Render the portal gun texture
			Raylib.DrawTexture(portalGunTexture, 0, 0, Color.White);

			// Render all of the layers
			foreach (Layer layer in layers)
			{
				// Get the size in a nice way
				Vector2 size = new Vector2(layer.Texture.Width, layer.Texture.Height);

				// Draw the texture
				Raylib.DrawTexturePro(
					layer.Texture,
					new Rectangle(0, 0, size),
					new Rectangle(layer.Position, size * layer.Scale),
					Vector2.Zero,
					layer.Rotation,
					Color.White
				);
			}
		}

		// Render all of the layers
		{
			// Dimension things
			int layerPanelWidth = 150;
			int layerPanelX = (Raylib.GetScreenWidth() / 2) - layerPanelWidth;
			int x = layerPanelX;
			int y = 10;

			// Draw the background and title thing
			Raylib.DrawRectangle(layerPanelX, 0, layerPanelWidth, Raylib.GetScreenHeight(), Color.Green);
			Raylib.DrawText("layers", x, y, 30, Color.White);
			y += 30 + 10;

			// Draw every layer
			int height = 100;
			foreach (Layer layer in layers)
			{
				// Draw a background
				Raylib.DrawRectangle(x, y, layerPanelWidth, height, Color.DarkBlue);

				// Draw a preview of the original texture
				Raylib.DrawTexturePro(
					layer.Texture,
					new Rectangle(0, 0, layer.Texture.Width, layer.Texture.Height),
					new Rectangle(x, y, layerPanelWidth, height),
					Vector2.Zero,
					0f,
					Color.White
				);

				// Increase the y for next time
				y += height + 10;
			}
		}
	}

	public override void CleanUp()
	{
		// Unload all the textures
		Raylib.UnloadTexture(portalGunTexture);
		layers.ForEach(layer => Raylib.UnloadTexture(layer.Texture));
	}
}


struct Layer
{
	public Texture2D Texture;
	public Vector2 Position;
	public float Rotation;
	public Vector2 Scale;
}