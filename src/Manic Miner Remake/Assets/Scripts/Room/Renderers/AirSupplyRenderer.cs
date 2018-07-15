using Com.SloanKelly.ZXSpectrum;

public class AirSupplyRenderer : IRenderer
{
    private RoomData _roomData;
    private SpectrumScreen _screen;

    public AirSupplyRenderer(RoomData roomData)
    {
        _roomData = roomData;
    }

    public void Draw()
    {
        // Draw the air supply
        for (int x = 0; x < 10; x++)
            _screen.SetAttribute(x, 17, 7, 2);

        for (int x = 10; x < 32; x++)
            _screen.SetAttribute(x, 17, 7, 4);

        byte[] airBlock = new byte[] { 0, 0, 255, 255, 255, 255, 0, 0 };

        var airSupplyLength = _roomData.AirSupply.Length;
        var airHead = _roomData.AirSupply.Tip;

        for (int x = 0; x < airSupplyLength; x++)
            _screen.DrawSprite(x + 4, 17, 1, 1, airBlock);

        byte[] airTipBlock = new byte[] { 0, 0, (byte)airHead, (byte)airHead, (byte)airHead, (byte)airHead, 0, 0 };
        _screen.DrawSprite(4 + airSupplyLength, 17, 1, 1, airTipBlock);

        _screen.PrintMessage(0, 17, "AIR");
    }

    public void Init(SpectrumScreen screen)
    {
        _screen = screen;
    }
}
