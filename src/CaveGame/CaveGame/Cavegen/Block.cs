using Microsoft.Xna.Framework;

namespace CaveGame.Cavegen {
    public class Block {
        private Color color;

        public Block(Color color) {
            this.color = color;
        }

        public Color getColor() {
            return color;
        }
    }
}
