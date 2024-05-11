using System.Numerics;
using Raylib_cs;

class Program
{
    private static Font font;
    private static Image icon;
    private static Image basePortalGunImage;
    private static Texture2D basePortalGunTexture;
    private static Camera3D camera;

    public static void Main(string[] args)
    {
        Raylib.SetTraceLogLevel(TraceLogLevel.Warning);
        Raylib.InitWindow(800, 600, "I jus,....... lived I guess");
        font = Raylib.LoadFont("./assets/font.otf");
        icon = Raylib.LoadImage("./assets/icon.png");
        Raylib.SetWindowIcon(icon);

        camera = new Camera3D()
        {
            Position = Vector3.Zero,
            Target = Vector3.Zero,
            Up = Vector3.UnitY,
            FovY = 90, //? from gbt
            Projection = CameraProjection.Perspective
        };

        // Load in the base portal gun texture
        basePortalGunImage = Raylib.LoadImage("./assets/v_portalgun.png");
        basePortalGunTexture = Raylib.LoadTextureFromImage(basePortalGunImage);

        while (!Raylib.WindowShouldClose())
        {
            Update();
            Render();
        }


        Raylib.UnloadFont(font);
        Raylib.UnloadImage(icon);
        Raylib.UnloadImage(basePortalGunImage);
        Raylib.UnloadTexture(basePortalGunTexture);
        Raylib.CloseWindow();
    }


    private static void Update()
    {
        
    }

    private static void Render()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Magenta);

        Raylib.DrawTextureEx(basePortalGunTexture, Vector2.Zero, 0f, 0.3f, Color.White);
        Raylib.DrawTextEx(font, "Chell portal", new Vector2(10, (Raylib.GetScreenHeight() - 35f) - 10), 35f, (35f / 10f), Color.White);

        Raylib.EndDrawing();
    }
}