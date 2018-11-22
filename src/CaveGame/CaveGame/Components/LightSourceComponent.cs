using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Tiled;

namespace CaveGame.Components {
    class LightSourceComponent : RenderableComponent, IUpdatable {
        public override RectangleF bounds => new RectangleF(0, 0, Constants.BUFFER_ZONE + Constants.CAVE_WIDTH, Constants.CAVE_HEIGHT);

        List<LightSegment> lightSegments;

        struct LightSegment {
            public Vector2 line { get; set; }
            public bool isCurved { get; set; }

            public LightSegment(Vector2 line, bool isCurved) {
                this.line = line;
                this.isCurved = isCurved;
            }
        }

        public override void onAddedToEntity() {
            base.onAddedToEntity();
            lightSegments = new List<LightSegment>();
        }

        void IUpdatable.update() {
            TiledTileLayer map = (TiledTileLayer) entity.scene.entities.findEntity("map_tiles").getComponent<TiledMapComponent>().tiledMap.getLayer((int) Constants.Layer.Walls);
            float positionX = entity.position.X + Constants.TILE_WIDTH / 2;
            float positionY = entity.position.Y + Constants.TILE_HEIGHT / 2;
            int x1 = (int) (positionX - Constants.LIGHT_STRENGTH) >= 0 ? (int) (positionX - Constants.LIGHT_STRENGTH) : 0;
            int y1 = (int) (positionY - Constants.LIGHT_STRENGTH) >= 0 ? (int) (positionY - Constants.LIGHT_STRENGTH) : 0;
            int x2 = (int) (positionX + Constants.LIGHT_STRENGTH) < Constants.CAVE_WIDTH ? (int) (positionX + Constants.LIGHT_STRENGTH) : Constants.CAVE_WIDTH - 1;
            int y2 = (int) (positionY + Constants.LIGHT_STRENGTH) < Constants.CAVE_HEIGHT ? (int) (positionY + Constants.LIGHT_STRENGTH) : Constants.CAVE_HEIGHT - 1;

            x1 /= Constants.TILE_WIDTH;
            x2 /= Constants.TILE_WIDTH;
            y1 /= Constants.TILE_HEIGHT;
            y2 /= Constants.TILE_HEIGHT;

            lightSegments.Clear();
            for (int i = x1; i < x2; i++) {
                for (int j = y1; j < y2; j++) {
                    TiledTile currentTile = map.getTile(i, j);
                    for (int k = 0; k < 2; k++) {
                        for (int l = 0; l < 2; l++) {
                            if (currentTile != null && currentTile.id == (int)Constants.Id.Solid) {
                                float cornerX = (i + k) * Constants.TILE_WIDTH + Constants.BUFFER_ZONE;
                                float cornerY = (j + l) * Constants.TILE_HEIGHT + Constants.BUFFER_ZONE;
                                float distance = (float) Math.Sqrt((cornerX - positionX) * (cornerX - positionX) + (cornerY - positionY) * (cornerY - positionY));
                                if (distance > Constants.LIGHT_STRENGTH) continue;

                                float rayX = (Constants.LIGHT_STRENGTH / distance * (cornerX - positionX)) + positionX;
                                float rayY = (Constants.LIGHT_STRENGTH / distance * (cornerY - positionY)) + positionY;

                                Vector2 ray = new Vector2(rayX, rayY);
                                RaycastHit hit = Physics.linecast(entity.position, ray);

                                bool isCurved = true;
                                if (hit.collider != null) {
                                    ray = hit.point;
                                    isCurved = false;
                                }

                                lightSegments.Add(new LightSegment(ray, isCurved));
                            }
                        }
                    }
                }
            }
        }

        public override void render(Graphics graphics, Camera camera) {
            var g = graphics.batcher;
            foreach (LightSegment lightSegment in lightSegments) {
                g.drawLine(entity.position, lightSegment.line, Color.Red);
            }
        }
    }
}
