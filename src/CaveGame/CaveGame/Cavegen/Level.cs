using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez.Tiled;
using Glint;

namespace CaveGame.Cavegen {
    public class Level {
        public Vector2 spawn;
        public int waterLevel;

        private Block[,] data;

        public Level() {
            data = new Block[Constants.LEVEL_ROWS, Constants.LEVEL_COLUMNS];
            spawn.X = -1;
            spawn.Y = -1;
            waterLevel = 20;
        }

        public void set(int i, int j, Block block) {
            if (i < 0 || i > Constants.LEVEL_ROWS) return;
            if (j < 0 || j > Constants.LEVEL_COLUMNS) return;
            data[i, j] = block;

            // Entrance
            if (block.id == (int)Constants.Id.Entrance) {
                // We know if X is -1, then Y is -1 because of the constructor. Unless something went horribly wrong.
                // Also I want to check if the player isnt simply holding the button in the same spot.
                if (spawn.X != -1 && spawn.X != i && spawn.Y != j)
                    data[(int) spawn.X, (int)spawn.Y] = new AirBlock();
                spawn.X = i;
                spawn.Y = j;
            }
        }

        public Block get(int i, int j) {
            return data[i, j];
        }

        public Level clone() {
            Level clone = new Level();
            for (int i = 0; i < Constants.LEVEL_ROWS; i++) {
                for (int j = 0; j < Constants.LEVEL_COLUMNS; j++) {
                    clone.set(i, j, data[i, j]);
                }
            }

            return clone;
        }

        public TiledMap bake() {
            // initializing
            TiledMap map = new TiledMap(0, Constants.LEVEL_ROWS, Constants.LEVEL_COLUMNS, Constants.TILE_WIDTH, Constants.TILE_HEIGHT);
            Texture2D tilesetTexture = GlintCore.contentSource.Load<Texture2D>("spritesheet");
            TiledTileset tiledTileset = map.createTileset(tilesetTexture, 0, Constants.TILE_WIDTH, Constants.TILE_HEIGHT, true, 0, 0, 5, 5);

            // creating the collidable tiledtile[]
            TiledTile[] collidableTiles = new TiledTile[data.GetLength(0) * data.GetLength(1)];
            int mw = data.GetLength(0);
            int mh = data.GetLength(1);
            for (int j = 0; j < mh; j++) {
                for (int i = 0; i < mw; i++) {
                    if (data[i, j].id == (int) Constants.Id.Solid)
                        collidableTiles[j * mw + i] = new TiledTile(data[i, j].id) { tileset = tiledTileset };
                }
            }

            // creating the water tiledtile[]
            TiledTile[] waterTiles = new TiledTile[data.GetLength(0) * data.GetLength(1)];
            for (int i = 0; i < mw; i++) {
                for (int j = mh - waterLevel; j < mh; j++) {
                        waterTiles[j * mw + i] = new TiledTile((int) Constants.Id.Water) { tileset = tiledTileset };
                }
            }

            // adding the layers togetherz
            map.createTileLayer("water", map.width, map.height, waterTiles);
            map.createTileLayer("walls", map.width, map.height, collidableTiles);

            return map;
        }

        public double getDensityScore(int x, int y, double threshold, Block searchBlock) {
            double score = 0;
            for (int i = 0; i < x; i++) {
                for (int j = 0; j < y; j++) {
                    int numAlive = 0;
                    for (int k = i * (Constants.LEVEL_ROWS / x); k < (i + 1) * (Constants.LEVEL_ROWS / x); k++) {
                        for (int l = j * (Constants.LEVEL_COLUMNS / y); l < (j + 1) * (Constants.LEVEL_COLUMNS / y); l++) {
                            if (data[k, l].getColor() == searchBlock.getColor()) {
                                numAlive++;
                            }
                        }
                    }
                    double density = numAlive / (((double)Constants.LEVEL_ROWS / x) * ((double)Constants.LEVEL_COLUMNS / y));
                    score += 1 - Math.Abs(density - threshold);
                }
            }

            return score / (x * y);
        }

        public double getRoomSizeScore(int threshold, Block searchBlock) {
            int numberOfRoomsThatMeetThreshold = 0;
            int numberOfRooms = 0;

            bool[,] isMarked = new bool[Constants.LEVEL_ROWS, Constants.LEVEL_COLUMNS]; // default values of false
            for (int i = 0; i < Constants.LEVEL_ROWS; i++) {
                for (int j = 0; j < Constants.LEVEL_COLUMNS; j++) {
                    if (!isMarked[i, j] && data[i, j].getColor() == searchBlock.getColor()) {
                        int roomSize = 0;
                        Stack blocksToSearch = new Stack();
                        blocksToSearch.Push(i);
                        blocksToSearch.Push(j);
                        while (blocksToSearch.Count != 0) {
                            int y = (int)blocksToSearch.Pop();
                            int x = (int)blocksToSearch.Pop(); // i'm using a stack
                            if (x != Constants.LEVEL_ROWS - 1 && data[x + 1, y].getColor() == searchBlock.getColor() && !isMarked[x + 1, y]) {
                                blocksToSearch.Push(x + 1);
                                blocksToSearch.Push(y);
                                isMarked[x + 1, y] = true;
                            }
                            if (x != 0 && data[x - 1, y].getColor() == searchBlock.getColor() && !isMarked[x - 1, y]) {
                                blocksToSearch.Push(x - 1);
                                blocksToSearch.Push(y);
                                isMarked[x - 1, y] = true;
                            }
                            if (y != Constants.LEVEL_COLUMNS - 1 && data[x, y + 1].getColor() == searchBlock.getColor() && !isMarked[x, y + 1]) {
                                blocksToSearch.Push(x);
                                blocksToSearch.Push(y + 1);
                                isMarked[x, y + 1] = true;
                            }
                            if (y != 0 && data[x, y - 1].getColor() == searchBlock.getColor() && !isMarked[x, y - 1]) {
                                blocksToSearch.Push(x);
                                blocksToSearch.Push(y - 1);
                                isMarked[x, y - 1] = true;
                            }
                            roomSize++;
                        }

                        numberOfRooms++;
                        if (roomSize > threshold) {
                            numberOfRoomsThatMeetThreshold++;
                        }
                    }
                }
            }
            return numberOfRoomsThatMeetThreshold / (double)numberOfRooms;
        }

        public Level fillLevel(Block block) {
            for (int i = 0; i < Constants.LEVEL_ROWS; i++) {
                for (int j = 0; j < Constants.LEVEL_COLUMNS; j++) {
                    set(i, j, block);
                }
            }

            return this;
        }

        public Level createRooms(int numberOfRooms, int minimumRoomWidth, int minimumRoomHeight, int maximumRoomWidth, int maximumRoomHeight, Block block) {
            for (int i = 0; i < numberOfRooms; i++) {
                Random rnd = new Random();
                int x1 = (int)(rnd.NextDouble() * (Constants.LEVEL_ROWS - minimumRoomWidth));
                int y1 = (int)(rnd.NextDouble() * (Constants.LEVEL_COLUMNS - minimumRoomHeight));
                int x2 = (int)(x1 + (rnd.NextDouble() * (maximumRoomWidth - minimumRoomWidth)));
                int y2 = (int)(y1 + (rnd.NextDouble() * (maximumRoomHeight - minimumRoomHeight)));
                if (x2 > Constants.LEVEL_ROWS) x2 = Constants.LEVEL_ROWS - 1;
                if (y2 > Constants.LEVEL_COLUMNS) y2 = Constants.LEVEL_COLUMNS - 1;

                for (int j = x1; j < x2; j++) {
                    for (int k = y1; k < y2; k++) {
                        set(j, k, block);
                    }
                }
            }

            return this;
        }

        public Level smooth(Block aliveState, Block deadState) {
            Level tempLevel = clone();

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
                        set(i, j, aliveState);
                    } else {
                        set(i, j, deadState);
                    }
                }
            }
            return this;
        }
    }
}