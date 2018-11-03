using System;
using System.Collections;

namespace CaveGame.Cavegen {
    public class Level {
        private Block[,] levelMap;

        public Level() {
            levelMap = new Block[Constants.LEVEL_ROWS, Constants.LEVEL_COLUMNS];
        }

        public void set(int i, int j, Block block) {
            if (i < 0 || i > Constants.LEVEL_ROWS) return;
            if (j < 0 || j > Constants.LEVEL_COLUMNS) return;
            levelMap[i, j] = block;
        }

        public Block get(int i, int j) {
            return levelMap[i, j];
        }

        public Level clone() {
            Level clone = new Level();
            for (int i = 0; i < Constants.LEVEL_ROWS; i++) {
                for (int j = 0; j < Constants.LEVEL_COLUMNS; j++) {
                    clone.set(i, j, levelMap[i, j]);
                }
            }

            return clone;
        }

        public double getDensityScore(int x, int y, double threshold, Block searchBlock) {
            double score = 0;
            for (int i = 0; i < x; i++) {
                for (int j = 0; j < y; j++) {
                    int numAlive = 0;
                    for (int k = i * (Constants.LEVEL_ROWS / x); k < (i + 1) * (Constants.LEVEL_ROWS / x); k++) {
                        for (int l = j * (Constants.LEVEL_COLUMNS / y); l < (j + 1) * (Constants.LEVEL_COLUMNS / y); l++) {
                            if (levelMap[k, l].getColor() == searchBlock.getColor()) {
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
                    if (!isMarked[i, j] && levelMap[i, j].getColor() == searchBlock.getColor()) {
                        int roomSize = 0;
                        Stack blocksToSearch = new Stack();
                        blocksToSearch.Push(i);
                        blocksToSearch.Push(j);
                        while (blocksToSearch.Count != 0) {
                            int y = (int) blocksToSearch.Pop();
                            int x = (int) blocksToSearch.Pop(); // i'm using a stack
                            if (x != Constants.LEVEL_ROWS - 1 && levelMap[x + 1, y].getColor() == searchBlock.getColor() && !isMarked[x + 1, y]) {
                                blocksToSearch.Push(x + 1);
                                blocksToSearch.Push(y);
                                isMarked[x + 1, y] = true;
                            }
                            if (x != 0 && levelMap[x - 1, y].getColor() == searchBlock.getColor() && !isMarked[x - 1, y]) {
                                blocksToSearch.Push(x - 1);
                                blocksToSearch.Push(y);
                                isMarked[x - 1, y] = true;
                            }
                            if (y != Constants.LEVEL_COLUMNS - 1 && levelMap[x, y + 1].getColor() == searchBlock.getColor() && !isMarked[x, y + 1]) {
                                blocksToSearch.Push(x);
                                blocksToSearch.Push(y + 1);
                                isMarked[x, y + 1] = true;
                            }
                            if (y != 0 && levelMap[x, y - 1].getColor() == searchBlock.getColor() && !isMarked[x, y - 1]) {
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
            return numberOfRoomsThatMeetThreshold / (double) numberOfRooms;
        }
    }
}