using Microsoft.Xna.Framework;

namespace CaveGame {
    public class Constants {
        // Display.java
        public const int DISPLAY_WIDTH  = 800;
        public const int DISPLAY_HEIGHT = 600;
        
        // LevelBuilder.java
        public const int LEVEL_ROWS          = 80;
        public const int LEVEL_COLUMNS       = 60;
        public const int MIN_NEIGHBORS_ALIVE = 4;
        public const int MAX_NEIGHBORS_ALIVE = 8;

        // Player.java
        public Color PLAYER_COLOR                    = Color.Green;
	    public const double PLAYER_ACCELERATION_X    = 0.1;
        public const double PLAYER_MAX_SPEED_X       = 1;
        public const double PLAYER_ACCELERATION_JUMP = 1;
    }
}
