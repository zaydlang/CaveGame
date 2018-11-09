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
                TiledTileLayer currentLayer = (TiledTileLayer) this.tiledMap.layers[a];
                int width = currentLayer.width;
                int height = currentLayer.height;

                for (int i = 0; i < width; i++) {
                    for (int j = 0; j < height; j++) {
                        if (currentLayer.tiles[i + j * width].id == 3) {
                            TiledTile currentTile = currentLayer.tiles[i + j * width];
                            if (currentTile.id == (int)Id.Torch) {
                                for (int k = i - Constants.TORCH_BLOCK_RADIUS; k < Constants.TORCH_BLOCK_RADIUS * 2 + 1 + i; k++) {
                                    for (int l = j - Constants.TORCH_BLOCK_RADIUS; l < Constants.TORCH_BLOCK_RADIUS * 2 + 1 + j; l++) {
                                        if (k + l * width < 0 || k + l * width >= Constants.LEVEL_ROWS * Constants.LEVEL_COLUMNS) continue;
                                        int distance = (int) Math.Sqrt((k - i) * (k - i) + (j - l) * (j - l));

                                        if (distance < Constants.TORCH_BLOCK_RADIUS) {
                                            ((TiledTileLayer)tiledMap.layers[1]).tiles[k + l * width].id = 2;
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
