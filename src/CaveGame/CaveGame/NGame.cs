using CaveGame.Scenes;
using Glint;

namespace CaveGame {
    public class NGame : GlintCore {
        public NGame() : base(1280, 720, false, "CaveGame") { }

        protected override void Initialize() {
            base.Initialize();
            
            scene = new DemoScene();
        }
    }
}