using Microsoft.Xna.Framework;

namespace CaveGame.Cavegen {
    public abstract class Block {
        private Color color;

        public Block(Color color) {
            this.color = color;
        }

        public Color getColor() {
            return color;
        }

        public abstract int id { get; }
    }

    public class SolidBlock : Block {
        public SolidBlock() : base(Constants.SOLID_BLOCK_COLOR) {

        }

        public override int id => (int) Constants.Id.Solid;
    }

    public class AirBlock : Block {
        public AirBlock() : base(Constants.AIR_BLOCK_COLOR) {

        }
        
        public override int id => (int)Constants.Id.Air;
    }

    public class EntranceBlock : Block {
        public EntranceBlock() : base(Constants.ENTRANCE_BLOCK_COLOR) {

        }
        
        public override int id => (int)Constants.Id.Entrance;
    }

    public class TorchBlock : Block {
        public TorchBlock() : base(Constants.TORCH_BLOCK_COLOR) {

        }

        public override int id => (int)Constants.Id.Torch;
    }
}
