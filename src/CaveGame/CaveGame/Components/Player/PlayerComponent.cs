using CaveGame.Cavegen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Tiled;
using System;
using Glint;
using System.Collections.Generic;

namespace CaveGame.Components {
    public class PlayerComponent : RenderableComponent, IUpdatable {
        public override RectangleF bounds => new RectangleF(0, 0, Constants.BUFFER_ZONE + Constants.CAVE_WIDTH, Constants.CAVE_HEIGHT);

        public bool grounded  = false;
        public bool underWater = false;

        TiledMapMover mover;
        BoxCollider collider;
        Vector2 velocity;
        List<CollisionResult> collisions = new List<CollisionResult>();

        public override void onAddedToEntity() {
            base.onAddedToEntity();
            mover = this.getComponent<TiledMapMover>();
            collider = entity.getComponent<BoxCollider>();

            grounded = false;
        }

        void IUpdatable.update() {
            if (Input.isKeyDown(Keys.Left) && !Input.isKeyDown(Keys.Right)) {
                velocity.X = -(float) Constants.PLAYER_SPEED;
            }
            if (Input.isKeyDown(Keys.Right) && !Input.isKeyDown(Keys.Left)) {
                velocity.X = (float) Constants.PLAYER_SPEED;
            }
            if (!Input.isKeyDown(Keys.Left) && !Input.isKeyDown(Keys.Right)) {
                velocity.X = 0;
            }
            if (Input.isKeyDown(Keys.Up) && grounded) {
                Console.WriteLine(grounded);
                grounded = false;
                velocity.Y = -(float) Math.Sqrt(2 * Constants.GRAVITY * Constants.PLAYER_JUMP_HEIGHT);
            }

            grounded = false;
            velocity.Y += (float) Constants.GRAVITY * Time.deltaTime;

            // water collision
            TiledTileLayer waterLayer = (TiledTileLayer)entity.scene.entities.findEntity("map_tiles")
                                                               .getComponent<TiledMapComponent>()
                                                               .tiledMap.getLayer((int)Constants.Layer.Water);
            List<TiledTile> waterTiles = waterLayer.getTilesIntersectingBounds(new Rectangle((int)entity.position.X - Constants.BUFFER_ZONE,
                                                                                             (int)entity.position.Y - Constants.BUFFER_ZONE,
                                                                                             Constants.PLAYER_WIDTH,
                                                                                             Constants.PLAYER_HEIGHT));
            for (int i = 0; i < waterTiles.Count; i++) {
                TiledTile tile = waterTiles[i];

                // Console.WriteLine(underWater + " " + i + " " + waterTiles[i].id);
                // Console.WriteLine(waterTiles.Count);
                if (tile.id == (int) Constants.Id.Water) {
                    velocity.Y -= Constants.BOUYANT_FORCE * Time.deltaTime;
                    
                    if (underWater == false) {
                        if (Math.Abs(velocity.Y) < Constants.DAMPENING_FORCE_UPON_ENTRY) velocity.Y = 0;
                        else velocity.Y -= Constants.DAMPENING_FORCE_UPON_ENTRY;
                        underWater = true;
                    }

                    break;
                }
            }

            // If none of the tiles were water
            if (waterTiles.Count == 0) {
                underWater = false;
            }

            var motion = velocity * Time.deltaTime;

            // wall collision
            collisions.Clear();
            if (collider.collidesWithAnyMultiple(motion, collisions)) {
                for (int i = 0; i < collisions.Count; i++) {
                    motion -= collisions[i].minimumTranslationVector;
                    // Console.WriteLine(collisions[i].minimumTranslationVector);
                    // Console.WriteLine(entity.position);
                    if (collisions[i].normal.Y == 1) {
                        velocity.Y = 0;
                    }

                    if (collisions[i].normal.Y == -1) {
                        velocity.Y = 0;
                        Console.WriteLine("Hit floor!");
                        grounded = true;
                    }
                }
            }

            entity.position += motion;
        }

        public override void render(Graphics graphics, Camera camera) {
            var g = graphics.batcher;
            g.drawRect(entity.position.X,
                       entity.position.Y,
                       Constants.PLAYER_WIDTH,
                       Constants.PLAYER_HEIGHT, 
                       Constants.PLAYER_COLOR);
        }
    }
}