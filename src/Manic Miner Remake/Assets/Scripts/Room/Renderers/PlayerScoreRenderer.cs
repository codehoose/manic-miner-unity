using Com.SloanKelly.ZXSpectrum;

public class PlayerScoreRenderer : IRenderer
{
    const string ScoreFormat = "High Score {0:000000}   Score {1:000000}";

    private IScoreInformation _scoreInfo;
    private SpectrumScreen _screen;

    public PlayerScoreRenderer(IScoreInformation ctrl)
    {
        _scoreInfo = ctrl;
    }

    public void Draw()
    {
        for (int x = 0; x < 32; x++)
            _screen.SetAttribute(x, 19, 6, 0);

        _screen.PrintMessage(0, 19, string.Format(ScoreFormat, _scoreInfo.HiScore, _scoreInfo.Score));
    }

    public void Init(SpectrumScreen screen)
    {
        _screen = screen;
    }
}
