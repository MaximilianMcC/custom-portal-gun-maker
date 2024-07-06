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
			if (!(layer.MouseOnBody() || layer.MouseOnTransformControl(out _))) continue;

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

		// Check for if the user is holding down on something. If
		// they are then exit early. Otherwise continue and get the 
		// selected layer since we'll be using it a decent bit
		if (Raylib.IsMouseButtonDown(MouseButton.Left) == false) return;
		Layer selectedLayer = Layers[SelectedIndex];

		// If the user is holding down on a transform
		// control then they want to resize the image
		if (selectedLayer.MouseOnTransformControl(out int? controlIndex))
		{
			// Get the distance that the mouse has moved
			// since the last time we moved it
			Vector2 offset = Raylib.GetMouseDelta();
			Vector2 size = selectedLayer.GetSize();
			Vector2 scale;

			// Check for what transform control they're on
			// and resize the image according to that. The resizing
			// works by getting the distance the transform control
			// has moved, then turning that into a scale and adding
			// it onto the original scale
			switch (controlIndex)
			{
				// Top left
				case 0:
				    scale = -offset / size;
					selectedLayer.Scale += scale;
					selectedLayer.Position += offset;
					break;

				// Top right
				case 1:
					scale = new Vector2(offset.X, -offset.Y) / size;
					selectedLayer.Scale += scale;
					selectedLayer.Position.Y += offset.Y;
					break;

				// Bottom left
				case 2:
					scale = new Vector2(-offset.X, offset.Y) / size;
					selectedLayer.Scale += scale;
					selectedLayer.Position.X += offset.X;
					break;

				// Bottom right
				case 3:
					scale = offset / size;
					selectedLayer.Scale += scale;
					break;
			}

			// Return (only one action per frame please)
			return;
		}

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

			// Return (only one action per frame please)
			return;
		}
	}

	// Delete any needed layers
	public void DeleteLayers()
	{
		// Check for if we press either the delete
		// key, backspace, or x (blender users)
		if (!(Raylib.IsKeyPressed(KeyboardKey.Delete) || Raylib.IsKeyPressed(KeyboardKey.Backspace) || Raylib.IsKeyPressed(KeyboardKey.X))) return;

		// Delete the selected layer
		Layers.Remove(Layers[SelectedIndex]);

		// Get every index after/higher than the deleted
		// layer and bring it back one
		foreach (Layer layer in Layers)
		{
			if (layer.Index > SelectedIndex) layer.Index--;
		}
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
				new Rectangle(Vector2.Zero, layer.GetSize()),
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
				new Rectangle(Vector2.Zero, layer.GetSize()),
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

	// Get the original size of the layer
	//! Does NOT come with scale applied
	public Vector2 GetSize()
	{
		return new Vector2(Texture.Width, Texture.Height);
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
	// TODO: Could return an enum (top left, bottom right, etc) instead of controlIndex int
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