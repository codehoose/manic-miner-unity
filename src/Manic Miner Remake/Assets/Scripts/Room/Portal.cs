using Com.SloanKelly.ZXSpectrum;

public class Portal 
{
    public int X { get; private set; }

    public int Y { get; private set; }

    public byte[] Shape { get; private set; }

    public ZXAttribute Attr { get; private set; }

    public Portal(byte portalColour, byte[] portalShape, int x, int y)
    {
        X = x;
        Y = y;
        Shape = portalShape;
        Attr = new ZXAttribute(portalColour);
    }
}