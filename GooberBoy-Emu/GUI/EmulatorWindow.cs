using SFML.Window;
using SFML.Graphics;
namespace GooberBoy_Emu.GUI;

public class EmulatorWindow
{
    const int WIDTH = 640;
    const int HEIGHT = 480;
    const string TITLE = "gooberBoy Emu";
    public static void TestSFML()
    {
     
        VideoMode mode = new VideoMode(WIDTH, HEIGHT);
        RenderWindow window = new RenderWindow(mode, TITLE);
            
        window.SetVerticalSyncEnabled(true);
        
        window.Closed += (sender, args) => window.Close();
        while (window.IsOpen)
        {
            window.DispatchEvents();
            window.Clear(Color.Blue);
            window.Display();
        }
    }
}