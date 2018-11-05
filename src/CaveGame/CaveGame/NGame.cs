using CaveGame.Scenes;
using Glint;

namespace CaveGame {
    public class NGame : GlintCore {
        public NGame() : base(Constants.GAME_WIDTH, Constants.GAME_HEIGHT, false, "CaveGame") { }

        protected override void Initialize() {
            base.Initialize();
            
            scene = new DemoScene();
        }
    }
}