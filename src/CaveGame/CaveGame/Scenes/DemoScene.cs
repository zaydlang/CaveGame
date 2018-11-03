using System;
using CaveGame.Cavegen;
using CaveGame.Components;
using Microsoft.Xna.Framework;
using Nez;

namespace CaveGame.Scenes {
    public class DemoScene : Scene {
        public VirtualButton leftClick;
        public VirtualButton rightClick;

        public CaveView caveView;

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
            caveView = new CaveView();
            caveViewEntity.addComponent(caveView);

            leftClick = new VirtualButton();
            leftClick.nodes.Add(new Nez.VirtualButton.MouseLeftButton());

            rightClick = new VirtualButton();
            rightClick.nodes.Add(new Nez.VirtualButton.MouseRightButton());
        }

        public override void update() {
            base.update();

            Vector2 mouseLocation = Nez.Input.scaledMousePosition;
            
            if (leftClick.isDown) {
                caveView.level.set((int) (mouseLocation.X / (Constants.DISPLAY_WIDTH / Constants.LEVEL_ROWS)),
                                   (int) (mouseLocation.Y / (Constants.DISPLAY_HEIGHT / Constants.LEVEL_COLUMNS)),
                                   new Block(Color.Black));
            }

            if (rightClick.isDown) {
                caveView.level.set((int)(mouseLocation.X / (Constants.DISPLAY_WIDTH / Constants.LEVEL_ROWS)),
                                   (int)(mouseLocation.Y / (Constants.DISPLAY_HEIGHT / Constants.LEVEL_COLUMNS)),
                                   new Block(Color.White));
            }
        }
    }
}