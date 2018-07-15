using Com.SloanKelly.ZXSpectrum;

public class MinerWillyRenderer : IRenderer
{
    private Mob _player;
    private RoomData _data;
    private SpectrumScreen _screen;

    public MinerWillyRenderer(Mob player, RoomData data)
    {
        _player = player;
        _data = data;
    }

    public void Init(SpectrumScreen screen)
    {
        _screen = screen;
    }

    public void Draw()
    {

        int attr = _data.Attributes[_player.Y * 32 + _player.X];
        attr &= 0xF8; // XXXXX--- - bit pattern
        attr |= 7;// Miner Willy is always white on whatever background we have

        ZXAttribute attribute = new ZXAttribute((byte)attr);

        _screen.FillAttribute(_player.X, _player.Y, 2, 2, attribute);
        _screen.RowOrderSprite();
        _screen.DrawSprite(_player.X, _player.Y, 2, 2, _player.Frames[_player.Frame]);
    }
}
