using UnityEngine;

public static class BitCollision
{
    public static bool DidCollide2x2(int ax, int ay, byte[] aShape, int bx, int by, byte[] bShape)
    {
        return Mathf.Abs(ax - bx) * 2 < 4 && Mathf.Abs(ay - by) < 4;
    }
}
