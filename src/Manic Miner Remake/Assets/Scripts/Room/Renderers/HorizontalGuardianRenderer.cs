using Com.SloanKelly.ZXSpectrum;
using System.Collections.Generic;

public class HorizontalGuardianRenderer : IRenderer
{
    private RoomData _roomData;
    private SpectrumScreen _screen;
    private IList<Mob> _mobs;

    public HorizontalGuardianRenderer(RoomData roomData, IList<Mob> mobs)
    {
        _roomData = roomData;
        _mobs = mobs;
    }

    public void Draw()
    {
        foreach (var g in _mobs)
        {
            if (g.Attribute == 0) continue;

            _screen.FillAttribute(g.X, g.Y, 2, 2, g.Attribute.GetInk(), g.Attribute.GetPaper());
            _screen.RowOrderSprite();
            _screen.DrawSprite(g.X, g.Y, 2, 2, _roomData.GuardianGraphics[g.Frame]);
        }
    }

    public void Init(SpectrumScreen screen)
    {
        _screen = screen;
    }
}
