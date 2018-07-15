using UnityEngine;

public static class CellPointExtensions
{
    public static Vector3 ToVector3(this CellPoint pt)
    {
        return new Vector3(pt.X, pt.Y);
    }
}
