using Com.SloanKelly.ZXSpectrum;

public class PortalRenderer : IRenderer
{
    private SpectrumScreen _screen;
    private RoomData _roomData;

    public PortalRenderer(RoomData roomData)
    {
        _roomData = roomData;
    }

    public void Draw()
    {
        for (int py = 0; py < 2; py++)
        {
            for (int px = 0; px < 2; px++)
            {
                _screen.SetAttribute(_roomData.Portal.X + px, _roomData.Portal.Y + py, _roomData.Portal.Attr);
            }
        }

        _screen.RowOrderSprite();
        _screen.DrawSprite(_roomData.Portal.X, _roomData.Portal.Y, 2, 2, _roomData.Portal.Shape);
    }

    public void Init(SpectrumScreen screen)
    {
        _screen = screen;
    }
}
