using System.Numerics;
using Raylib_cs;

class LayerHandler
{
	public bool Debug { get; set; }

	public List <Layer> Layers { get; set; }
	public int SelectedIndex { get; set; }


	public LayerHandler()
	{
		// Make the layers list
		// TODO: Don't use -1 (make unsigned (faster))
		Layers = new List<Layer>();
		SelectedIndex = -1;
	}

	// Check for if a layer is being selected (clicked on)
	// TODO: Also make work for the layer view thing on the side
	public void GetSelectedLayer()
	{
		// Check for if bro is clicking
		if (Raylib.IsMouseButtonPressed(MouseButton.Left) == false) return;
		
		// Loop over every layer
		// TODO: Don't use -1
		int selectedLayerIndex = -1;
		foreach (Layer layer in Layers)
		{
			// Check for if the mouse is over it
			//? Purposely decided to only use the body, not transform controls because you can't see them
			if (!layer.MouseOnBody()) continue;

			// Check for if the current layer is above the previously
			// selected layer. We only want to select the highest layer
			if (layer.Index > selectedLayerIndex) selectedLayerIndex = layer.Index;
		}

		// Select the layer
		SelectedIndex = selectedLayerIndex;
	}

	public void TransformSelectedLayer()
	{
		// TODO: Don't do this
		//! bad
		if (SelectedIndex == -1) return;

		// Check for if the user is holding down on something
		if (Raylib.IsMouseButtonDown(MouseButton.Left) == false) return;

		// Get the selected layer
		Layer selectedLayer = Layers[SelectedIndex];

		// If the user is holding down on the body then
		// then want to drag the image around
		if (selectedLayer.MouseOnBody())
		{
			// Get the distance that the mouse has moved
			// since the last time we moved it and add
			// it to the position of the layer so that
			// it follows the mouse (dragging)
			Vector2 offset = Raylib.GetMouseDelta();
			selectedLayer.Position += offset;
		}
	}

	// Check for if the user wants to resize the currently selected layer
	public void ResizeSelectedLayer()
	{
		// TODO: Don't do this
		//! bad
		if (SelectedIndex == -1) return;

		// Check for if the user is holding down on something
		if (Raylib.IsMouseButtonDown(MouseButton.Left) == false) return;

		// Check for if the user is holding down on one
		// of the transform controls of the selected layer,
		// and if so what one they are holding down on
		// TODO: Use a bigger hitbox for the controls
		Vector2 mousePosition = Raylib.GetMousePosition();
		Rectangle[] transformControls = Layers[SelectedIndex].GetTransformControls();
		if (Raylib.CheckCollisionPointRec(mousePosition, transformControls[0])) return;
		if (Raylib.CheckCollisionPointRec(mousePosition, transformControls[1])) return;
		if (Raylib.CheckCollisionPointRec(mousePosition, transformControls[2])) return;
		if (Raylib.CheckCollisionPointRec(mousePosition, transformControls[3])) return;
	}

	// Check for if the user wants to rotate the currently selected layer
	public void RotateSelectedLayer()
	{

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
	// final stuff used for baking 
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

	// Get the body's rectangle
	public Rectangle GetRectangle()
	{
		return new Rectangle(Position, new Vector2(Texture.Width, Texture.Height) * Scale);
	}

	// Get the rectangles of all transform controls
	public Rectangle[] GetTransformControls()
	{
		// Precalculate this thingy
		Vector2 resizeBoxSize = new Vector2(16f);
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

	// Check for if the mouse is hovering over the
	// body of this layer
	public bool MouseOnBody()
	{
		// Get the mouse position
		Vector2 mousePosition = Raylib.GetMousePosition();

		// Check for collision
		return Raylib.CheckCollisionPointRec(mousePosition, GetRectangle());
	}

	// Check for if the mouse is hovering over a
	// transform control of this layer
	public bool MouseOnTransformControl(out int? controlIndex)
	{
		// Get the mouse position
		Vector2 mousePosition = Raylib.GetMousePosition();

		// Get all of the controls then loop through them all
		Rectangle[] controls = GetTransformControls();
		for (int i = 0; i < controls.Length; i++)
		{
			// Check for if we've hit a control
			if (Raylib.CheckCollisionPointRec(mousePosition, controls[i]))
			{
				controlIndex = i;
				return true;
			}
		}

		// We didn't hit any controls
		controlIndex = null;
		return false;
	}
}