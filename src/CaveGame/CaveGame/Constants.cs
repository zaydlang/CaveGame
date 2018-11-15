using Microsoft.Xna.Framework;

namespace CaveGame {
    public class Constants {

        // CaveEditor.cs
        public const int CAVE_WIDTH       = 800;
        public const int CAVE_HEIGHT      = 600;
        public const int BUFFER_ZONE      = 0;
        public static Color TOOLBOX_COLOR = Color.LightGray;
        public static Color BUFFER_COLOR  = Color.Black;

        // Blocks
        public static Color SOLID_BLOCK_COLOR    = Color.Gray;
        public static Color AIR_BLOCK_COLOR      = Color.White;
        public static Color ENTRANCE_BLOCK_COLOR = Color.LightGreen;
        public static Color TORCH_BLOCK_COLOR    = Color.Yellow;
        public static Color WATER_BLOCK_COLOR    = Color.LightBlue;
        public static int TORCH_BLOCK_RADIUS     = 5;

        // Level.cs
        public const int LEVEL_ROWS          = 80;
        public const int LEVEL_COLUMNS       = 60;
        public const int MIN_NEIGHBORS_ALIVE = 4;
        public const int MAX_NEIGHBORS_ALIVE = 8;

        // Player.cs
        public static Color PLAYER_COLOR        = Color.Green;
        public static int PLAYER_WIDTH          = 10;
        public static int PLAYER_HEIGHT         = 10;
        public static double PLAYER_SPEED       = 120;
        public static double PLAYER_JUMP_HEIGHT = 100;

        // Scenes
        public const int GAME_WIDTH  = CAVE_WIDTH + BUFFER_ZONE * 3 + (CAVE_WIDTH / LEVEL_ROWS) * 2;
        public const int GAME_HEIGHT = CAVE_HEIGHT + BUFFER_ZONE * 2;
        
        // Tilemap
        public const int TILE_WIDTH  = CAVE_WIDTH / LEVEL_ROWS;
        public const int TILE_HEIGHT = CAVE_HEIGHT / LEVEL_COLUMNS;
        public const double GRAVITY  = 650;

        // Water
        public const int BOUYANT_FORCE = 1000;

        // Light
        public const int LIGHT_STRENGTH = 50;

        // Enum
        public enum Id {
            Solid,
            Air,
            Entrance,
            Torch,
            Water
        }

        public enum Layer {
            Water,
            Walls
        }
    }
}
