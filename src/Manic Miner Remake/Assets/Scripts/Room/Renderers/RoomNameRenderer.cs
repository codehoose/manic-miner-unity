using Com.SloanKelly.ZXSpectrum;

public class RoomNameRenderer : IRenderer
{
    private SpectrumScreen _screen;
    private RoomData _roomData;

    public RoomNameRenderer(RoomData roomData)
    {
        _roomData = roomData;
    }

    public void Draw()
    {
        for (int x = 0; x < 32; x++)
            _screen.SetAttribute(x, 16, 0, 6);

        _screen.PrintMessage(0, 16, _roomData.RoomName);
    }

    public void Init(SpectrumScreen screen)
    {
        _screen = screen;
    }
}
