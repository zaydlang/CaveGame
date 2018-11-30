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
        public Nez.VirtualButton switchLevels;

        public CaveEditor[] caveEditors;
        public int currentEditor;

        public Entity mapEntity;
        public Entity playerEntity;
        public Entity caveViewEntity;
        public Entity currentLevelDisplayEntity;
        public MarkupText currentLevelDisplayMarkupText;

        enum Mode : int {
            editting,
            playing,
            switching
        }

        public int mode = (int) Mode.editting;

        public override void initialize() {
            base.initialize();

            // setup
            addRenderer(new DefaultRenderer());
            clearColor = Color.White;
            caveEditors = new CaveEditor[Constants.NUMBER_OF_LEVELS];

            // add cave view component
            caveViewEntity = createEntity("cave_view");
            caveEditors[currentEditor] = caveViewEntity.addComponent(new CaveEditor());
            caveEditors[currentEditor].generate();

            // add buttons
            leftClick = new Nez.VirtualButton();
            leftClick.nodes.Add(new Nez.VirtualButton.MouseLeftButton());

            rightClick = new Nez.VirtualButton();
            rightClick.nodes.Add(new Nez.VirtualButton.MouseRightButton());

            play = new Nez.VirtualButton();
            play.nodes.Add(new Nez.VirtualButton.KeyboardKey(Microsoft.Xna.Framework.Input.Keys.P));

            edit = new Nez.VirtualButton();
            edit.nodes.Add(new Nez.VirtualButton.KeyboardKey(Microsoft.Xna.Framework.Input.Keys.E));

            switchLevels = new Nez.VirtualButton();
            switchLevels.nodes.Add(new Nez.VirtualButton.KeyboardKey(Microsoft.Xna.Framework.Input.Keys.N));

            // add other entities
            mapEntity = createEntity("map_tiles");
            playerEntity = createEntity("player");
            currentLevelDisplayEntity = createEntity("current-level-display");

            // setup current level display
            currentLevelDisplayMarkupText = new MarkupText();
            currentLevelDisplayMarkupText.setText("Current Level: 0");
            currentLevelDisplayMarkupText.setColor(Constants.CURRENT_LEVEL_DISPLAY_TEXT_COLOR);
            currentLevelDisplayEntity.addComponent(currentLevelDisplayMarkupText);
            currentLevelDisplayEntity.setPosition(0, 0);
        }

        public override void update() {
            base.update();

            Vector2 mouseLocation = Input.scaledMousePosition;
            
            if (leftClick.isDown) {
                caveEditors[currentEditor].setBlock(mouseLocation.X, mouseLocation.Y);
            }

            if (rightClick.isDown) {
                caveEditors[currentEditor].selectBlock(mouseLocation.X, mouseLocation.Y);
            }

            Console.WriteLine(currentLevelDisplayEntity.position);

            if (play.isDown && mode == (int) Mode.editting) {
                setPlay();
            } else

            if (edit.isDown && mode == (int) Mode.playing) {
                setEdit();
            } else

            if (switchLevels.isPressed && mode != (int) Mode.switching) {
                setSwitch();
            }
        }

        public void setPlay() {
            TiledMap map = caveEditors[currentEditor].level.bake();
            mapEntity.addComponent(new TiledMapComponent(map, "walls"));
            mapEntity.setPosition(Constants.BUFFER_ZONE, Constants.BUFFER_ZONE);

            playerEntity.addComponent(new PlayerComponent());
            playerEntity.addComponent(new TiledMapMover(map.getLayer<TiledTileLayer>("walls")));
            playerEntity.addComponent(new BoxCollider(0, 0, Constants.PLAYER_WIDTH, Constants.PLAYER_HEIGHT));

            Console.WriteLine(caveEditors[currentEditor].level.spawn.X + " " + caveEditors[currentEditor].level.spawn.Y);
            playerEntity.setPosition(caveEditors[currentEditor].level.spawn.X * (Constants.CAVE_HEIGHT / Constants.LEVEL_COLUMNS) + Constants.BUFFER_ZONE,
                                     caveEditors[currentEditor].level.spawn.Y * (Constants.CAVE_HEIGHT / Constants.LEVEL_COLUMNS) + Constants.BUFFER_ZONE);
            mode = (int)Mode.playing;
        }

        public void setEdit() {
            mapEntity.removeComponent<TiledMapComponent>();

            playerEntity.removeComponent<TiledMapMover>();
            playerEntity.removeComponent<BoxCollider>();
            playerEntity.removeComponent<PlayerComponent>();
            mode = (int)Mode.editting;
        }

        public void setSwitch() {
            setEdit();

            mode = (int)Mode.switching;
            currentEditor++;
            if (currentEditor >= Constants.NUMBER_OF_LEVELS) currentEditor = 0;
            caveViewEntity.removeComponent<CaveEditor>();

            if (caveEditors[currentEditor] == null) {
                Console.WriteLine("Creating new Level... " + currentEditor);
                caveEditors[currentEditor] = caveViewEntity.addComponent(new CaveEditor());
                caveEditors[currentEditor].generate();
            } else {
                caveViewEntity.addComponent(caveEditors[currentEditor]);
            }

            Console.WriteLine("Done!");
            currentLevelDisplayMarkupText.setText("Current Level: " + currentEditor);
            mode = (int)Mode.editting;
        }
    }
}