using System;
using CaveGame.Util;
using Microsoft.Xna.Framework;

namespace CaveGame.Cavegen {
    [Serializable]
    public abstract class Block {
        private SerializableColor SerializableColor;

        public Block(SerializableColor SerializableColor) {
            this.SerializableColor = SerializableColor;
        }

        public Color getColor() {
            return SerializableColor.getColor();
        }

        public abstract int id { get; }
    }

    [Serializable]
    public class SolidBlock : Block {
        public SolidBlock() : base(Constants.SOLID_BLOCK_COLOR) {

        }

        public override int id => (int) Constants.Id.Solid;
    }

    [Serializable]
    public class AirBlock : Block {
        public AirBlock() : base(Constants.AIR_BLOCK_COLOR) {

        }

        public override int id => (int) Constants.Id.Air;
    }

    [Serializable]
    public class EntranceBlock : Block {
        public EntranceBlock() : base(Constants.ENTRANCE_BLOCK_COLOR) {

        }

        public override int id => (int) Constants.Id.Entrance;
    }

    [Serializable]
    public class ExitBlock : Block {
        public ExitBlock() : base(Constants.EXIT_BLOCK_COLOR) {

        }

        public override int id => (int) Constants.Id.Exit;
    }

    [Serializable]
    public class WaterBlock : Block {
        public WaterBlock() : base(Constants.WATER_BLOCK_COLOR) {

        }

        public override int id => (int) Constants.Id.Water;
    }
}
