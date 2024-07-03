using System.Numerics;
using Raylib_cs;

class Layer
{
	public Texture2D Texture;
	public Vector2 Position;
	public float Rotation;
	public Vector2 Scale;
	public int layerIndex;

	public Rectangle[] GetTransformControls()
	{
		// Settings stuff
		Vector2 resizeBox = new Vector2(15f);
		Vector2 resizeBoxOffset = resizeBox / 2;

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
			new Rectangle(top + left, resizeBox),
			new Rectangle(top + right, resizeBox),
			new Rectangle(bottom + left, resizeBox),
			new Rectangle(bottom + right, resizeBox),
		};
	}

}