using CaveGame.Cavegen;
using Microsoft.Xna.Framework;
using Nez;
using System;
using Glint;

namespace CaveGame.Components {
    public class CaveView : RenderableComponent {
        public override RectangleF bounds => new RectangleF(0, 0, 1280, 720);

        public Level level;

        public CaveView() {
            level = new LevelBuilder().fillLevel(new Block(Color.Black))
                                                   .createRooms(9, 10, 10, 30, 30, new Block(Color.White))
                                                   .smooth(new Block(Color.White), new Block(Color.Black))
                                                   .createRooms(9, 10, 10, 30, 30, new Block(Color.White))
                                                   .smooth(new Block(Color.White), new Block(Color.Black))
                                                   .createRooms(9, 10, 10, 30, 30, new Block(Color.White))
                                                   .smooth(new Block(Color.White), new Block(Color.Black))
                                                   .createRooms(9, 10, 10, 30, 30, new Block(Color.White))
                                                   .smooth(new Block(Color.White), new Block(Color.Black))
                                                   .createRooms(9, 10, 10, 30, 30, new Block(Color.White))
                                                   .smooth(new Block(Color.White), new Block(Color.Black))
                                                   .getLevel();
        }

        public override void render(Graphics graphics, Camera camera) {
            var g = graphics.batcher;

            for (int i = 0; i < Constants.LEVEL_ROWS; i++) {
                for (int j = 0; j < Constants.LEVEL_COLUMNS; j++) {
                    Block currentBlock = level.get(i, j);
                    Color currentColor = currentBlock.getColor();
                    if (i == 0 && j == 0) currentColor = Color.Green;
                    g.drawRect((int)Math.Floor((double) Constants.DISPLAY_WIDTH - ((i + 1) * Constants.DISPLAY_WIDTH / Constants.LEVEL_ROWS)),
                               (int)Math.Floor((double) Constants.DISPLAY_HEIGHT - ((j + 1) * Constants.DISPLAY_HEIGHT / Constants.LEVEL_COLUMNS)),
                               (int)Math.Floor((double) Constants.DISPLAY_WIDTH / Constants.LEVEL_ROWS),
                               (int)Math.Floor((double) Constants.DISPLAY_HEIGHT / Constants.LEVEL_COLUMNS),
                               currentColor);
                }
            }
        }
    }
}