using CaveGame.Cavegen;
using CaveGame.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Tiled;

namespace CaveGame.Scenes {
    public class DemoScene : Scene {
        public VirtualButton leftClick;
        public VirtualButton rightClick;

        public CaveEditor caveEditor;

        public override void initialize() {
            base.initialize();

            // setup
            addRenderer(new DefaultRenderer());
            clearColor = Color.White;
            
            // generate level
            // TODO: generate this according to GoodStuff(TM)
            var level = new Level();

            // add cave view component
            var caveViewEntity = createEntity("cave_view");
            caveEditor = caveViewEntity.addComponent(new CaveEditor());
            caveEditor.generate();

            leftClick = new VirtualButton();
            leftClick.nodes.Add(new VirtualButton.MouseLeftButton());

            rightClick = new VirtualButton();
            rightClick.nodes.Add(new VirtualButton.MouseRightButton());
            
            
            /*
             * How to make a TiledMap
             */
//            var map = new TiledMap(0, Constants.CAVE_WIDTH, Constants.CAVE_HEIGHT, Constants.TILE_SIZE, Constants.TILE_SIZE);
//            var tilesetTexture = default(Texture2D); // TODO: !!! IMPORTANT replace this with an actual tileset texture
//            var tileset = map.createTileset(tilesetTexture, 0, Constants.TILE_SIZE, Constants.TILE_SIZE, true, 0, 0);
//            var tileLayer = map.createTileLayer("walls", map.width, map.height, level.bake(tileset));
        }

        public override void update() {
            base.update();

            Vector2 mouseLocation = Input.scaledMousePosition;
            
            if (leftClick.isDown) {
                caveEditor.setBlock(mouseLocation.X, mouseLocation.Y);
            }

            if (rightClick.isDown) {
                caveEditor.selectBlock(mouseLocation.X, mouseLocation.Y);
            }
        }
    }
}