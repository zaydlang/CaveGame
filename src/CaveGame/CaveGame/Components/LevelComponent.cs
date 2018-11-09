using System;
using System.Collections.Generic;
using System.Text;
using Nez;
using Nez.Tiled;

namespace CaveGame.Components {
    public class LevelComponent : TiledMapComponent, IUpdatable {
        enum Id {
            Solid,
            Air,
            Entrance,
            Torch
        }

        public LevelComponent(TiledMap tiledMap, String name) : base(tiledMap, name) {

        }

        void IUpdatable.update() {
            tiledMap.update();
            for (int a = 0; a < tiledMap.layers.Count; a++) {
                TiledTileLayer currentLayer = (TiledTileLayer)this.tiledMap.layers[a];
                int width = currentLayer.width;
                int height = currentLayer.height;

                for (int i = 0; i < width; i++) {
                    for (int j = 0; j < height; j++) {
                        if (currentLayer.tiles[i + j * width].id == 3) {
                            TiledTile currentTile = currentLayer.tiles[i + j * width];
                            if (currentTile.id == (int)Id.Torch) {
                                for (int k = 0; k < Constants.TORCH_BLOCK_RADIUS * 2 + 1; k++) {
                                    for (int l = 0; l < Constants.TORCH_BLOCK_RADIUS * 2 + 1; l++) {
                                        public double pythag(double a, double b) {

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
