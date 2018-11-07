using CaveGame.Cavegen;
using CaveGame.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Tiled;
using Glint;

namespace CaveGame.Scenes {
    public class EditorScene : Scene {
        public Nez.VirtualButton leftClick;
        public Nez.VirtualButton rightClick;
        public Nez.VirtualButton play;
        public Nez.VirtualButton edit;

        public CaveEditor caveEditor;

        public Entity mapEntity;

        public string mode = "editting";

        public override void initialize() {
            base.initialize();

            // setup
            addRenderer(new DefaultRenderer());
            clearColor = Color.White;

            // add cave view component
            var caveViewEntity = createEntity("cave_view");
            caveEditor = caveViewEntity.addComponent(new CaveEditor());
            caveEditor.generate();

            leftClick = new Nez.VirtualButton();
            leftClick.nodes.Add(new Nez.VirtualButton.MouseLeftButton());

            rightClick = new Nez.VirtualButton();
            rightClick.nodes.Add(new Nez.VirtualButton.MouseRightButton());
            
            
            /*
             * How to make a TiledMap
             */

            play = new Nez.VirtualButton();
            play.nodes.Add(new Nez.VirtualButton.KeyboardKey(Microsoft.Xna.Framework.Input.Keys.P));

            edit = new Nez.VirtualButton();
            edit.nodes.Add(new Nez.VirtualButton.KeyboardKey(Microsoft.Xna.Framework.Input.Keys.E));
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

            if (play.isDown && mode == "editting") {
                TiledMap map = new TiledMap(0, Constants.LEVEL_ROWS, Constants.LEVEL_COLUMNS, Constants.TILE_WIDTH, Constants.TILE_HEIGHT);
                Texture2D tilesetTexture = GlintCore.contentSource.Load<Texture2D>("spritesheet");
                TiledTileset tileset = map.createTileset(tilesetTexture, 0, Constants.TILE_WIDTH, Constants.TILE_HEIGHT, true, 0, 0, 3, 3);
                TiledLayer tileLayer = map.createTileLayer("walls", map.width, map.height, caveEditor.level.bake(tileset));
                TiledTile[] tiles = caveEditor.level.bake(tileset);
                mapEntity = this.createEntity("map_tiles");
                mapEntity.setPosition(Constants.BUFFER_ZONE, Constants.BUFFER_ZONE);
                mapEntity.addComponent(new TiledMapComponent(map, "walls"));
                mode = "playing";
            }

            if (edit.isDown && mode == "playing") {
                entities.findEntity("map_tiles").removeComponent<TiledMapComponent>();
                mode = "editting";
            }
        }
    }
}