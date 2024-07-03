using System.Numerics;
using Raylib_cs;

class P1PortalgunGenerator : Scene
{
	private Texture2D portalGunTexture;
	private List<Layer> layers;
	private int selectedLayerIndex;

	public override void Start()
	{
		// Make the layers list
		layers = new List<Layer>();
		selectedLayerIndex = 0;

		// Load in the portalgun texture
		portalGunTexture = Raylib.LoadTexture("./assets/game/p1gun/v_portalgun.png");
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
				// Position = Vector2.Zero,
				Position = new Vector2(25f),
				Rotation = 0f,
				Scale = Vector2.One,
				layerIndex = layers.Count
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
			for (int i = 0; i < layers.Count; i++)
			{
				Layer layer = layers[i];

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

				// If we're on the currently selected
				// thingy then draw transform controls
				// around the current image
				if (i != selectedLayerIndex) continue;

				// Draw the main box around it
				Raylib.DrawRectangleLinesEx(new Rectangle(layer.Position, size), 2f, Color.Magenta);

				// Draw the four transform controls
				foreach (Rectangle transformControl in layer.GetTransformControls())
				{
					Raylib.DrawRectangleRec(transformControl, Color.White);
					Raylib.DrawRectangleLinesEx(transformControl, 2f, Color.Magenta);
				}
			}
		}

		// Render all of the layers
		{
			// Dimension things
			int layerPanelWidth = 150;
			int layerPanelX = Raylib.GetScreenWidth() - layerPanelWidth;
			int x = layerPanelX;
			int y = 10;

			// Draw the background and title thing
			Raylib.DrawRectangle(layerPanelX, 0, layerPanelWidth, Raylib.GetScreenHeight(), Color.Green);
			Raylib.DrawText("layers", x, y, 30, Color.White);
			y += 30 + 10;

			// Draw every layer
			int height = 100;
			for (int i = 0; i < layers.Count; i++)
			{
				Layer layer = layers[i];

				// Draw a background
				Color color = (i == selectedLayerIndex) ? Color.DarkBlue : Color.DarkGreen;
				Raylib.DrawRectangle(x, y, layerPanelWidth, height, color);

				// Draw a preview of the original texture
				Raylib.DrawTexturePro(
					layer.Texture,
					new Rectangle(0, 0, layer.Texture.Width, layer.Texture.Height),
					new Rectangle(x + 10, y + 10, layerPanelWidth / 2, height / 2),
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