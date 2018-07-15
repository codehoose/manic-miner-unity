public class RoomKey
{
    public int Attr { get; set; }

    public CellPoint Position { get; set; }

    public RoomKey(int attr, CellPoint pt)
    {
        Attr = attr;
        Position = pt;
    }
}
