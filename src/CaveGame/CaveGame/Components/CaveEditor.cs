using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using CaveGame.Cavegen;
using Microsoft.Xna.Framework;
using Nez;
using Glint;

namespace CaveGame.Components {
    public class CaveEditor : RenderableComponent {
        public override RectangleF bounds => new RectangleF(0, 0, 1280, 720);

        public Level level;

        public Block[] toolbox;
        public Block selectedBlock { get; set; }

        public CaveEditor() {
            toolbox = new Block[] {new SolidBlock(), new AirBlock(), new EntranceBlock(), new WaterBlock(), new ExitBlock()};
            selectedBlock = toolbox[0];
        }

        public void generate() {
            do {
                level = new Level().fillLevel(new SolidBlock())
                    .createRooms(15, 10, 10, 30, 30, new AirBlock())
                    .smooth(new AirBlock(), new SolidBlock())
                    .createRooms(10, 10, 10, 30, 30, new AirBlock())
                    .smooth(new AirBlock(), new SolidBlock())
                    .createRooms(9, 10, 10, 30, 30, new AirBlock())
                    .smooth(new AirBlock(), new SolidBlock())
                    .createRooms(8, 10, 10, 30, 30, new AirBlock())
                    .smooth(new AirBlock(), new SolidBlock())
                    .smooth(new AirBlock(), new SolidBlock());
            } while (false);
            // while level.getDensityScore(4, 4, 0.5, new AirBlock()) < 0.85 ||
            // level.getRoomSizeScore(250, new AirBlock()) < 1 ||
            // level.getRoomSizeScore(30, new SolidBlock()) < 1
        }

        public override void render(Graphics graphics, Camera camera) {
            var g = graphics.batcher;

            g.drawRect(0, 0, Constants.GAME_WIDTH, Constants.GAME_HEIGHT, Constants.BUFFER_COLOR.getColor());
            g.drawRect(Constants.BUFFER_ZONE, Constants.BUFFER_ZONE, Constants.CAVE_WIDTH, Constants.CAVE_HEIGHT, Constants.AIR_BLOCK_COLOR.getColor());

            // water rendering
            for (int i = 0; i < Constants.LEVEL_ROWS; i++) {
                for (int j = Constants.LEVEL_COLUMNS -  level.waterLevel; j < Constants.LEVEL_COLUMNS; j++) {
                    g.drawRect((int)Math.Floor((double)((i) * Constants.CAVE_WIDTH / Constants.LEVEL_ROWS) + Constants.BUFFER_ZONE),
                               (int)Math.Floor((double)((j) * Constants.CAVE_HEIGHT / Constants.LEVEL_COLUMNS) + Constants.BUFFER_ZONE),
                               (int)Math.Floor((double)Constants.CAVE_WIDTH / Constants.LEVEL_ROWS),
                               (int)Math.Floor((double)Constants.CAVE_HEIGHT / Constants.LEVEL_COLUMNS),
                               Constants.WATER_BLOCK_COLOR.getColor());
                }
            }

            // wall rendering
            for (int i = 0; i < Constants.LEVEL_ROWS; i++) {
                for (int j = 0; j < Constants.LEVEL_COLUMNS; j++) {
                    Block currentBlock = level.get(i, j);
                    if (currentBlock.id == (int)Constants.Id.Air) continue;

                    Color currentColor = currentBlock.getColor();
                    g.drawRect((int)Math.Floor((double) ((i) * Constants.CAVE_WIDTH / Constants.LEVEL_ROWS) + Constants.BUFFER_ZONE),
                               (int)Math.Floor((double) ((j) * Constants.CAVE_HEIGHT / Constants.LEVEL_COLUMNS) + Constants.BUFFER_ZONE),
                               (int)Math.Floor((double) Constants.CAVE_WIDTH / Constants.LEVEL_ROWS),
                               (int)Math.Floor((double) Constants.CAVE_HEIGHT / Constants.LEVEL_COLUMNS),
                               currentColor);
                }
            }

            // toolbox rendering
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

            if (xIndex < 0 || yIndex < 0 || xIndex >= Constants.LEVEL_ROWS || yIndex >= Constants.LEVEL_COLUMNS) {
                return;
            }

            Block previousBlock = level.get(xIndex, yIndex);
            level.set(xIndex, yIndex, selectedBlock);

            if (selectedBlock.id == (int) Constants.Id.Water) {
                level.waterLevel = Constants.LEVEL_COLUMNS - yIndex;
            }
        }

        public void serializeLevel() {
            string runDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string cutoff = @"src\CaveGame\CaveGame";
            string levelName = "Test.txt";
            string levelDirectory = @"\Levels\";

            runDirectory = runDirectory.Substring(0, runDirectory.IndexOf(cutoff) + cutoff.Length);
            runDirectory += levelDirectory + levelName;
            Stream stream = new FileStream(runDirectory,
                                     FileMode.Create,
                                     FileAccess.Write, 
                                     FileShare.None);

            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, level);

            stream.Close();
        }

        public void deserializeLevel() {
            string runDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string cutoff = @"src\CaveGame\CaveGame";
            string levelName = "Test.txt";
            string levelDirectory = @"\Levels\";

            runDirectory = runDirectory.Substring(0, runDirectory.IndexOf(cutoff) + cutoff.Length);
            runDirectory += levelDirectory + levelName;
            Stream stream = new FileStream(runDirectory,
                                      FileMode.Open,
                                      FileAccess.Read,
                                      FileShare.Read);
            IFormatter formatter = new BinaryFormatter();
            Level loadedLevel = (Level) formatter.Deserialize(stream);
            level = loadedLevel;

            stream.Close();
        }
    }
}