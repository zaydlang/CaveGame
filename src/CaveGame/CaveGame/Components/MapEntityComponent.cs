using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Nez;

namespace CaveGame.Components {
    class MapEntityComponent : Component {
        Vector2 position;

        public override void onAddedToEntity() {
            position = new Vector2();
        }

        public Vector2 getScreenPosition() {
            Vector2 returnValue;
            returnValue.X = position.X * Constants.TILE_WIDTH + Constants.BUFFER_ZONE;
            returnValue.Y = position.Y * Constants.TILE_HEIGHT + Constants.BUFFER_ZONE;

            return returnValue;
        }

        public Vector2 getMapPosition() {
            Vector2 returnValue;
            returnValue.X = position.X * Constants.TILE_WIDTH;
            returnValue.Y = position.Y * Constants.TILE_HEIGHT;

            return returnValue;
        }

        public Vector2 getTilePosition() {
            return position;
        }

        public void setScreenPosition(Vector2 newPosition) {
            position.X = (newPosition.X - Constants.BUFFER_ZONE) / Constants.TILE_WIDTH;
            position.Y = (newPosition.Y - Constants.BUFFER_ZONE) / Constants.TILE_HEIGHT;
        }

        public void setMapPosition(Vector2 newPosition) {
            position.X = newPosition.X / Constants.TILE_WIDTH;
            position.Y = newPosition.Y / Constants.TILE_HEIGHT;
        }

        public void setTilePosition(Vector2 newPosition) {
            position = newPosition;
        }
    }
}
