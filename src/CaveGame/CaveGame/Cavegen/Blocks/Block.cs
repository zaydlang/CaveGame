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

        public override int id => 0;
    }

    public class AirBlock : Block {
        public AirBlock() : base(Constants.AIR_BLOCK_COLOR) {

        }
        
        public override int id => 1;
    }

    public class EnteranceBlock : Block {
        public EnteranceBlock() : base(Constants.ENTERANCE_BLOCK_COLOR) {

        }
        
        public override int id => 2;
    }
}
