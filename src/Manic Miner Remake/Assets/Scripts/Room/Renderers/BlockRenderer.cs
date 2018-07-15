using Com.SloanKelly.ZXSpectrum;

public class BlockRenderer : IRenderer
{
    private SpectrumScreen _screen;
    private RoomData _roomData;

    public BlockRenderer(RoomData roomData)
    {
        _roomData = roomData;
    }

    public void Draw()
    {
        _screen.OrDraw();

        for (int y = 0; y < 16; y++)
        {
            for (int x = 0; x < 32; x++)
            {
                int attr = _roomData.Attributes[y * 32 + x];
                if (attr != 0)
                {
                    // HACK: SOmething v. wrong with room #19
                    if (!_roomData.Blocks.ContainsKey(attr)) continue;

                    int ink = attr.GetInk();
                    int paper = attr.GetPaper();
                    bool bright = attr.IsBright();
                    bool flashing = attr.IsFlashing();

                    _screen.SetAttribute(x, y, ink, paper, bright, flashing);

                    if (_roomData.Blocks[attr].BlockType == BlockType.Conveyor)
                    {
                        _screen.DrawSprite(x, y, 1, 1, _roomData.ConveyorShape);
                    }
                    else
                    {
                        _screen.DrawSprite(x, y, 1, 1, _roomData.Blocks[attr].Shape);
                    }
                }
            }
        }
    }

    public void Init(SpectrumScreen screen)
    {
        _screen = screen;
    }
}
