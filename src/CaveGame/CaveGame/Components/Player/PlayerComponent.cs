using CaveGame.Cavegen;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Tiled;
using System;
using Glint;

namespace CaveGame.Components {
    public class PlayerComponent : RenderableComponent {
        public override RectangleF bounds => new RectangleF(0, 0, 1280, 720);

        public PlayerComponent() {
        }

        public override void render(Graphics graphics, Camera camera) {


        }
    }
}