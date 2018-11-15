using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Tiled;

namespace CaveGame.Components {
    class LightSourceComponent : RenderableComponent, IUpdatable {

        List<Vector2> lines;

        public LightSourceComponent() {

        }

        public override void onAddedToEntity() {
            base.onAddedToEntity();
            lines = new List<Vector2>();
        }

        void IUpdatable.update() {
            TiledTileLayer map = (TiledTileLayer) entity.scene.entities.findEntity("map_tiles").getComponent<TiledMapComponent>().tiledMap.getLayer((int) Constants.Layer.Walls);
            int x1 = (int) (entity.position.X - Constants.LIGHT_STRENGTH) >= 0 ? (int) (entity.position.X - Constants.LIGHT_STRENGTH) : 0;
            int y1 = (int) (entity.position.Y - Constants.LIGHT_STRENGTH) >= 0 ? (int) (entity.position.Y - Constants.LIGHT_STRENGTH) : 0;
            int x2 = (int) (entity.position.X + Constants.LIGHT_STRENGTH) < Constants.CAVE_WIDTH ? (int) (entity.position.X - Constants.LIGHT_STRENGTH) : Constants.CAVE_WIDTH - 1;
            int y2 = (int) (entity.position.Y + Constants.LIGHT_STRENGTH) < Constants.CAVE_HEIGHT ? (int) (entity.position.Y + Constants.LIGHT_STRENGTH) : Constants.CAVE_HEIGHT - 1;

            x1 /= Constants.TILE_WIDTH;
            x2 /= Constants.TILE_WIDTH;
            y1 /= Constants.TILE_HEIGHT;
            y2 /= Constants.TILE_HEIGHT;

            lines.Clear();
            for (int i = x1; i < x2; i++) {
                for (int j = y1; j < y2; j++) {
                    TiledTile currentTile = map.getTile(i, j);
                    for (int k = 0; k < 2; k++) {
                        for (int l = 0; l < 2; l++) {
                            if (currentTile.id == (int)Constants.Id.Solid) {
                                Vector2 ray = new Vector2(i + k * Constants.TILE_WIDTH, j + l * Constants.TILE_HEIGHT);
                                RaycastHit hit1 = Physics.linecast(entity.position, ray);

                                if (hit1.collider != null) {
                                    lines.Add(ray);
                                }
                            }

                        }
                    }
                }
            }
        }

        public override void render(Graphics graphics, Camera camera) {
            var g = graphics.batcher;
            foreach (Vector2 line in lines) {
                g.drawLine(entity.position, line, Color.Red);
            }
        }
    }
}
