using CaveGame.Cavegen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Tiled;
using System;
using Glint;

namespace CaveGame.Components {
    public class PlayerComponent : RenderableComponent, IUpdatable {
        public override RectangleF bounds => new RectangleF(0, 0, Constants.BUFFER_ZONE + Constants.CAVE_WIDTH, Constants.CAVE_HEIGHT);

        TiledMapMover mover;
        TiledMapMover.CollisionState collisionState = new TiledMapMover.CollisionState();
        BoxCollider boxCollider;
        Vector2 velocity;

        public override void onAddedToEntity() {
            base.onAddedToEntity();
            mover = this.getComponent<TiledMapMover>();
            boxCollider = entity.getComponent<BoxCollider>();
        }

        void IUpdatable.update() {
            Console.WriteLine("POS: " + entity.transform.position.X + " " + entity.transform.position.Y);
            if (Input.isKeyDown(Keys.Left) && !Input.isKeyDown(Keys.Right)) {
                velocity.X = -(float) Constants.PLAYER_SPEED;
            }
            if (Input.isKeyDown(Keys.Left) && !Input.isKeyDown(Keys.Right)) {
                velocity.X = (float) Constants.PLAYER_SPEED;
            }
            if (!Input.isKeyDown(Keys.Left) && !Input.isKeyDown(Keys.Right)) {
                velocity.X = 0;
            }
            if (Input.isKeyDown(Keys.Up)) {
                velocity.Y = (float) Constants.PLAYER_JUMP * Time.deltaTime;
            }

            velocity.Y += (float) Constants.GRAVITY * Time.deltaTime;
            Console.WriteLine("TIME: " + Time.deltaTime);
            Console.WriteLine("OLD: " + velocity * Time.deltaTime);

            mover.move(velocity * Time.deltaTime, boxCollider, collisionState);

            if (collisionState.left || collisionState.right) {
                velocity.X = 0;
            }
            if (collisionState.above || collisionState.below) {
                velocity.Y = 0;
            }

            Console.WriteLine("NEW: " + velocity * Time.deltaTime);
        }

        public override void render(Graphics graphics, Camera camera) {
            var g = graphics.batcher;
            g.drawRect((entity.transform.position.X * (Constants.CAVE_WIDTH / Constants.LEVEL_ROWS)) + Constants.BUFFER_ZONE,
                       (entity.transform.position.Y * (Constants.CAVE_HEIGHT / Constants.LEVEL_COLUMNS)) + Constants.BUFFER_ZONE,
                       Constants.PLAYER_WIDTH,
                       Constants.PLAYER_HEIGHT, 
                       Constants.PLAYER_COLOR);
        }
    }
}