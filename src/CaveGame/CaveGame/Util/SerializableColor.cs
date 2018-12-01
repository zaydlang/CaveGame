using System;
using Microsoft.Xna.Framework;

[Serializable]
public class SerializableColor {
    public int R;
    public int G;
    public int B;
    public int A;

    public SerializableColor(Color color) {
        R = color.R;
        G = color.G;
        B = color.B;
        A = color.A;
    }

    public Color getColor() {
        return new Color(R, G, B, A);
    }
}