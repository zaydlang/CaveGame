using System;

namespace CaveGame {
    public static class Program {
        [STAThread]
        static void Main() {
            using (var game = new NGame())
                game.Run();
        }
    }
}