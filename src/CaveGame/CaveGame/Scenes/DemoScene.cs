using CaveGame.Cavegen;
using CaveGame.Components;
using Microsoft.Xna.Framework;
using Nez;

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