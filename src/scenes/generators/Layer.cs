using System.Numerics;
using Raylib_cs;

class LayerHandler
{
	public List <Layer> Layers { get; set; }
	public int SelectedIndex { get; set; }
	public bool Debug { get; set; }

	public LayerHandler()
	{
		// Make the layers list
		// TODO: Don't use -1 (make unsigned (faster))
		Layers = new List<Layer>();
		SelectedIndex = -1;
	}

	// Check for if a layer is being selected (clicked on)
	public void GetSelectedLayer()
	{
		// Check for if the user is clicking
		if (Raylib.IsMouseButtonPressed(MouseButton.Left) == false) return;

		// Check for if the users mouse is over a layer
		Vector2 mousePosition = Raylib.GetMousePosition();
		int selectedLayerIndex = 0;
		foreach (Layer layer in Layers)
		{
			// If the layer can be selected then add the index
			// to a list of indexes so that we can figure
			// out what the top one was selected
			if (!Raylib.CheckCollisionPointRec(mousePosition, layer.GetRectangle())) continue;
			if (layer.Index > selectedLayerIndex) selectedLayerIndex = layer.Index;
		}

		SelectedIndex = selectedLayerIndex;
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
			Raylib.DrawTexturePro(
				layer.Texture,
				new Rectangle(Vector2.Zero, new Vector2(layer.Texture.Width, layer.Texture.Height)),
				layer.GetRectangle(),
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
				Raylib.DrawRectangleLinesEx(layer.GetRectangle(), lineThickness, lineColor);

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
		
			// Draw the layer index in the top corner 
			// and a little border around it (debug only)
			if (Debug)
			{
				Rectangle rectangle = layer.GetRectangle();
				Raylib.DrawRectangleLinesEx(rectangle, 1f, Color.Red);
				Raylib.DrawText($"{layer.Index}", (int)(layer.Position.X + (rectangle.Width / 2)), (int)(layer.Position.Y + (rectangle.Height / 2)), 20, Color.White);
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
			Raylib.DrawTexturePro(
				layer.Texture,
				new Rectangle(Vector2.Zero, new Vector2(layer.Texture.Width, layer.Texture.Height)),
				layer.GetRectangle(),
				Vector2.Zero,
				layer.Rotation,
				Color.White
			);
		}
	}

	// Get rid of all the rubbish
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

	public Rectangle GetRectangle()
	{
		return new Rectangle(Position, new Vector2(Texture.Width, Texture.Height) * Scale);
	}

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