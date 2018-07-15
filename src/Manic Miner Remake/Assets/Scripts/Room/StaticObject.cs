public class StaticObject
{
    public int X { get; set; }

    public int Y { get; set; }

    public int RowOffset { get; set; }

    public byte[] Shape { get; private set; }

    public StaticObject(byte[] shape, int x =0, int y=0)
    {
        X = x;
        Y = y;
        Shape = shape;
    }
}
