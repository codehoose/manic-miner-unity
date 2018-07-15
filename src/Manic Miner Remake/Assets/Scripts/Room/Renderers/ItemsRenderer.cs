using Com.SloanKelly.ZXSpectrum;

public class ItemsRenderer : IRenderer
{
    private RoomData _roomData;
    private SpectrumScreen _screen;

    public ItemsRenderer(RoomData roomData)
    {
        _roomData = roomData;
    }

    public void Draw()
    {
        foreach (var key in _roomData.RoomKeys)
        {
            if (key.Attr == 255) continue;

            int attr = _roomData.Attributes[key.Position.Y * 32 + key.Position.X];
            attr &= 0xF8; // XXXXX--- - bit pattern
            attr |= key.Attr;

            ZXAttribute attribute = new ZXAttribute((byte)attr);

            _screen.SetAttribute(key.Position.X, key.Position.Y, attribute);
            _screen.DrawSprite(key.Position.X, key.Position.Y, 1, 1, _roomData.KeyShape);
        }
    }

    public void Init(SpectrumScreen screen)
    {
        _screen = screen;
    }
}
