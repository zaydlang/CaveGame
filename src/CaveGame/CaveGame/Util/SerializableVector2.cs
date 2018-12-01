using System;
using Microsoft.Xna.Framework;

namespace CaveGame.Util {
    [Serializable]
    public class SerializableVector2 {
        public float X;
        public float Y;

        public SerializableVector2(Vector2 vector) {
            X = vector.X;
            Y = vector.Y;
        }

        public Vector2 getVector2() {
            return new Vector2(X, Y);
        }
    }
}
