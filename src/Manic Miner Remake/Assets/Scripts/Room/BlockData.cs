public class BlockData
{
    public byte[] Shape { get; private set; }

    public BlockType BlockType { get; private set; }

    public BlockData(byte[] shape, BlockType type )
    {
        Shape = shape;
        BlockType = type;
    }
}
