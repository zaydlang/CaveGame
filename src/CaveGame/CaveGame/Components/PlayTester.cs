using CaveGame.Cavegen;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Tiled;
using System;
using Glint;

namespace CaveGame.Components {
    public class PlayTester : RenderableComponent {
        public override RectangleF bounds => new RectangleF(0, 0, 1280, 720);
        public TiledMap map;

        public PlayTester(TiledMap map) {
            this.map = map;
        }

        public override void render(Graphics graphics, Camera camera) {
            throw new NotImplementedException();
        }
    }
}