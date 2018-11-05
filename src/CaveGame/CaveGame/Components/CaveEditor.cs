using CaveGame.Cavegen;
using Microsoft.Xna.Framework;
using Nez;
using System;
using Glint;

namespace CaveGame.Components {
    public class CaveEditor : RenderableComponent {
        public override RectangleF bounds => new RectangleF(0, 0, 1280, 720);

        public Level level;

        public Block[] toolbox;
        public Block selectedBlock { get; set; }

        public CaveEditor() {
            do {
                level = new Level().fillLevel(new Block(Color.Gray))
                                    .createRooms(15, 10, 10, 30, 30, new Block(Color.White))
                                    .smooth(new Block(Color.White), new Block(Color.Gray))
                                    .createRooms(10, 10, 10, 30, 30, new Block(Color.White))
                                    .smooth(new Block(Color.White), new Block(Color.Gray))
                                    .createRooms(9, 10, 10, 30, 30, new Block(Color.White))
                                    .smooth(new Block(Color.White), new Block(Color.Gray))
                                    .createRooms(8, 10, 10, 30, 30, new Block(Color.White))
                                    .smooth(new Block(Color.White), new Block(Color.Gray))
                                    .smooth(new Block(Color.White), new Block(Color.Gray));
            } while (level.getDensityScore(4, 4, 0.5, new Block(Color.White)) < 0.85 ||
                     level.getRoomSizeScore(250, new Block(Color.White)) < 1 ||
                     level.getRoomSizeScore(30, new Block(Color.Gray)) < 1);

            toolbox = new Block[] {new Block(Color.Gray), new Block(Color.White), new Block(Color.LightGreen)};
            selectedBlock = toolbox[0];
        }

        public override void render(Graphics graphics, Camera camera) {
            var g = graphics.batcher;

            g.drawRect(0, 0, Constants.GAME_WIDTH, Constants.GAME_HEIGHT, Constants.BUFFER_COLOR);

            for (int i = 0; i < Constants.LEVEL_ROWS; i++) {
                for (int j = 0; j < Constants.LEVEL_COLUMNS; j++) {
                    Block currentBlock = level.get(i, j);
                    Color currentColor = currentBlock.getColor();
                    g.drawRect((int)Math.Floor((double) ((i) * Constants.CAVE_WIDTH / Constants.LEVEL_ROWS) + Constants.BUFFER_ZONE),
                               (int)Math.Floor((double) ((j) * Constants.CAVE_HEIGHT / Constants.LEVEL_COLUMNS) + Constants.BUFFER_ZONE),
                               (int)Math.Floor((double) Constants.CAVE_WIDTH / Constants.LEVEL_ROWS),
                               (int)Math.Floor((double) Constants.CAVE_HEIGHT / Constants.LEVEL_COLUMNS),
                               currentColor);
                }
            }

            for (int i = 0; i < toolbox.Length; i++) {
                Block tool = toolbox[i];
                g.drawRect((int)Math.Floor((double) Constants.GAME_WIDTH) - Constants.BUFFER_ZONE - (Constants.CAVE_WIDTH / Constants.LEVEL_ROWS) * 2,
                           (int)Math.Floor((double) i * Constants.CAVE_HEIGHT / Constants.LEVEL_COLUMNS * 2 + Constants.BUFFER_ZONE),
                           (int)Math.Floor((double) Constants.CAVE_WIDTH / Constants.LEVEL_ROWS * 2),
                           (int)Math.Floor((double) Constants.CAVE_HEIGHT / Constants.LEVEL_COLUMNS * 2),
                           tool.getColor());
            }
        }

        public void selectBlock(double x, double y) {
            if (x > Constants.BUFFER_ZONE * 2 + Constants.CAVE_WIDTH && x < Constants.GAME_WIDTH - Constants.BUFFER_ZONE) {
                int index = (int)(y - Constants.BUFFER_ZONE) / (Constants.CAVE_HEIGHT / Constants.LEVEL_COLUMNS * 2);
                if (index >= 0 && index < toolbox.Length) {
                    selectedBlock = toolbox[index];
                }
            }
        }

        public void setBlock(double x, double y) {
            int xIndex = (int) (x - Constants.BUFFER_ZONE) / (Constants.CAVE_WIDTH / Constants.LEVEL_ROWS);
            int yIndex = (int) (y - Constants.BUFFER_ZONE) / (Constants.CAVE_HEIGHT / Constants.LEVEL_COLUMNS);

            if (xIndex >= 0 && yIndex >= 0 && xIndex < Constants.LEVEL_ROWS && yIndex < Constants.LEVEL_COLUMNS) {
                level.set(xIndex, yIndex, selectedBlock);
            }
        }
    }
}