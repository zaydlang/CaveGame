using System;
using CaveGame.Cavegen;
using CaveGame.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Tiled;
using Glint;
using Nez.Shadows;
using Nez.Textures;

namespace CaveGame.Scenes {
    public class EditorScene : Scene {
        public Nez.VirtualButton leftClick;
        public Nez.VirtualButton rightClick;
        public Nez.VirtualButton play;
        public Nez.VirtualButton edit;

        public CaveEditor caveEditor;

        public Entity mapEntity;
        public Entity playerEntity;

        enum Mode : int {
            editting,
            playing
        }

        public int mode = (int) Mode.editting;

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

            play = new Nez.VirtualButton();
            play.nodes.Add(new Nez.VirtualButton.KeyboardKey(Microsoft.Xna.Framework.Input.Keys.P));

            edit = new Nez.VirtualButton();
            edit.nodes.Add(new Nez.VirtualButton.KeyboardKey(Microsoft.Xna.Framework.Input.Keys.E));

            mapEntity = createEntity("map_tiles");
            playerEntity = createEntity("player");
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

            if (play.isDown && mode == (int) Mode.editting) {
                TiledMap map = caveEditor.level.bake();
                mapEntity.addComponent(new TiledMapComponent(map, "walls"));
                mapEntity.setPosition(Constants.BUFFER_ZONE, Constants.BUFFER_ZONE);

                // create lightsources
                for (int i = 0; i < Constants.LEVEL_ROWS; i++) {
                    for (int j = 0; j < Constants.LEVEL_COLUMNS; j++) {
                        if (caveEditor.level.get(i, j).id == (int)Constants.Id.Torch) {
                            Entity light = createEntity("light");
                            light.setPosition(i * Constants.TILE_WIDTH + Constants.BUFFER_ZONE, 
                                              j * Constants.TILE_HEIGHT + Constants.BUFFER_ZONE);
                            light.addComponent<BoxCollider>();
                            light.addComponent(new PolyLight(100) {
                                color = Color.Yellow * 0.5f
                            });
                        }
                    }
                }

                playerEntity.addComponent(new PlayerComponent());
                playerEntity.addComponent(new TiledMapMover(map.getLayer<TiledTileLayer>("walls")));
                playerEntity.addComponent(new BoxCollider(0, 0, Constants.PLAYER_WIDTH, Constants.PLAYER_HEIGHT));

                Console.WriteLine(caveEditor.level.spawn.X + " " + caveEditor.level.spawn.Y);
                playerEntity.setPosition(caveEditor.level.spawn.X * (Constants.CAVE_HEIGHT / Constants.LEVEL_COLUMNS) + Constants.BUFFER_ZONE, 
                                         caveEditor.level.spawn.Y * (Constants.CAVE_HEIGHT / Constants.LEVEL_COLUMNS) + Constants.BUFFER_ZONE);
                mode = (int) Mode.playing;
            }

            if (edit.isDown && mode == (int) Mode.playing) {
                mapEntity.removeComponent<TiledMapComponent>();
                foreach (Entity light in this.entities.entitiesWithName("light")) light.destroy();
                playerEntity.removeComponent<TiledMapMover>();
                playerEntity.removeComponent<BoxCollider>();
                playerEntity.removeComponent<PlayerComponent>();
                mode = (int) Mode.editting;
            }
        }
    }
}