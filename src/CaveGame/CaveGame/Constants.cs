using Microsoft.Xna.Framework;

namespace CaveGame {
    public class Constants {

        // CaveEditor.cs
        public const int CAVE_WIDTH       = 800;
        public const int CAVE_HEIGHT      = 600;
        public const int BUFFER_ZONE      = 10;
        public static Color TOOLBOX_COLOR = Color.LightGray;
        public static Color BUFFER_COLOR  = Color.Black;

        // Blocks
        public static Color SOLID_BLOCK_COLOR     = Color.Gray;
        public static Color AIR_BLOCK_COLOR       = Color.White;
        public static Color ENTERANCE_BLOCK_COLOR = Color.LightGreen;

        // Level.cs
        public const int LEVEL_ROWS          = 80;
        public const int LEVEL_COLUMNS       = 60;
        public const int MIN_NEIGHBORS_ALIVE = 4;
        public const int MAX_NEIGHBORS_ALIVE = 8;

        // Player.cs
        public static Color PLAYER_COLOR             = Color.Green;
	    public const double PLAYER_ACCELERATION_X    = 0.1;
        public const double PLAYER_MAX_SPEED_X       = 1;
        public const double PLAYER_ACCELERATION_JUMP = 1;

        // Scenes
        public const int GAME_WIDTH = CAVE_WIDTH + BUFFER_ZONE * 3 + (CAVE_WIDTH / LEVEL_ROWS) * 2;
        public const int GAME_HEIGHT = CAVE_HEIGHT + BUFFER_ZONE * 2;
        
        // Tilemap
        public const int TILE_WIDTH  = CAVE_WIDTH / LEVEL_ROWS;
        public const int TILE_HEIGHT = CAVE_HEIGHT / LEVEL_COLUMNS;
    }
}
