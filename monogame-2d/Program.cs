
using System;
using monogame_2d;
using monogame2d.States;

namespace monogame2d
{
    public static class Program
    {
        private const int WIDTH = 1280;
        private const int HEIGHT = 720;
            [STAThread]
            static void Main()
            {
                using (var game = new MainGame(WIDTH, HEIGHT, new SplashState()))
                    game.Run();
            }
    }
}