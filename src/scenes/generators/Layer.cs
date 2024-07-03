using System.Numerics;
using Raylib_cs;

class LayerHandler
{
	public List <Layer> Layers { get; set; }
	public int SelectedIndex { get; set; }

	public LayerHandler()
	{
		// Make the layers list
		// TODO: Don't use -1 (make unsigned (faster))
		Layers = new List<Layer>();
		SelectedIndex = -1;
	}

	// Draw the layers and include stuff like
	// transform controls and whatnot
	public void DrawLayersWithControls()
	{
		// Loop over every layer
		foreach (Layer layer in Layers)
		{
			// Draw the layer
			// TODO: Don't reuse code
			Vector2 size = new Vector2(layer.Texture.Width, layer.Texture.Height);
			Raylib.DrawTexturePro(
				layer.Texture,
				new Rectangle(Vector2.Zero, size),
				new Rectangle(layer.Position, size * layer.Scale),
				Vector2.Zero,
				layer.Rotation,
				Color.White
			);

			// If the layer is selected, then draw
			// the transform controls around it
			if (SelectedIndex == layer.Index)
			{
				// Settings idk
				float lineThickness = 2f;
				Color lineColor = Color.Magenta;
				Color backgroundColor = Color.White;

				// Draw a border thingy around it
				Raylib.DrawRectangleLinesEx(new Rectangle(layer.Position, size * layer.Scale), lineThickness, lineColor);

				// Draw all of the transform control box
				// things on each corner of the image
				foreach (Rectangle control in layer.GetTransformControls())
				{
					// Draw a outline thingy around it, and also
					// a background color
					Raylib.DrawRectangleRec(control, backgroundColor);
					Raylib.DrawRectangleLinesEx(control, lineThickness, lineColor);
				}
			}
			
		}
	}

	// Draw layers but with nothing else.
	// final stuff used for baking and whatnot
	public void DrawLayers()
	{
		// Loop over every layer
		foreach (Layer layer in Layers)
		{
			// Draw the layer
			// TODO: Don't reuse code
			Vector2 size = new Vector2(layer.Texture.Width, layer.Texture.Height);
			Raylib.DrawTexturePro(
				layer.Texture,
				new Rectangle(Vector2.Zero, size),
				new Rectangle(layer.Position, size * layer.Scale),
				Vector2.Zero,
				layer.Rotation,
				Color.White
			);
		}
	}

	public void CleanUp()
	{
		// Unload all of the layer textures
		Layers.ForEach(layer => Raylib.UnloadTexture(layer.Texture));
	}
}

class Layer
{
	public Texture2D Texture;
	public Vector2 Position;
	public float Rotation;
	public Vector2 Scale;
	public int Index;

	public Rectangle[] GetTransformControls()
	{
		// Precalculate this thingy
		Vector2 resizeBoxSize = new Vector2(15f);
		Vector2 resizeBoxOffset = resizeBoxSize / 2;

        // Precalculate the y positions
        Vector2 top = new Vector2(0, Position.Y - resizeBoxOffset.Y);
        Vector2 bottom = new Vector2(0, Position.Y + (Texture.Height * Scale.Y) - resizeBoxOffset.Y);

        // Precalculate the x positions
        Vector2 left = new Vector2(Position.X - resizeBoxOffset.X, 0);
        Vector2 right = new Vector2(Position.X + (Texture.Width * Scale.X) - resizeBoxOffset.X, 0);

		// Make all of the transform control
        // rectangles then return them
		return new Rectangle[]
		{
			new Rectangle(top + left, resizeBoxSize),
			new Rectangle(top + right, resizeBoxSize),
			new Rectangle(bottom + left, resizeBoxSize),
			new Rectangle(bottom + right, resizeBoxSize),
		};
	}
}