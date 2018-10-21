using CaveGame.Cavegen;
using CaveGame.Components;
using Microsoft.Xna.Framework;
using Nez;

namespace CaveGame.Scenes {
    public class DemoScene : Scene {
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
            var caveView = new CaveView(level);
            caveViewEntity.addComponent(caveView);
        }
    }
}