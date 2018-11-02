using CaveGame.Scenes;
using Glint;

namespace CaveGame {
    public class NGame : GlintCore {
        public NGame() : base(Constants.DISPLAY_WIDTH, Constants.DISPLAY_HEIGHT, false, "CaveGame") { }

        protected override void Initialize() {
            base.Initialize();
            
            scene = new DemoScene();
        }
    }
}