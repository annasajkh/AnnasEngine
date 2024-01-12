using DemoGame.Scripts.Core;

namespace DemoGame.Scripts;

public static class Program
{
    static void Main(string[] args)
    {
        using (Game game = new Game("Demo Game", 960, 540))
        {
            game.Run();
        }
    }
}
