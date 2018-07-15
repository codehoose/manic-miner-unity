using Com.SloanKelly.ZXSpectrum;

public class GameOverTextRenderer : IRenderer
{
    public static readonly string GameOver = "Game    Over";

    private SpectrumScreen _screen;

    public int[] Ink { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public bool Active { get; set; }
    
    public void Draw()
    {
        if (!Active) return;

        for (int i = 0; i < GameOver.Length; i++)
        {
            if (i < 4 || i > 8)
            {
                _screen.SetAttribute(X + i, Y, Ink[i], 0);
            }
        }

        _screen.OrDraw();
        _screen.PrintMessage(X, Y, GameOver);
        _screen.OverwriteDraw();
    }

    public void Init(SpectrumScreen screen)
    {
        _screen = screen;
    }
}
