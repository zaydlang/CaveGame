using CaveGame.Cavegen;
using Microsoft.Xna.Framework;
using Nez;
using Glint;

namespace CaveGame.Components {
    public class CaveView : RenderableComponent {
        public override RectangleF bounds => new RectangleF(0, 0, 1280, 720);

        public Level level;

        public CaveView(Level level) {
            this.level = level;
        }

        public override void render(Graphics graphics, Camera camera) {
            // your rendering logic
            var g = graphics.batcher;
            g.drawRect(20, 20, 100, 100, Color.Blue);
        }
    }
}