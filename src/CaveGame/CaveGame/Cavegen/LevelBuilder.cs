using System;
using System.Collections.Generic;
using System.Text;

namespace CaveGame.Cavegen
{
    class LevelBuilder {
        private Level level;

        public LevelBuilder() {
            level = new Level();
        }

        public LevelBuilder fillLevel(Block block) {
            for (int i = 0; i < Constants.LEVEL_ROWS; i++) {
                for (int j = 0; j < Constants.LEVEL_COLUMNS; j++) {
                    level.set(i, j, block);
                }
            }

            return this;
        }

        public LevelBuilder createRooms(int numberOfRooms, int minimumRoomWidth, int minimumRoomHeight, int maximumRoomWidth, int maximumRoomHeight, Block block) {
            for (int i = 0; i < numberOfRooms; i++) {
                Random rnd = new Random();
                int x1 = (int) (rnd.NextDouble() * (Constants.LEVEL_ROWS - minimumRoomWidth));
                int y1 = (int) (rnd.NextDouble() * (Constants.LEVEL_COLUMNS - minimumRoomHeight));
                int x2 = (int) (x1 + (rnd.NextDouble() * (maximumRoomWidth - minimumRoomWidth)));
                int y2 = (int) (y1 + (rnd.NextDouble() * (maximumRoomHeight - minimumRoomHeight)));
                if (x2 > Constants.LEVEL_ROWS) x2 = Constants.LEVEL_ROWS - 1;
                if (y2 > Constants.LEVEL_COLUMNS) y2 = Constants.LEVEL_COLUMNS - 1;

                for (int j = x1; j < x2; j++) {
                    for (int k = y1; k < y2; k++) {
                        level.set(j, k, block);
                    }
                }
            }

            return this;
        }

        public LevelBuilder smooth(Block aliveState, Block deadState) {
            Level tempLevel = level.clone();

            for (int i = 0; i < Constants.LEVEL_ROWS; i++) {
                for (int j = 0; j < Constants.LEVEL_COLUMNS; j++) {
                    int neighbors = 0;

                    for (int k = 0; k < 3; k++) {
                        for (int l = 0; l < 3; l++) {
                            if ((k == 1 && l == 1) || 
                                (i == 0 && k == 0) ||
                                (j == 0 && l == 0) ||
                                (i == Constants.LEVEL_ROWS - 1 && k == 2) ||
                                (j == Constants.LEVEL_COLUMNS - 1 && l == 2))
                                continue;

                            if (tempLevel.get(i + (k - 1), j + (l - 1)).getColor() == aliveState.getColor()) {
                                neighbors++;
                            }
                        }
                    }

                    if (neighbors >= Constants.MIN_NEIGHBORS_ALIVE && neighbors <= Constants.MAX_NEIGHBORS_ALIVE) {
                        level.set(i, j, aliveState);
                    } else {
                        level.set(i, j, deadState);
                    }
                }
            }
            return this;
        }

        public Level getLevel() {
            return level;
        }
    }
}
