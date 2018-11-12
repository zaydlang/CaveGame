using System;
using System.Collections.Generic;
using System.Text;
using Nez;
using Nez.Tiled;

namespace CaveGame.Components {
    class LightSourceComponent : Component, IUpdatable{
        public double strength;

        public LightSourceComponent(double strength) {
            this.strength = strength;
        }

        public override void onAddedToEntity() {
            base.onAddedToEntity();
        }

        void IUpdatable.update() {

        }
    }
}
