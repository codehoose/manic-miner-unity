using Com.SloanKelly.ZXSpectrum;

public class StaticObjectRenderer : IRenderer
{
    private StaticObject _so;
    private SpectrumScreen _screen;

    public StaticObjectRenderer(StaticObject so)
    {
        _so = so;
    }

    public void Draw()
    {
        _screen.RowOrderSprite();
        _screen.DrawSpritePP(_so.X, _so.Y, 2, 2, _so.RowOffset, _so.Shape);
    }

    public void Init(SpectrumScreen screen)
    {
        _screen = screen;
    }
}
